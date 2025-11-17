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
    private List<ModuleModel> moduleList = new();
    private Module selectedModule;

    private string searchString = "";

    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] ModuleService ModuleService { get; set; } = default!;

    [Inject] ConfirmDialogService ConfirmDialogService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetAll();
    }



    public async Task GetAll()
    {
        try
        {
            moduleList = await ModuleService.GetAllModule();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Something went Wrong! {ex}", Severity.Error);
        }

    }


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
            await GetAll();
        }
    }

    public async Task Delete(int moduleId)
    {
        try
        {
            bool confirmed = await ConfirmDialogService.ConfirmAsync("Are you sure you want to delete?");

            if (!confirmed)
                return;

           bool res =  await ModuleService.DeleteModuleAsync(moduleId);

            if (res)
            {
                Snackbar.Add("Module deleted successfully.", Severity.Success);
            }
            else
            {
                Snackbar.Add("Somethin went Wrong!", Severity.Error);
            }
        }
        catch 
        {
            Snackbar.Add($"Something went Wrong!", Severity.Error);
        }
        finally
        {
            await GetAll();
        }
    }


    public async Task Reset()
    {
        Model = new();
    }

}

