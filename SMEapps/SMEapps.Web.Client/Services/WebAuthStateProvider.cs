using Microsoft.AspNetCore.Components.Authorization;
using SMEapps.Shared.Services;
using SMEapps.Web.Client.Services.SStore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Anudan.Web.Client.Services;

//public class WebAuthStateProvider : AuthenticationStateProvider
//{
//    private readonly ISStore _store;
//    private readonly IHttpClientFactory _clientFactory;
//    //private readonly IDataCacheService? _dataCacheService;

//    public WebAuthStateProvider(ISStore store, IHttpClientFactory clientFactory)
//    {
//        _store = store;
//        _clientFactory = clientFactory;
//        //_dataCacheService = dataCacheService;
//    }

//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        string? token = null;
//        string? refreshToken = null;

//        try
//        {
//            // Guard against JS interop exceptions during server static rendering by catching them here.
//            token = await _store.GetAsync<string>("token");
//            refreshToken = await _store.GetAsync<string>("refreshToken");
//        }
//        catch
//        {
//            // If localStorage isn't available (prerendering / no JS runtime), treat as anonymous.
//            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        }

//        if (string.IsNullOrEmpty(token))
//            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

//        if (IsTokenExpired(token) && !string.IsNullOrEmpty(refreshToken))
//        {
//            // Attempt to refresh the token
//            var client = _clientFactory.CreateClient("ApiClient");
//            var response = await client.PostAsJsonAsync("Identity/RefreshToken", new { RefreshToken = refreshToken });
//            if (response.IsSuccessStatusCode)
//            {
//                var result = await response.Content.ReadFromJsonAsync<TokenResult>();
//                if (result is not null && !string.IsNullOrEmpty(result.Token))
//                {
//                    await _store.SaveAsync("token", result.Token);
//                    await _store.SaveAsync("refreshToken", result.RefreshToken);
//                    token = result.Token;

//                    // Clear menu cache when token is refreshed to ensure fresh user data
//                    //_dataCacheService?.ClearMenuItems();
//                }
//                else
//                {
//                    await ClearAuthenticationDataAsync();
//                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//                }
//            }
//            else
//            {
//                await ClearAuthenticationDataAsync();
//                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//            }
//        }

//        var handler = new JwtSecurityTokenHandler();
//        var jwt = handler.ReadJwtToken(token);
//        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
//        return new AuthenticationState(new ClaimsPrincipal(identity));
//    }

//    public async Task NotifyUserAuthenticationAsync()
//    {
//        // Clear menu cache when user authenticates to ensure fresh menu data
//        //_dataCacheService?.ClearMenuItems();

//        var authStateTask = Task.FromResult(await GetAuthenticationStateAsync());
//        NotifyAuthenticationStateChanged(authStateTask);
//    }

//    private bool IsTokenExpired(string token)
//    {
//        var handler = new JwtSecurityTokenHandler();
//        var jwt = handler.ReadJwtToken(token);
//        return jwt.ValidTo < DateTime.UtcNow;
//    }

//    public async Task LogoutAsync()
//    {
//        await ClearAuthenticationDataAsync();
//        var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        NotifyAuthenticationStateChanged(Task.FromResult(authState));
//    }

//    private async Task ClearAuthenticationDataAsync()
//    {
//        await _store.RemoveAsync("SMEuser");

//        // Clear all cached data
//        //_dataCacheService?.ClearAllCache();
//    }

//    private class TokenResult
//    {
//        public string? Token { get; set; }
//        public string? RefreshToken { get; set; }
//    }
//}



public class WebAuthStateProvider : AuthenticationStateProvider
{
    private readonly ISStore _store;
    private readonly IHttpClientFactory _clientFactory;
    //private readonly IDataCacheService? _dataCacheService;

    public WebAuthStateProvider(ISStore store, IHttpClientFactory clientFactory)
    {
        _store = store;
        _clientFactory = clientFactory;
        //_dataCacheService = dataCacheService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = null;
        string? refreshToken = null;

        try
        {
            // Guard against JS interop exceptions during server static rendering by catching them here.
            token = await _store.GetAsync<string>("token");
            refreshToken = await _store.GetAsync<string>("refreshToken");
        }
        catch
        {
            // If localStorage isn't available (prerendering / no JS runtime), treat as anonymous.
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        if (IsTokenExpired(token) && !string.IsNullOrEmpty(refreshToken))
        {
            // Attempt to refresh the token
            var client = _clientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("Identity/RefreshToken", new { RefreshToken = refreshToken });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResult>();
                if (result is not null && !string.IsNullOrEmpty(result.Token))
                {
                    await _store.SaveAsync("token", result.Token);
                    await _store.SaveAsync("refreshToken", result.RefreshToken);
                    token = result.Token;

                    // Clear menu cache when token is refreshed to ensure fresh user data
                    //_dataCacheService?.ClearMenuItems();
                }
                else
                {
                    await ClearAuthenticationDataAsync();
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }
            else
            {
                await ClearAuthenticationDataAsync();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task NotifyUserAuthenticationAsync()
    {
        // Clear menu cache when user authenticates to ensure fresh menu data
        //_dataCacheService?.ClearMenuItems();

        var authStateTask = Task.FromResult(await GetAuthenticationStateAsync());
        NotifyAuthenticationStateChanged(authStateTask);
    }

    private bool IsTokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.ValidTo < DateTime.UtcNow;
    }

    public async Task LogoutAsync()
    {
        await ClearAuthenticationDataAsync();
        var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    private async Task ClearAuthenticationDataAsync()
    {
        await _store.RemoveAsync("token");
        await _store.RemoveAsync("userName");
        await _store.RemoveAsync("validity");
        await _store.RemoveAsync("refreshToken");
        await _store.RemoveAsync("userId");
        await _store.RemoveAsync("emailId");
        await _store.RemoveAsync("roleId");
        await _store.RemoveAsync("expiredTime");
        await _store.RemoveAsync("roleName");
        await _store.RemoveAsync("id");


        // Clear all cached data
        //_dataCacheService?.ClearAllCache();
    }

    private class TokenResult
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}