using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SMEapps.Shared.Services;

namespace SMEapps.Shared.Pages
{
    public partial class Home
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ISStore SStore { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            // Check for token in client storage — if missing, redirect to login.
            // This avoids blocking prerendering on the server but ensures client navigations require auth.
            var token = await SStore.GetAsync<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                // Force a full navigation to login page so auth state is reset and user can login.
                NavigationManager.NavigateTo($"/identity/login?returnUrl={Uri.EscapeDataString(NavigationManager.ToBaseRelativePath(NavigationManager.Uri))}", true);
                return;
            }

            await InvokeAsync(StateHasChanged);
        }
    }
}
