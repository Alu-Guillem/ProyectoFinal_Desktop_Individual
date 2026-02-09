using System.Text.Json.Serialization;

namespace PereMaria.GestorHotel.Models;

public class RoomModel
{
	[JsonPropertyName("roomId")] public string RoomId { get; set; } = "";

	[JsonPropertyName("name")] public string Name { get; set; } = "";

	[JsonPropertyName("offer")] public int Offer { get; set; }

	[JsonPropertyName("pricePerNight")] public decimal PricePerNight { get; set; }

	[JsonPropertyName("occupancyLimit")] public int OccupancyLimit { get; set; }
}