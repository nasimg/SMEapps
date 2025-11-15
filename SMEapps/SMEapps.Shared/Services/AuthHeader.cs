using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace SMEapps.Shared.Services
{
    //public class AuthHeader : DelegatingHandler
    //{
    //    private readonly ISStore _store;
    //    private readonly IHttpClientFactory _clientFactory;
    //    private readonly IServiceProvider _serviceProvider;

    //    public AuthHeader(ISStore store, IHttpClientFactory clientFactory, IServiceProvider serviceProvider)
    //    {
    //        _store = store;
    //        _clientFactory = clientFactory;
    //        _serviceProvider = serviceProvider;
    //    }

    //    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        var accessToken = await _store.GetAsync<string>("token");
    //        var refreshToken = await _store.GetAsync<string>("refreshToken");

    //        // 1. If access token expired and refresh token exists, attempt refresh using a client that DOES NOT have this handler
    //        if (!string.IsNullOrEmpty(accessToken) && IsTokenExpired(accessToken) && !string.IsNullOrEmpty(refreshToken))
    //        {
    //            // Use a separate named client that does NOT have AuthHeader in its pipeline to avoid recursion
    //            var refreshClient = _clientFactory.CreateClient("ApiClient.Refresh");
    //            var response = await refreshClient.PostAsJsonAsync("/Identity/GetRefreshToken", new { refreshToken = refreshToken }, cancellationToken);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var result = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
    //                if (result == null || string.IsNullOrEmpty(result.Token) || string.IsNullOrEmpty(result.RefreshToken))
    //                {
    //                    await _store.RemoveAsync("token");
    //                    await _store.RemoveAsync("refreshToken");
    //                    // redirect to login if possible
    //                    TryRedirectToLogin();
    //                    throw new UnauthorizedAccessException("Session expired. Please log in again.");
    //                }
    //                await _store.SaveAsync("token", result.Token);
    //                await _store.SaveAsync("refreshToken", result.RefreshToken);
    //                accessToken = result.Token;
    //            }
    //            else
    //            {
    //                await _store.RemoveAsync("token");
    //                await _store.RemoveAsync("refreshToken");
    //                TryRedirectToLogin();
    //                throw new UnauthorizedAccessException("Session expired. Please log in again.");
    //            }
    //        }

    //        // 2. Attach token (if present)
    //        if (!string.IsNullOrEmpty(accessToken))
    //            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    //        // Send the request and inspect the response for 401/403
    //        var serverResponse = await base.SendAsync(request, cancellationToken);

    //        if (serverResponse.StatusCode == HttpStatusCode.Unauthorized || serverResponse.StatusCode == HttpStatusCode.Forbidden)
    //        {
    //            // Clear tokens and redirect to login page when possible
    //            await _store.RemoveAsync("token");
    //            await _store.RemoveAsync("refreshToken");
    //            TryRedirectToLogin();
    //        }

    //        return serverResponse;
    //    }

    //    private void TryRedirectToLogin()
    //    {
    //        try
    //        {
    //            var nav = _serviceProvider.GetService(typeof(NavigationManager)) as NavigationManager;
    //            if (nav != null)
    //            {
    //                var relativePath = nav.ToBaseRelativePath(nav.Uri);
    //                var returnUrl = "/" + (string.IsNullOrEmpty(relativePath) ? "" : relativePath);
    //                nav.NavigateTo($"/identity/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
    //            }
    //        }
    //        catch
    //        {
    //            // ignore - navigation not available on server side
    //        }
    //    }

    //    private bool IsTokenExpired(string token)
    //    {
    //        var handler = new JwtSecurityTokenHandler();
    //        var jwt = handler.ReadJwtToken(token);
    //        return jwt.ValidTo < DateTime.UtcNow;
    //    }

    //    public class TokenResponse
    //    {
    //        public string Token { get; set; } = string.Empty;
    //        public string RefreshToken { get; set; } = string.Empty;
    //    }
    //}

    public class AuthHeader : DelegatingHandler
    {
        private readonly ISStore _store;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceProvider _serviceProvider;

        public AuthHeader(ISStore store, IHttpClientFactory clientFactory, IServiceProvider serviceProvider)
        {
            _store = store;
            _clientFactory = clientFactory;
            _serviceProvider = serviceProvider;
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
                        // redirect to login if possible
                        TryRedirectToLogin();
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
                    TryRedirectToLogin();
                    throw new UnauthorizedAccessException("Session expired. Please log in again.");
                }
            }

            // 2. Attach token (if present)
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send the request and inspect the response for 401/403
            var serverResponse = await base.SendAsync(request, cancellationToken);

            if (serverResponse.StatusCode == HttpStatusCode.Unauthorized || serverResponse.StatusCode == HttpStatusCode.Forbidden)
            {
                // Clear tokens and redirect to login page when possible
                await _store.RemoveAsync("token");
                await _store.RemoveAsync("refreshToken");
                TryRedirectToLogin();
            }

            return serverResponse;
        }

        private void TryRedirectToLogin()
        {
            try
            {
                var nav = _serviceProvider.GetService(typeof(NavigationManager)) as NavigationManager;
                if (nav != null)
                {
                    var relativePath = nav.ToBaseRelativePath(nav.Uri);
                    var returnUrl = "/" + (string.IsNullOrEmpty(relativePath) ? "" : relativePath);
                    nav.NavigateTo($"/identity/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
                }
            }
            catch
            {
                // ignore - navigation not available on server side
            }
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
