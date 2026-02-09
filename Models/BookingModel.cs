using System;
using System.Text.Json.Serialization;

namespace PereMaria.GestorHotel.Models
{
    public class BookingModel
    {
        [JsonPropertyName("userId")] public string UserId { get; set; } = "";

        [JsonPropertyName("roomId")] public string RoomId { get; set; } = "";

        [JsonPropertyName("startDate")] public string StartDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        [JsonPropertyName("endDate")] public string EndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        [JsonPropertyName("bookingDate")] public string BookingDate { get; set; } = "";

        [JsonPropertyName("occupants")] public int Occupants { get; set; }

        [JsonPropertyName("pricePerNight")] public decimal PricePerNight { get; set; }

        [JsonPropertyName("totalPrice")] public decimal TotalPrice { get; set; }

        [JsonPropertyName("discount")] public int Discount { get; set; }

        [JsonPropertyName("totalNights")] public int TotalNights { get; set; }

        [JsonPropertyName("status")] public string Status { get; set; } = "active";

        [JsonPropertyName("isPaid")] public bool IsPaid { get; set; }

        [JsonPropertyName("bookingId")] public string? BookingId { get; set; }

        public override string ToString()
        {
            return
                $"BookingId: {BookingId}, UserId: {UserId}, RoomId: {RoomId}, StartDate: {StartDate}, EndDate: {EndDate}, BookingDate: {BookingDate}, Occupants: {Occupants}, PricePerNight: {PricePerNight}, TotalPrice: {TotalPrice}, Discount: {Discount}, TotalNights: {TotalNights}, Status: {Status}, IsPaid: {IsPaid}";
        }

        [JsonIgnore] public CustomerModel? Customer { get; set; }

        [JsonIgnore] public RoomModel? Room { get; set; }
    }
}