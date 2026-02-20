using System;
using System.Text.Json.Serialization;

namespace PereMaria.GestorHotel.Models;

/// <summary>
/// Representa el contrato de intercambio para reservas entre la aplicación WPF y la API.
/// </summary>
public class BookingModel
{
    /// <summary>
    /// Identificador del usuario propietario de la reserva.
    /// </summary>
    [JsonPropertyName("userId")] public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la habitación reservada.
    /// </summary>
    [JsonPropertyName("roomId")] public string RoomId { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de inicio de la estancia en formato DD/MM/YYYY.
    /// </summary>
    [JsonPropertyName("startDate")] public string StartDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

    /// <summary>
    /// Fecha de fin de la estancia en formato DD/MM/YYYY.
    /// </summary>
    [JsonPropertyName("endDate")] public string EndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

    /// <summary>
    /// Fecha de creación de la reserva.
    /// </summary>
    [JsonPropertyName("bookingDate")] public string BookingDate { get; set; } = string.Empty;

    /// <summary>
    /// Número de ocupantes de la reserva.
    /// </summary>
    [JsonPropertyName("occupants")] public int Occupants { get; set; }

    /// <summary>
    /// Precio por noche calculado por el servidor.
    /// </summary>
    [JsonPropertyName("pricePerNight")] public decimal PricePerNight { get; set; }

    /// <summary>
    /// Precio total de la estancia calculado por el servidor.
    /// </summary>
    [JsonPropertyName("totalPrice")] public decimal TotalPrice { get; set; }

    /// <summary>
    /// Porcentaje de descuento aplicado.
    /// </summary>
    [JsonPropertyName("discount")] public int Discount { get; set; }

    /// <summary>
    /// Número total de noches de la reserva.
    /// </summary>
    [JsonPropertyName("totalNights")] public int TotalNights { get; set; }

    /// <summary>
    /// Estado actual de la reserva (active o canceled).
    /// </summary>
    [JsonPropertyName("status")] public string Status { get; set; } = "active";

    /// <summary>
    /// Indica si la reserva ya fue pagada.
    /// </summary>
    [JsonPropertyName("isPaid")] public bool IsPaid { get; set; }

    /// <summary>
    /// Marca si el recordatorio de check-in fue notificado por el backend.
    /// </summary>
    [JsonPropertyName("checkInNotified")] public bool CheckInNotified { get; set; }

    /// <summary>
    /// Marca si el recordatorio de check-out fue notificado por el backend.
    /// </summary>
    [JsonPropertyName("checkOutNotified")] public bool CheckOutNotified { get; set; }

    /// <summary>
    /// Identificador público de la reserva.
    /// </summary>
    [JsonPropertyName("bookingId")] public string? BookingId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return
            $"BookingId: {BookingId}, UserId: {UserId}, RoomId: {RoomId}, StartDate: {StartDate}, EndDate: {EndDate}, BookingDate: {BookingDate}, Occupants: {Occupants}, PricePerNight: {PricePerNight}, TotalPrice: {TotalPrice}, Discount: {Discount}, TotalNights: {TotalNights}, Status: {Status}, IsPaid: {IsPaid}, CheckInNotified: {CheckInNotified}, CheckOutNotified: {CheckOutNotified}";
    }

    /// <summary>
    /// Cliente asociado (se completa localmente).
    /// </summary>
    [JsonIgnore] public CustomerModel? Customer { get; set; }

    /// <summary>
    /// Habitación asociada (se completa localmente).
    /// </summary>
    [JsonIgnore] public RoomModel? Room { get; set; }
}