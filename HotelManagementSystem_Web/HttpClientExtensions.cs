using System.Net.Http.Json;

namespace HotelManagementSystem_Web
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchJsonAsync<T>(
            this HttpClient client, string url, T payload, CancellationToken token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = JsonContent.Create(payload)
            };
            return client.SendAsync(request, token);
        }
    }

}
