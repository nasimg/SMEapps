using MudBlazor;
using SMEapps.Shared.Components.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Services;

public class ConfirmDialogService
{
    private readonly IDialogService _dialog;

    public ConfirmDialogService(IDialogService dialog)
    {
        _dialog = dialog;
    }

    public async Task<bool> ConfirmAsync(string message)
    {
        var parameters = new DialogParameters
        {
            { "Message", message }
        };

        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,

        };

        var dialog = _dialog.Show<ConfirmDialog>("Confirmation", parameters, options);
        var result = await dialog.Result;

        return !result.Canceled; 
    }
}

