using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PereMaria.GestorHotel.Models;

public class CustomerModel : UserModel
{
    

    
    [JsonProperty("gender")]
    public string Gender { get; set; }
    
    [JsonProperty("dni")]
    public string Dni { get; set; }
    
    [JsonProperty("city")]
    public string City { get; set; }
    
    [JsonProperty("vip")]
    public bool Vip { get; set; }

        
    [JsonProperty("birthDate")]
    public string BirthDate {get; set; }
    
    
    public string VipText => Vip ? "Sí" : "No";
    
    public CustomerModel(string userId, string email, string role, string password, string firstName, string lastName, string photo,  string gender, string dni, string city, bool vip, string birthDate) :
        base(userId, email, role, password, firstName, lastName, photo)
    {
        Gender = gender;
        Dni = dni;
        City = city;
        Vip = vip;
        BirthDate = birthDate;
    }
    
    public CustomerModel() {}

    public CustomerModel(string birthDate)
    {
        BirthDate = birthDate;
    }

}