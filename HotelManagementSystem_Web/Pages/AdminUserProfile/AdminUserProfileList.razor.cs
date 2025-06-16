using HotelManagementSystem_Web.Models;
using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Pages.AdminUserProfile
{
    public partial class AdminUserProfileList
    {
        private List<AdminUserInfoModel> Users = new();
        private List<AdminUserInfoModel> FilteredUsers = new();


        private async Task UserList()
        {
            try
            {
                var res = await httpClient.GetAsync("api/User");
                var jsonStr = await res.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GetAllUserInfoResponseModel>(jsonStr);
                if (dto?.respCode == "200")
                {
                    Users = dto.users;
                    FilteredUsers = Users.ToList();
                }
                else
                {
                    Console.WriteLine("Bad payload:\n" + jsonStr);
                    Users = new();
                    FilteredUsers = new();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await UserList();
        }

   
        private int PageSize = 12;      
        private int CurrentPage = 1;

        private void ClearSearch()
        {
            SearchTerm = string.Empty;
            FilteredUsers.Clear();
            FilteredUsers.AddRange(Users);
            CurrentPage = 1;
        }

        private void OnSearchChanged()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                ClearSearch();
                return;
            }

            var term = SearchTerm.Trim();

            FilteredUsers.Clear();
            FilteredUsers.AddRange(
                Users.Where(u =>
                    (u.UserName ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (u.Email ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase)));

            CurrentPage = 1;
        }

        private string? _searchTerm;
        private string? SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm == value) return;
                _searchTerm = value;
                OnSearchChanged();
            }
        }

        private AdminUserInfoModel? selectedUser;
        private void CloseModal() => selectedUser = null;
        //Pagination
        private int TotalPages => (int)Math.Ceiling((double)FilteredUsers.Count / PageSize);
        private bool CanNext => CurrentPage < TotalPages;
        private bool CanPrevious => CurrentPage > 1;

        private IEnumerable<AdminUserInfoModel> PaginatedUsers =>
            FilteredUsers.Skip((CurrentPage - 1) * PageSize).Take(PageSize);

        private void GoToPage(int p) { CurrentPage = p; }
        private void NextPage() { if (CanNext) CurrentPage++; }
        private void PreviousPage() { if (CanPrevious) CurrentPage--; }

        //temp svg
        private const string InlineAvatar =
        "data:image/svg+xml;base64," +
        "PHN2ZyB3aWR0aD0iMTIwIiBoZWlnaHQ9IjE2MCIgdmlld0JveD0iMCAwIDEyMCAxNjAiIHhtbG5zPSJodHRwOi8vd3d3" +
        "LnczLm9yZy8yMDAwL3N2ZyI+PHJlY3Qgd2lkdGg9IjEyMCIgaGVpZ2h0PSIxNjAiIGZpbGw9IiNlMWUxZTEiIHJ4PSIy" +
        "MCIvPjx0ZXh0IHg9IjYwIiB5PSI4MCIgZmlsbD0iIzhjOGM4YyIgZm9udC1zaXplPSI1MCIgdGV4dC1hbmNob3I9Im1p" +
        "ZGRsZSIgZm9udC1mYW1pbHk9IkFyaWFsLCBzYW5zLXNlcmlmIj8+PC90ZXh0Pjwvc3ZnPg==";

        private string GetImageSrc(AdminUserInfoModel u)
        {
            if (!string.IsNullOrWhiteSpace(u.Image))
            {
                var mime = string.IsNullOrWhiteSpace(u.imageMimeType)
                           ? "image/jpeg"
                           : u.imageMimeType;

                return $"data:{mime};base64,{u.Image}";
            }

            return InlineAvatar;
        }
    }
}
