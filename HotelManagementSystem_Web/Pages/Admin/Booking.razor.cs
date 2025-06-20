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
    private int currentPage = 1;
    private int pageSize = 10;

    private List<RoomTypeNameModel> roomTypes = new();
    private List<RoomModel> roomListRes = new();
    private int totalPages => (int)Math.Ceiling((double)(filteredBookings?.Count ?? 0) / pageSize);
    private bool CanGoBack => currentPage > 1;
    private bool CanGoForward => currentPage < totalPages;

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
        {
            _model.Rooms.Add(roomId);
            RecalculateTotal();
        }
    }

    private void RemoveRoom(Guid roomId)
    {
        if (IsRemovable(roomId) && _model.Rooms.Remove(roomId))
        {
            RecalculateTotal();
        }
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
                _lockedRoomIds.Clear();
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
        _modalMode = ModalMode.Add;
        _lockedRoomIds.Clear();
        _model = new BookingReqModel();
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
    private string _searchTerm = string.Empty;
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            _searchTerm = value;
            ApplyFilter();
        }
    }
    private string _selectedStatus = string.Empty;
    public string SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            ApplyFilter();
        }
    }
    private void ApplyFilter()
    {
        IEnumerable<BookingModel> query = bookingList;

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var term = SearchTerm.Trim().ToLower();
            query = query.Where(b =>
                (b.UserName ?? string.Empty).ToLower().Contains(term) ||
                (b.GuestName ?? string.Empty).ToLower().Contains(term) ||
                (b.GuestPhoneNo ?? string.Empty).ToLower().Contains(term) ||
                (b.GuestNrc ?? string.Empty).ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(SelectedStatus))
        {
            query = query.Where(b =>
                string.Equals(b.BookingStatus, SelectedStatus, StringComparison.OrdinalIgnoreCase));
        }

        filteredBookings = query.ToList();
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

    public async Task ApplyReserve(BookingModel booking)
    {
        _modalMode = ModalMode.Reserve;
        _lockedRoomIds = new HashSet<Guid>(booking.RoomIds);
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
            BookingStatus = "Reserved",
            PaymentType = booking.PaymentType,
            Rooms = booking.RoomIds,
        };
        RecalculateTotal();
        await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
    }

    public async Task ApplyEdit(BookingModel booking)
    {
        _modalMode = ModalMode.Edit;
        _lockedRoomIds = new HashSet<Guid>(booking.RoomIds);
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
            Rooms = booking.RoomIds,
        };
        RecalculateTotal();
        await JS.InvokeVoidAsync("showBootstrapModal", "#bookingModal");
    }

    public async Task Edit()
    {
        try
        {
            var res = await _httpClient.PatchAsJsonAsync("/admin/UpdateBooking", _model);
            var api = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponseModel>(await res.Content.ReadAsStringAsync());
            if (api?.respCode != "200")
            {
                _model = new BookingReqModel();
                await JS.InvokeVoidAsync("hideBootstrapModal", "#bookingModal");
                _lockedRoomIds.Clear();
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private enum ModalMode { Add, Edit, Reserve }
    private ModalMode _modalMode = ModalMode.Add;

    private HashSet<Guid> _lockedRoomIds = new();   

    private bool IsRemovable(Guid roomId)
        => _modalMode == ModalMode.Add || !_lockedRoomIds.Contains(roomId);

    private void RecalculateTotal()
    {
        decimal total = 0;

        foreach (var roomId in _model.Rooms)
        {
            var room = roomListRes.FirstOrDefault(r => r.roomId == roomId);
            if (room is null) continue;

            var roomType = roomTypes.FirstOrDefault(rt => rt.RoomTypeId == room.roomTypeId);
            if (roomType is null) continue;

            total += roomType.Price;
        }

        _model.TotalAmount = total;
        StateHasChanged(); 
    }
}