using HotelManagementSystem_Web.Models.Guest;
using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Pages.Admin
{
    public partial class Guest
    {
        //GuestReqModel _model = new GuestReqModel();

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


        protected override async Task OnInitializedAsync()
        {
           await Task.WhenAll(GuestList());
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
        private int pageSize = 5;

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
