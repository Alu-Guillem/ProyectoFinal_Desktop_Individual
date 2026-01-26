namespace PereMaria.GestorHotel.Models;

public class BookingModel(string client)
{
    public string Client { get; set; } = client;
}