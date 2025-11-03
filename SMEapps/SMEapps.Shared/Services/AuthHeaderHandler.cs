using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;

namespace SMEapps.Shared.Services
{
    public class AuthHeaderHandler: DelegatingHandler
    {
        private readonly ISStore _store;
        private readonly IHttpClientFactory _clientFactory;

        public AuthHeaderHandler(ISStore store, IHttpClientFactory clientFactory)
        {
            _store = store;
            _clientFactory = clientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _store.GetAsync<string>("token");
            var refreshToken = await _store.GetAsync<string>("refreshToken");

            // 1. Check if access token is expired
            if (!string.IsNullOrEmpty(accessToken) && IsTokenExpired(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                // 2. Attempt to refresh the token
                var client = _clientFactory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("/Identity/GetRefreshToken", new { refreshToken = refreshToken });
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    if (result == null || string.IsNullOrEmpty(result.Token) || string.IsNullOrEmpty(result.RefreshToken))
                    {
                        // 4. Refresh failed, clear tokens and handle logout
                        await _store.RemoveAsync("token");
                        await _store.RemoveAsync("refreshToken");
                        // Optionally: throw or redirect to login
                        throw new UnauthorizedAccessException("Session expired. Please log in again.");
                    }
                    await _store.SaveAsync("token", result.Token);
                    await _store.SaveAsync("refreshToken", result.RefreshToken);
                    accessToken = result.Token;
                }
                else
                {
                    // 4. Refresh failed, clear tokens and handle logout
                    await _store.RemoveAsync("token");
                    await _store.RemoveAsync("refreshToken");
                    // Optionally: throw or redirect to login
                    throw new UnauthorizedAccessException("Session expired. Please log in again.");
                }
            }

            // 3. Attach the (new or existing) access token
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
