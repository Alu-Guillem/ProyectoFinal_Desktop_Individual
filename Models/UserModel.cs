using Newtonsoft.Json;

namespace PereMaria.GestorHotel.Models;

public class UserModel : BaseViewModel
{
    
    [JsonProperty("userId")]
    public string UserId { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("role")]
    public string Role { get; set; }
    
    private string _password;
    [JsonProperty("password")]
    public string Password 
    { 
        get => _password; 
        set { _password = value; OnPropertyChanged(nameof(Password)); } 
    }
    
    private string _firstName;
    [JsonProperty("firstName")]
    public string FirstName 
    { 
        get => _firstName; 
        set { _firstName = value; OnPropertyChanged(nameof(FirstName)); } 
    }
    
    private string _lastName;
    [JsonProperty("lastName")]
    public string LastName 
    { 
        get => _lastName; 
        set { _lastName = value; OnPropertyChanged(nameof(LastName)); } 
    }

    [JsonProperty("photo")]
    public string Photo { get; set; }
    
    public UserModel(string userId, string email, string role, string password, string firstName, string lastName, string photo)
    {
        UserId = userId;
        Email = email;
        Role = role;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Photo = photo;
    }

    public UserModel() {}
}