using Newtonsoft.Json;
using System.Net.Http;

namespace HotelManagementSystem_Web.DevCode
{
    public  class GetRoomTypeNamesService
    {
        private  readonly HttpClient _httpClient;

        public  GetRoomTypeNamesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public  async Task<List<RoomTypeNameModel>> GetRoomTypeNames()
        {
            var response = await _httpClient.GetAsync("api/RoomType/getroomtypes");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var jsonify = JsonConvert.DeserializeObject<RoomResponse>(jsonString);

                 return jsonify!.RoomTypeList;

            }

            return new List<RoomTypeNameModel>(); //return empty list instead of null

        }
    }
}
