using HotelManagementSystem_Web.Models;
using HotelManagementSystem_Web.Models.Guest;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace HotelManagementSystem_Web.Pages.Admin
{
    public partial class Guest
    {
        CheckOutReqModel _model = new CheckOutReqModel();

        private List<GuestReqModel> guestList = new();
        private List<GuestReqModel> filterguestList = new();
        private async Task GuestList()
        {
            try
            {
                var res = await _httpClient.GetAsync("api/Guest/GetGuestList");
                var jsonStr = await res.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GuestListReqModel>(jsonStr);
                if (dto?.respCode == "200")
                {
                    guestList = dto.guestList;
                    filterguestList = guestList.ToList();
                }
                else
                {
                    Console.WriteLine("Bad payload:\n" + jsonStr);
                    guestList = new();
                    filterguestList = new();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task GoCheckOut(Guid guestId)
        {
            try
            {
                _model.GuestId = guestId;
                var res = await _httpClient.PostAsJsonAsync("/api/CheckInAndCheckOut/checkout", _model);
                Console.WriteLine(JsonConvert.SerializeObject(_model));
                if (res.IsSuccessStatusCode)
                {
                    var jsonStr = await res.Content.ReadAsStringAsync();
                    var respModel = JsonConvert.DeserializeObject<CheckOutRespModel>(jsonStr);
                    if (respModel?.respCode == "200")
                    {
                        _model = new CheckOutReqModel();
                        await GuestList();
                        await ShowInvoiceAsync(respModel.GuestId);
                        StateHasChanged();
                    }
                }
                else
                {
                    Console.WriteLine(res.ToString());
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<Invoice>? _invoices;    
        private Invoice? _modalInvoice;      
        private Guid? _selectedGuest;       


        protected override async Task OnInitializedAsync()
        {
           await GuestList();
        }

        private async Task EnsureInvoicesAsync()
        {
            if (_invoices is not null) return;          

            var res = await _httpClient.GetAsync("api/Invoices/all");
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                _invoices = JsonConvert.DeserializeObject<List<Invoice>>(json)
                            ?? new List<Invoice>();
            }
            else
            {
                _invoices = new List<Invoice>();       
            }
        }

        private async Task ShowInvoiceAsync(Guid guestId)
        {
            await EnsureInvoicesAsync();                

            _selectedGuest = guestId;
            _modalInvoice = _invoices!
                            .FirstOrDefault(i => i.GuestId == guestId);
        }

        private void CloseModal()
        {
            _selectedGuest = null;
            _modalInvoice = null;
        }

        private string? _searchTerm;
        private string? SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm == value) return;
                _searchTerm = value;
                FilterGuest();          
            }
        }
        private int currentPage = 1;
        private int pageSize = 10;

        private int totalPages => (int)Math.Ceiling((double)(filterguestList?.Count ?? 0) / pageSize);
        private bool CanGoNext => currentPage < totalPages;
        private bool CanGoPrevious => currentPage > 1;

        private void HandleFilter()
        {
            
        }

        private void FilterGuest()
        {
            if (guestList.Count == 0)
                return;
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                filterguestList = guestList.ToList();    
            }
            else
            {
                var term = SearchTerm.Trim();              
                filterguestList = guestList
                    .Where(g =>
                        (g.name ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (g.phoneNo ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (g.email ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (g.nrc ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            currentPage = 1;
        }

        private void ClearSearch()
        {
            SearchTerm = string.Empty;      // erase text
            filterguestList = guestList.ToList(); // restore full list
            currentPage = 1;
        }

        private void NextPage()
        {
            if (CanGoNext) currentPage++;
        }

        private void PreviousPage()
        {
            if (CanGoPrevious) currentPage--;
        }
    }
}
