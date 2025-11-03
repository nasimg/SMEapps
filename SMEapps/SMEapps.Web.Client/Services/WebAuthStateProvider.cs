using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using SMEapps.Shared.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Anudan.Web.Client.Services;

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
        var token = await _store.GetAsync<string>("token");
        var refreshToken = await _store.GetAsync<string>("refreshToken");

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
        await _store.RemoveAsync("SMEuser");

        // Clear all cached data
        //_dataCacheService?.ClearAllCache();
    }

    private class TokenResult
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
