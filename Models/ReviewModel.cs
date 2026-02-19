using System.Text.Json.Serialization;

namespace PereMaria.GestorHotel.Models;

/// <summary>
/// Representa una reseña almacenada en el backend.
/// </summary>
public class ReviewModel
{
	/// <summary>
	/// Identificador amigable de la reseña.
	/// </summary>
	[JsonPropertyName("reviewId")] public string? ReviewId { get; set; }

	/// <summary>
	/// Identificador interno de MongoDB.
	/// </summary>
	[JsonPropertyName("_id")] public string? Id { get; set; }

	/// <summary>
	/// Identificador de la reserva asociada.
	/// </summary>
	[JsonPropertyName("bookingId")] public string BookingId { get; set; } = string.Empty;

	/// <summary>
	/// Identificador del usuario que emitió la reseña.
	/// </summary>
	[JsonPropertyName("userId")] public string UserId { get; set; } = string.Empty;

	/// <summary>
	/// Identificador de la habitación reseñada.
	/// </summary>
	[JsonPropertyName("roomId")] public string RoomId { get; set; } = string.Empty;

	/// <summary>
	/// Valor numérico de la calificación, admite decimales (0.5 a 5).
	/// </summary>
	[JsonPropertyName("rate")] public double Rate { get; set; }

	/// <summary>
	/// Comentario escrito por el huésped.
	/// </summary>
	[JsonPropertyName("comment")] public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// Fecha de creación de la reseña en formato DD/MM/YYYY.
	/// </summary>
	[JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = string.Empty;

	/// <summary>
	/// Datos del cliente asociados a la reseña.
	/// </summary>
	[JsonIgnore] public CustomerModel? Customer { get; set; }

	/// <summary>
	/// Habitación asociada a la reseña.
	/// </summary>
	[JsonIgnore] public RoomModel? Room { get; set; }

	/// <summary>
	/// Nombre completo del cliente o, en su defecto, el identificador.
	/// </summary>
	[JsonIgnore]
	public string GuestName =>
		Customer != null
			? $"{Customer.FirstName} {Customer.LastName}".Trim()
			: UserId;

	/// <summary>
	/// Nombre de la habitación o el identificador bruto.
	/// </summary>
	[JsonIgnore]
	public string RoomName => Room?.Name ?? RoomId;
}