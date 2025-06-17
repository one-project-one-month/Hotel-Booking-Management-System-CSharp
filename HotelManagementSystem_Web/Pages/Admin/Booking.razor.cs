using HotelManagementSystem_Web.DevCode;
using HotelManagementSystem_Web.Models;
using HotelManagementSystem_Web.Models.Booking;
using HotelManagementSystem_Web.Models.Room;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace HotelManagementSystem_Web.Pages.Admin;

public partial class Booking
{
    [Inject] public GetRoomTypeNamesService _roomTypeNameService { get; set; }

    BookingReqModel _model = new BookingReqModel();
    private bool showModal = false;
    private bool showActionColumn = false;
    private List<BookingReqModel> bookings = new();
    private List<BookingModel> filteredBookings = new();
    private List<BookingModel> bookingList = new();
    private string selectedStatus = "";
    private int currentPage = 1;
    private int pageSize = 10;


    //private List<RoomTypeModel> roomTypes = new();

    private List<RoomTypeNameModel> roomTypes = new();
    private List<RoomModel> roomListRes = new();
    private List<string> roomTypeNames  = new List<string>();
    private int totalPages => (int)Math.Ceiling((double)(filteredBookings?.Count ?? 0) / pageSize);
    private bool CanGoBack => currentPage > 1;
    private bool CanGoForward => currentPage < totalPages;

    protected override async Task OnInitializedAsync()
    {
        await GetRoomTypesList();
        await GetBookingList();
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            var res = await _httpClient.PostAsJsonAsync("/admin/CreateBooking", _model);
            var jsonStr = await res.Content.ReadAsStringAsync();
            var respModel = JsonConvert.DeserializeObject<BaseResponseModel>(jsonStr);
            if (respModel?.respCode == "200")
            {
                Console.WriteLine("Booking created successfully");
                _model = new BookingReqModel();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task GetBookingList()
    {
        try
        {
            var res = await _httpClient.GetAsync("/admin/Bookings");
            var jsonStr = await res.Content.ReadAsStringAsync();
            var respModel = JsonConvert.DeserializeObject<BookingListResponseModel>(jsonStr);
            if (respModel.RespCode == "200")
            {
                bookingList = respModel.Bookings;
                filteredBookings = bookingList;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task CreateBooking()
    {
        var res = await _httpClient.PostAsJsonAsync("admin/CreateBooking", _model);
        if (res.IsSuccessStatusCode)
        {
            var jsonRes = await res.Content.ReadAsStringAsync();
            var respModel = JsonConvert.DeserializeObject<BookingCreateResModel>(jsonRes);
            if (respModel.respCode == "200")
            {
                await JS.InvokeVoidAsync("hideBootstrapModal", "#bookingModal");
                Console.WriteLine("Booking created successfully");
                await GetBookingList();
                _model = new BookingReqModel();
        roomTypeNames = new List<string>();
                StateHasChanged();
            }
            else
            {
                Console.WriteLine(jsonRes);
            }
        }
    }
    private async Task ShowAddBookingModal()
    {
        _model = new BookingReqModel();
        roomTypeNames = new List<string>();
        await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
    }

    private async Task GetRoomTypesList()
    {
        roomTypes = await _roomTypeNameService.GetRoomTypeNames();
    }

    private async Task OnRoomTypeChanged(ChangeEventArgs? e)
    {
        var roomTypeIdStr = e?.Value.ToString();
        if (Guid.TryParse(roomTypeIdStr, out Guid selectedRoomTypeId))
        {
            await GetRoomList();
            AddRoomIdToBooking(selectedRoomTypeId);

            var json = JsonConvert.SerializeObject(_model);
            Console.WriteLine(json);
        }
        else
        {
            Console.WriteLine("Invalid Room Type ID");
        }
    }
    

    private void AddRoomIdToBooking(Guid roomTypeId)
    {
        
        var roomId = roomListRes.Where(x => x.roomTypeId == roomTypeId && x.roomStatus).Select(x => x.roomId)
            .FirstOrDefault();
 
        _model.Rooms.Add(roomId);
        var roomType = roomListRes
            .Where(room => _model.Rooms.Contains(room.roomId))
            .Join(roomTypes,
                room => room.roomTypeId.ToString(),
                type => type.RoomTypeId,
                (room, type) => type.RoomTypeName)
            .FirstOrDefault();
        roomTypeNames.Add(roomType);
    }

    
    private async Task GetRoomList()
    {
        var res = await _httpClient.GetAsync("api/Room/getrooms");
        if (res.IsSuccessStatusCode)
        {
            var jsonRes = await res.Content.ReadAsStringAsync();
            var resModel = JsonConvert.DeserializeObject<RoomListResModel>(jsonRes)!;
            roomListRes = resModel.RoomList;
        }
    }

    //private void ApplyFilter()
    //{
    //    filteredBookings = bookings
    //        .Where(b =>
    //            string.IsNullOrEmpty(selectedStatus) ||
    //            b.BookingStatus?.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase) == true
    //        )
    //        .ToList();

    //    currentPage = 1;
    //}

    private void ToggleActionColumn() => showActionColumn = !showActionColumn;

    private void PreviousPage()
    {
        if (CanGoBack)
            currentPage--;
    }

    private void NextPage()
    {
        if (CanGoForward)
            currentPage++;
    }

    private void OpenEditModal(BookingReqModel booking)
    {
        _model = booking;
    }

    private async Task DeleteBooking(Guid? bookingId)
    {
        if (bookingId == null)
            return;

        var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure to delete this booking?");
        if (!confirmed) return;

        var response = await _httpClient.DeleteAsync($"/Bookings/createbookingbyadmin/{bookingId}");
    }
}