using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;

namespace SMEapps.Shared.Services
{
    public class AuthHeader : DelegatingHandler
    {
        private readonly ISStore _store;
        private readonly IHttpClientFactory _clientFactory;

        public AuthHeader(ISStore store, IHttpClientFactory clientFactory)
        {
            _store = store;
            _clientFactory = clientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _store.GetAsync<string>("token");
            var refreshToken = await _store.GetAsync<string>("refreshToken");

            // 1. If access token expired and refresh token exists, attempt refresh using a client that DOES NOT have this handler
            if (!string.IsNullOrEmpty(accessToken) && IsTokenExpired(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                // Use a separate named client that does NOT have AuthHeader in its pipeline to avoid recursion
                var refreshClient = _clientFactory.CreateClient("ApiClient.Refresh");
                var response = await refreshClient.PostAsJsonAsync("/Identity/GetRefreshToken", new { refreshToken = refreshToken }, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
                    if (result == null || string.IsNullOrEmpty(result.Token) || string.IsNullOrEmpty(result.RefreshToken))
                    {
                        await _store.RemoveAsync("token");
                        await _store.RemoveAsync("refreshToken");
                        throw new UnauthorizedAccessException("Session expired. Please log in again.");
                    }
                    await _store.SaveAsync("token", result.Token);
                    await _store.SaveAsync("refreshToken", result.RefreshToken);
                    accessToken = result.Token;
                }
                else
                {
                    await _store.RemoveAsync("token");
                    await _store.RemoveAsync("refreshToken");
                    throw new UnauthorizedAccessException("Session expired. Please log in again.");
                }
            }

            // 2. Attach token (if present)
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.ValidTo < DateTime.UtcNow;
        }

        public class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
        }
    }
}
