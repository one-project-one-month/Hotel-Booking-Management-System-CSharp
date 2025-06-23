using HotelManagementSystem_Web.Models.Room;
using HotelManagementSystem_Web.Models.Room.RoomTypeReqModel;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Pages.User;

public partial class RoomDetail : ComponentBase
{
    [Parameter] 
    public Guid RoomId { get; set; }
    public Guid? RoomTypeId { get; set; }


    public RoomReqModel RoomModel { get; set; } = new();
    public RoomTypeModel RoomTypeModel { get; set; } = new();
    public bool IsLoading { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadRoomData();
        IsLoading = false;
    }

    private async Task LoadRoomData()
    {
        try
        {
            // Get room info
            var roomResponse = await _httpClient.GetAsync($"api/Room/{RoomId}");
            Console.WriteLine($"Calling API: api/Room/{RoomId}");
            if (roomResponse.IsSuccessStatusCode)
            {
                var roomStr = await roomResponse.Content.ReadAsStringAsync();
                var room = JsonConvert.DeserializeObject<RoomReqModel>(roomStr);
                Console.WriteLine($"Room API Response: {room}");
                if (room != null)
                {
                    RoomModel = room;

                    var roomTypeRes = await _httpClient.GetAsync($"api/RoomType/{RoomModel.RoomTypeId}");

                    Console.WriteLine($"Deserialized RoomTypeId: {room?.RoomTypeId}");

                    if (roomTypeRes.IsSuccessStatusCode)
                    {
                        var roomTypeStr = await roomTypeRes.Content.ReadAsStringAsync();
                        var roomType = JsonConvert.DeserializeObject<RoomTypeModel>(roomTypeStr);
                        if (roomType != null)
                            RoomTypeModel = roomType;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
