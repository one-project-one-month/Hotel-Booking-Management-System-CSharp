using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Models.Guest;

public class GuestListReqModel : BaseResponseModel
{
    [JsonProperty("guests")]
    public List<GuestReqModel> guestList { get; set; }
}