namespace PereMaria.GestorHotel.Models;

public class RoomModel
{
    public string id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public int number { get; set; }
    public double pricePerNight { get; set; }
    public bool occuped { get; set; }
    public int occupancyLimit { get; set; }

}