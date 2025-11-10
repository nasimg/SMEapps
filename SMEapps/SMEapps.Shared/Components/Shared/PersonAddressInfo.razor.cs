//using Microsoft.AspNetCore.Components;
//using MudBlazor;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;

//namespace SMEapps.Shared.Components.Shared
//{
//    public partial class PersonAddressInfo
//    {
//        [Parameter] public string ReferenceType { get; set; } = default!;
//        [Parameter] public int ReferenceId { get; set; }
//        [Parameter] public string AddressType { get; set; } = "Present";

//        private MudForm? _form;
//        private PersonAddressInfo _model = new();

//        protected override async Task OnInitializedAsync()
//        {
//            await LoadAddress();
//        }

//        private async Task LoadAddress()
//        {
//            try
//            {
//                var result = await Http.GetFromJsonAsync<List<PersonAddressInfo>>(
//                    $"sme/api/AddressInfo/GetByReferenceAsync/{ReferenceType}/{ReferenceId}");

//                _model = result?.FirstOrDefault(a => a.AddressType == AddressType)
//                    ?? new PersonAddressInfo
//                    {
//                        ReferenceType = ReferenceType,
//                        ReferenceId = ReferenceId,
//                        AddressType = AddressType
//                    };
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Failed to load address: {ex.Message}");
//            }
//        }

//        private async Task Save()
//        {
//            await _form!.Validate();
//            if (!_form.IsValid)
//                return;

//            try
//            {
//                var response = await Http.PostAsJsonAsync("sme/api/AddressInfo/SaveUpdateAsyn", _model);
//                if (response.IsSuccessStatusCode)
//                    Snackbar.Add("Address saved successfully!", Severity.Success);
//                else
//                    Snackbar.Add("Failed to save address.", Severity.Error);
//            }
//            catch (Exception ex)
//            {
//                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
//            }
//        }

//        private void Reset()
//        {
//            _model = new PersonAddressInfo
//            {
//                ReferenceType = ReferenceType,
//                ReferenceId = ReferenceId,
//                AddressType = AddressType
//            };
//        }
//    }
//}
