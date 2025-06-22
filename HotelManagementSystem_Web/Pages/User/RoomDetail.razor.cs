using HotelManagementSystem_Web.Layout.Compoments;
using HotelManagementSystem_Web.Models.Room;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Pages.User;

public partial class RoomDetail : ComponentBase
{
   
    [Parameter]
    public string RoomTypeId { get; set; }
    public RoomTypeModel Model { get; set; } = new RoomTypeModel();
    public RoomTypeModel RoomDetails { get; set; } = new RoomTypeModel();
    public bool isLoading { get; set; } = false;
    private AppModal Modal;
    private Guid _roomTypeId;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        await GetRoomTypeById();
        isLoading = false;
    }

    public async Task GetRoomTypeById()
    {
        var res = await _httpClient.GetAsync($"api/RoomType/{RoomTypeId}");
        if (res.IsSuccessStatusCode)
        {
            var jsonStr = await res.Content.ReadAsStringAsync();
            var resModel = JsonConvert.DeserializeObject<RoomTypeResModel>(jsonStr);
            if (resModel.respCode == "200")
            {
                Model = resModel.RoomType;
                var editModel = JsonConvert.SerializeObject(Model);
                RoomDetails = JsonConvert.DeserializeObject<RoomTypeModel>(editModel);
            }
        }
        else
        {
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }
    }
}