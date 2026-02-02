using Newtonsoft.Json;

namespace PereMaria.GestorHotel.Models;

public class UserModel
{
    
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("role")]
    public string Role { get; set; }
    
    [JsonProperty("password")]
    public string Password { get; set; }
    
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    
    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("photo")]
    public string Photo { get; set; }
    
    public UserModel(string id, string email, string role, string password, string firstName, string lastName, string photo)
    {
        Id = id;
        Email = email;
        Role = role;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Photo = photo;
    }

    public UserModel() {}
}