using System.Net.Http.Json;

public class UserServices
{
    private readonly HttpClient _http;

    public UserServices(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _http.GetFromJsonAsync<List<User>>("http://localhost:5247/api/User/GetAllUsers");
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _http.GetFromJsonAsync<User>($"http://localhost:5247/api/User/GetUser/{id}");
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("http://localhost:5247/api/User/CreateUser", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var response = await _http.PutAsJsonAsync($"http://localhost:5247/api/User/UpdateUser/{user.Id}", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _http.DeleteAsync($"http://localhost:5247/api/User/DeleteUser/{id}");
        return response.IsSuccessStatusCode;
    }
}
namespace HotelManagementSystem_Web
{
    public class UserService
    {
    }
}
