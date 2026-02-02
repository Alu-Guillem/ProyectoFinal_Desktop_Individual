using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;

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

    public string VipText => Vip ? "Sí" : "No";
    
    public CustomerModel(string id, string email, string role, string password, string firstName, string lastName, string photo,  string gender, string dni, string city, bool vip) :
        base(id, email, role, password, firstName, lastName, photo)
    {
        Gender = gender;
        Dni = dni;
        City = city;
        Vip = vip;
    }
    
    public CustomerModel() {}
}