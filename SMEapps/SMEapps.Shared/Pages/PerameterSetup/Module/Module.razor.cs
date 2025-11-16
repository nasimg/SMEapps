using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Pages.PerameterSetup.Module;

public partial class Module : ComponentBase
{
    private MudForm form = default!;
    private ModuleModel Model = new();
    private bool _processing = false;
    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] ModuleService ModuleService { get; set; } = default!;

    public async Task Save()
    {
        _processing = true;
        await form.Validate();

        if (!form.IsValid)
            return;
        try
        {
            await ModuleService.PostModuleAsync(Model);
            Snackbar.Add("Module saved successfully.", Severity.Success);
            await Reset();

        }
        catch (Exception ex)
        {
            Snackbar.Add($"Something went Wrong! {ex}", Severity.Error);
        }
        finally
        {
            _processing = false;
        }
    }


    public async Task Reset()
    {
        Model = new();
    }

}

