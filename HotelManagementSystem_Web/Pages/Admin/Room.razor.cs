using HotelManagementSystem_Web.Models;
using HotelManagementSystem_Web.Models.Room;
using System.Net.Http.Json;
using HotelManagementSystem_Web.Models.Room.RoomTypeReqModel;
using Microsoft.AspNetCore.Components;
using HotelManagementSystem_Web.DevCode;
using Microsoft.JSInterop;

namespace HotelManagementSystem_Web.Pages.Admin
{
    public partial class Room
    {
        [Inject]
        public GetRoomTypeNamesService _roomTypeNameService { get; set; }

        private readonly RoomReqModel _model = new();

        private List<RoomTypeNameModel> roomTypes = new();
        private List<RoomModel> roomList = new();
        private List<RoomModel> filteredRooms = new();

        private readonly RoomFilter filter = new();

        // Pagination
        private int currentPage = 1;
        private const int pageSize = 10;
        private int totalPages => (int)Math.Ceiling((double)filteredRooms.Count / pageSize);
        private bool CanGoNext => currentPage < totalPages;
        private bool CanGoPrevious => currentPage > 1;

        //OnInitialize
        protected override async Task OnInitializedAsync()
        {
            await GetRoomList();
            await GetRoomTypesList();
        }

        // Filtering 
        private string statusString => filter.Status switch
        {
            true => "true",
            false => "false",
            _ => ""
        };
        private void OnRoomNoInput(ChangeEventArgs e)
        {
            filter.RoomNo = e.Value?.ToString();
            ApplyFilter();
        }

        private void OnRoomTypeChanged(Guid? value)
        {
            filter.RoomTypeId = value;
            ApplyFilter();
        }

        private void OnStatusStringChanged(ChangeEventArgs e)
        {
            var v = e.Value?.ToString();
            filter.Status = v switch
            {
                "true" => true,
                "false" => false,
                _ => null
            };
            ApplyFilter();
        }


        private void OnStatusChanged(bool? value)
        {
            filter.Status = value;
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            IEnumerable<RoomModel> q = roomList;

            bool hasRoomNo = !string.IsNullOrWhiteSpace(filter.RoomNo);
            bool hasRoomType = filter.RoomTypeId.HasValue && filter.RoomTypeId.Value != Guid.Empty;
            bool hasStatus = filter.Status is not null;

            if (hasRoomNo)                                  
            {
                q = q.Where(r => r.roomNo.Contains(filter.RoomNo!,
                                                   StringComparison.OrdinalIgnoreCase));
            }
            else if (hasRoomType)                           
            {
                q = q.Where(r => r.roomTypeId == filter.RoomTypeId);

                if (hasStatus)
                    q = filter.Status!.Value ? q.Where(r => r.roomStatus)
                                              : q.Where(r => !r.roomStatus);
            }
            else if (hasStatus)                              
            {
                q = filter.Status!.Value ? q.Where(r => r.roomStatus)
                                          : q.Where(r => !r.roomStatus);
            }

            filteredRooms = q.ToList();
            currentPage = 1;     
            StateHasChanged();
        }

        private void ResetFilters()
        {
            filter.RoomNo = null;
            filter.RoomTypeId = null;
            filter.Status = null;
            ApplyFilter();
        }

        // Api Calls
        private async Task GetRoomList()
        {
            try
            {
                var res = await _httpClient.GetAsync("api/Room/getrooms");
                if (!res.IsSuccessStatusCode) return;

                var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomListResModel>(await res.Content.ReadAsStringAsync());
                if (dto?.respCode == "200")
                {
                    roomList = dto.RoomList;
                    filteredRooms = roomList.ToList();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private async Task GetRoomTypesList() => roomTypes = await _roomTypeNameService.GetRoomTypeNames();

        private async Task HandleValidSubmit()
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("admin/createroom", _model);
                var api = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponseModel>(await res.Content.ReadAsStringAsync());
                if (api?.respCode == "200")
                {
                    _model.RoomNo = string.Empty;
                    _model.RoomStatus = string.Empty;
                    _model.RoomTypeId = Guid.Empty;
                    _model.GuestLimit = 0;
                    _model.IsFeatured = false;

                    await JS.InvokeVoidAsync("hideBootstrapModal", "#addRoomModal");
                    await GetRoomList();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        //  paganition 
        private IEnumerable<RoomModel> PaginatedRooms() =>
            filteredRooms.Skip((currentPage - 1) * pageSize).Take(pageSize);

        private void NextPage() { if (CanGoNext) currentPage++; }
        private void PreviousPage() { if (CanGoPrevious) currentPage--; }

        // Tooggle Feature
        private async Task ToggleFeatureAsync(RoomModel room)
        {
            if (room.IsBusy) return;
            room.IsBusy = true;
            var original = room.isFeatured;
            room.isFeatured = !room.isFeatured;
            StateHasChanged();
            try
            {
                var res = await _httpClient.PatchJsonAsync($"admin/updateroom/{room.roomId}", new { isFeatured = room.isFeatured });
                var api = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponseModel>(await res.Content.ReadAsStringAsync());
                if (api?.respCode != "200") room.isFeatured = original;
            }
            catch { room.isFeatured = original; }
            finally { room.IsBusy = false; StateHasChanged(); }
        }

        // class for filter mapping
        private class RoomFilter
        {
            public string? RoomNo { get; set; }
            public Guid? RoomTypeId { get; set; }
            public bool? Status { get; set; }
        }
    }
}
