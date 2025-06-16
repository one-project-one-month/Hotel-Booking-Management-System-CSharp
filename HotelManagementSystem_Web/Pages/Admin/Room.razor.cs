using HotelManagementSystem_Web.Models;using HotelManagementSystem_Web.Models.Room;using Newtonsoft.Json;using System.Net.Http.Json;using HotelManagementSystem_Web.Models.Room.RoomTypeReqModel;using Microsoft.AspNetCore.Components;using HotelManagementSystem_Web.DevCode;using System.Collections.Generic;namespace HotelManagementSystem_Web.Pages.Admin{    public partial class Room    {        [Inject]
        public GetRoomTypeNamesService _roomTypeNameService { get; set; }



        RoomReqModel _model = new RoomReqModel();
        private List<RoomTypeNameModel> roomTypes = new();
        private List<RoomModel> roomList = new();
        private List<RoomModel> filteredRooms = new();

        private List<RoomTypeNameModel> roomTypes = new();

        private List<RoomReqModel> roomList = new();        private List<RoomReqModel> filteredRooms = new();


        protected override async Task OnInitializedAsync()        {            await GetRoomList();            await GetRoomTypesList();        }

        private async Task HandleValidSubmit()        {            try            {                var res = await _httpClient.PostAsJsonAsync("admin/createroom", _model);                var jsonStr = await res.Content.ReadAsStringAsync();                var respModel = JsonConvert.DeserializeObject<BaseResponseModel>(jsonStr);                if (respModel.respCode == "200")                {                    Console.WriteLine("Success");                    _navigation.NavigateTo("/admin/room");
                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }        }        private async Task GetRoomList()        {            try            {                var res = await _httpClient.GetAsync("api/Room/getrooms");                if (!res.IsSuccessStatusCode) return;

                var json = await res.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<RoomListResModel>(json);

                if (dto?.respCode == "200")          
                {                    roomList = dto.RoomList;    
                    filteredRooms = roomList.ToList();
                }
                else
                {
                    Console.WriteLine("Bad payload:\n" + json);
                    roomList = new();          
                    filteredRooms = new();
                }
            }            catch (Exception ex) { Console.WriteLine(ex); }
        }                private void HandleRoomType(ChangeEventArgs e)        {            var selectedValue = e.Value?.ToString();            if (Guid.TryParse(selectedValue, out Guid selectedRoomTypeId))            {                Console.WriteLine($"Selected Room Type ID: {selectedRoomTypeId}");            }            else            {                Console.WriteLine("Invalid or empty Room Type ID.");            }        }                private async Task GetRoomTypesList()        {            roomTypes = await _roomTypeNameService.GetRoomTypeNames();        }        private string? searchRoomNo;
        private string? searchRoomType;
        private bool? searchRoomStatus;
        private int currentPage = 1;        private int pageSize = 5;        private int totalPages => (int)Math.Ceiling((double)filteredRooms.Count / pageSize);        private bool CanGoNext => currentPage < totalPages;        private bool CanGoPrevious => currentPage > 1;        private void ToggleFeature(RoomReqModel room)        {            _model.IsFeatured = !room.IsFeatured;        }        private void HandleFilter()        {            FilterRooms();        }        private void FilterRooms()        {            var query = roomList.AsQueryable();            if (!string.IsNullOrWhiteSpace(searchRoomNo))            {                query = query.Where(r => r.roomNo.Contains(searchRoomNo, StringComparison.OrdinalIgnoreCase));
            }            if (searchRoomStatus.HasValue)
            {                query = query.Where(r => r.roomStatus == searchRoomStatus);
            }            filteredRooms = query.ToList();            currentPage = 1;        }        private IEnumerable<RoomModel> PaginatedRooms()
        {            return filteredRooms                .Skip((currentPage - 1) * pageSize)                .Take(pageSize);        }        private void NextPage()        {            if (CanGoNext) currentPage++;        }        private void PreviousPage()        {            if (CanGoPrevious) currentPage--;        }        private async Task ToggleFeatureAsync(RoomModel room)
        {
            if (room.IsBusy) return;
            room.IsBusy = true;

            var original = room.isFeatured;
            room.isFeatured = !room.isFeatured;
            StateHasChanged();

            try
            {
                var resp = await _httpClient.PatchJsonAsync(
                               $"admin/updateroom/{room.roomId}",     
                               new { isFeatured = room.isFeatured }); 

                var json = await resp.Content.ReadAsStringAsync();
                var api = JsonConvert.DeserializeObject<BaseResponseModel>(json);

                if (api?.respCode != "200")
                {
                    Console.WriteLine("Patch failed: " + json);
                    room.isFeatured = original;  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Patch error: " + ex.Message);
                room.isFeatured = original;       
            }
            finally
            {
                room.IsBusy = false;
                StateHasChanged();
            }
        }
    }
}