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
    private string selectedStatus = string.Empty;
    private int currentPage = 1;
    private int pageSize = 10;


    //private List<RoomTypeModel> roomTypes = new();

    private List<RoomTypeNameModel> roomTypes = new();
    private List<RoomModel> roomListRes = new();
    private List<string> roomTypeNames  = new List<string>();
    private int totalPages => (int)Math.Ceiling((double)(filteredBookings?.Count ?? 0) / pageSize);
    private bool CanGoBack => currentPage > 1;
    private bool CanGoForward => currentPage < totalPages;

    static BookingReqModel NewBookingModel() => new()
    {
        Rooms = new List<Guid>()
    };

    private IEnumerable<RoomModel> FilteredRooms =>
     SelectedTypeId is null
        ? Enumerable.Empty<RoomModel>()
        : roomListRes.Where(r =>
              r.roomTypeId == SelectedTypeId &&
              r.roomStatus &&
              !_model.Rooms.Contains(r.roomId));

    private IEnumerable<RoomModel> SelectedRoomModels =>
    roomListRes.Where(r => _model.Rooms.Contains(r.roomId));

    private void AddRoom(Guid roomId)
    {
        _model.Rooms ??= new();
        if (!_model.Rooms.Contains(roomId))
            _model.Rooms.Add(roomId);
    }

    private void RemoveRoom(Guid roomId)
    {
        _model.Rooms.Remove(roomId);
    }


    Guid? SelectedTypeId;
    protected override async Task OnInitializedAsync()
    {
        await GetRoomList();
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
                _model = new BookingReqModel();
                SelectedTypeId = null;
                await JS.InvokeVoidAsync("hideBootstrapModal", "#bookingModal");
                StateHasChanged();
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
        SelectedTypeId = Guid.TryParse(e.Value?.ToString(), out var id) ? id : null;
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

    private void ApplyFilter()
    {
        filteredBookings = bookingList
            .Where(b =>
                string.IsNullOrWhiteSpace(selectedStatus) ||
                string.Equals(b.BookingStatus, selectedStatus, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        currentPage = 1;
    }

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

    private async Task  OpenEditModal(BookingReqModel booking)
    {
        _model = booking;
        
        await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
    }

    
    private async Task ApplyReserve(Guid? userId)
    {
        _model = new BookingReqModel();
        _model.UserId = userId;
        await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
    }
    public async Task ApplyEdit(BookingModel booking)
    {
        var editingBookingId = booking.BookingId;
        Console.WriteLine("Editing booking ID: " + editingBookingId);

        _model = new BookingReqModel
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            Name = booking.GuestName,
            Nrc = booking.GuestNrc,
            PhoneNo = booking.GuestPhoneNo,
            GuestCount = booking.GuestCount,
            CheckInTime = booking.CheckInTime,
            CheckOutTime = booking.CheckOutTime,
            DepositAmount = booking.DepositAmount,
            TotalAmount = booking.TotalAmount,
            BookingStatus = booking.BookingStatus,
            PaymentType = booking.PaymentType,
            Rooms = new List<Guid>()
        };
        var res = await _httpClient.PostAsJsonAsync("/admin/UpdateBooking", _model);
        var api = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponseModel>(await res.Content.ReadAsStringAsync());
        if (api?.respCode != "200")
        {
            _model = new BookingReqModel();
            await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
            StateHasChanged();
        }
    }

    public async Task Edit()
    {
        try
        {
            var res = await _httpClient.PostAsJsonAsync("/admin/UpdateBooking", _model);
            var api = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponseModel>(await res.Content.ReadAsStringAsync());
            if (api?.respCode != "200")
            {
                _model = new BookingReqModel();
                await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}