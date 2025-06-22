using HotelManagementSystem_Web.Models.Room;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Pages.User;

public partial class RoomDetail : ComponentBase
{
    [Parameter]
    public Guid RoomId { get; set; }

    private RoomTypeModel? RoomDetails = null;

        private bool isLoading = true; 
    private bool isDataEmpty = false; 

    protected override async Task OnInitializedAsync()
    {
        await LoadRoomDetail();
    }

    private async Task LoadRoomDetail()
    {
        isLoading = true;
        isDataEmpty = false;
        try
        {
            var res = await _httpClient.GetAsync("api/RoomType/getroomtypes");
            if (res.IsSuccessStatusCode)
            {
                var jsonStr = await res.Content.ReadAsStringAsync();
                var resModel = JsonConvert.DeserializeObject<RoomTypeListResModel>(jsonStr);

                if (resModel?.respCode == "200")
                {
                    RoomDetails = resModel.RoomTypeList.FirstOrDefault(r => r.RoomTypeId == RoomId);
                }

                if (RoomDetails == null)
                {
                    isDataEmpty = true;
                }
                else
                {
                    isDataEmpty = false; // API responded but no valid data
                }


            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            isDataEmpty = true; // error treat as no data
        }
        finally
        {
            isLoading = false;
        }
        
    }
}