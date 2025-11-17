using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
namespace SMEapps.Shared.Pages.PerameterSetup.Module;

public partial class Module : ComponentBase
{
    private MudForm form = default!;
    private ModuleModel Model = new();
    private bool _processing = false;
    private List<ModuleModel> moduleList = new();
    private ModuleModel selectedModule;

    private string btnTest = "Save";

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


    //public async Task Save()
    //{
    //    _processing = true;
    //    await form.Validate();

    //    if (!form.IsValid)
    //        return;
    //    try
    //    {
    //        await ModuleService.PostModuleAsync(Model);
    //        Snackbar.Add("Module saved successfully.", Severity.Success);
    //        await Reset();

    //    }
    //    catch (Exception ex)
    //    {
    //        Snackbar.Add($"Something went Wrong! {ex}", Severity.Error);
    //    }
    //    finally
    //    {
    //        _processing = false;
    //        await GetAll();
    //    }
    //}

    public async Task Delete(int moduleId)
    {
        try
        {
            bool confirmed = await ConfirmDialogService.ConfirmAsync("Are you sure you want to delete?");

            if (!confirmed)
                return;

            bool res = await ModuleService.DeleteModuleAsync(moduleId);

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

    private async Task Edit(ModuleModel item)
    {
        btnTest = "Update";
        selectedModule = item;
        Model = new ModuleModel
        {
            ModuleId = item.ModuleId,
            Title = item.Title,
            ModuleName = item.ModuleName,
            IconName = item.IconName,
            MenuPosition = item.MenuPosition,
            IsActive = item.IsActive
        };

        await InvokeAsync(StateHasChanged);
    }



    public async Task Reset()
    {
        Model = new();
        btnTest = "Save";
    }


    private async Task Save()
    {
        _processing = true;

        await form.Validate();

        if (!form.IsValid)
        {
            _processing = false;
            return;
        }

        if (Model.ModuleId > 0)
        {

            var update = await ModuleService.UpdateModuleAsync(Model);
            if (update is not null)
            {
                Snackbar.Add("Module updated successfully!", Severity.Success);
                Reset();
            }


            else
            {
                Snackbar.Add("Module updated Failed!", Severity.Error);
            }
        }
        else
        {
            var res = await ModuleService.PostModuleAsync(Model);
            if (res != null)
            {
                Snackbar.Add("Module saved successfully!", Severity.Success);
                Reset();
            }
            else
            {
                Snackbar.Add("Module Saved Failed!", Severity.Error);
            }

        }

        await GetAll();

        _processing = false;
    }


}

