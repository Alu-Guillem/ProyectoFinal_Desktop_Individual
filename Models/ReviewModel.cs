using System.Text.Json.Serialization;

namespace PereMaria.GestorHotel.Models;

public class ReviewModel
{
	[JsonPropertyName("reviewId")] public string? ReviewId { get; set; }

	[JsonPropertyName("_id")] public string? Id { get; set; }

	[JsonPropertyName("bookingId")] public string BookingId { get; set; } = "";

	[JsonPropertyName("userId")] public string UserId { get; set; } = "";

	[JsonPropertyName("roomId")] public string RoomId { get; set; } = "";

	[JsonPropertyName("rate")] public int Rate { get; set; }

	[JsonPropertyName("comment")] public string Comment { get; set; } = "";

	[JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";

	[JsonIgnore] public CustomerModel? Customer { get; set; }

	[JsonIgnore] public RoomModel? Room { get; set; }

	[JsonIgnore]
	public string GuestName =>
		Customer != null
			? $"{Customer.FirstName} {Customer.LastName}".Trim()
			: UserId;

	[JsonIgnore]
	public string RoomName => Room?.Name ?? RoomId;
}