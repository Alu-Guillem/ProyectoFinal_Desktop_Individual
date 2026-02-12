using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class AuthService
{
    private readonly ApiService _api = ApiService.Instance;


    public async Task<ApiResult<LoginRespone>> Login(string email, string password)
    {
        return await _api.Post<LoginRespone>($"auth/login", new { email, password });
    }
}

public class LoginRespone
{
    public string Token { get; set; }
}