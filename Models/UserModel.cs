using Newtonsoft.Json;

namespace PereMaria.GestorHotel.Models;

public class UserModel(String email, String password)
{
    
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; } = email;
    [JsonProperty("role")]
    public string Role { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; } = password;
    
}