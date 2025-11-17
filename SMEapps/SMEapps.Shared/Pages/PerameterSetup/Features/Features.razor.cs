using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMEapps.Shared.Model;
using SMEapps.Shared.Services;
namespace SMEapps.Shared.Pages.PerameterSetup.Features;

public partial class Features : ComponentBase
{
    private MudForm form = default!;
    public FeatureModel Model = new();
    private bool _processing = false;
    private List<FeatureModel> FeatureList = new();
    private FeatureModel selectedFeature;

    private List<ModuleModel> SelectModule = new();

    private string btnTest = "Save";

    private string searchString = "";

    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] FeaturesService FeaturesService { get; set; } = default!;

    [Inject] ConfirmDialogService ConfirmDialogService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetAll();
        await GetSelectModule();
    }



    public async Task GetAll()
    {
        try
        {
            FeatureList = await FeaturesService.GetAllFeatures();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Something went Wrong! {ex}", Severity.Error);
        }

    }


    public async Task Delete(int moduleId)
    {
        try
        {
            bool confirmed = await ConfirmDialogService.ConfirmAsync("Are you sure you want to delete?");

            if (!confirmed)
                return;

            bool res = await FeaturesService.DeleteFeatureAsync(moduleId);

            if (res)
            {
                Snackbar.Add("Feature deleted successfully.", Severity.Success);
            }
            else
            {
                Snackbar.Add("Something went wrong!", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add($"Something went wrong!", Severity.Error);
        }
        finally
        {
            await GetAll();
        }
    }

    private async Task Edit(FeatureModel item)
    {
        btnTest = "Update";
        selectedFeature = item;
        Model = new FeatureModel
        {
            FeatureId = item.FeatureId,
            ModuleId = item.ModuleId,
            FeatureName = item.FeatureName,
            Path = item.Path,
            OrderNo = item.OrderNo,
            IsActive = item.IsActive 
        };

        await InvokeAsync(StateHasChanged);
    }



    public async Task Reset()
    {
        Model = new();
        btnTest = "Save";
    }

    private async Task GetSelectModule()
    {
        SelectModule = await FeaturesService.GetSelectedModule();
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

            var update = await  FeaturesService.UpdateFeatureAsync(Model);
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
            var res = await FeaturesService.PostFeatureAsync(Model);
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

