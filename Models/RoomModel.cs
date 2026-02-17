using System.Text.Json.Serialization;

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

	[JsonPropertyName("roomId")] public string RoomId { get; set; } = "";

	[JsonPropertyName("name")] public string Name { get; set; } = "";

	[JsonPropertyName("offer")] public int Offer { get; set; }

	[JsonPropertyName("pricePerNight")] public decimal PricePerNight { get; set; }

	[JsonPropertyName("occupancyLimit")] public int OccupancyLimit { get; set; }
}