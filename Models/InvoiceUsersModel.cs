using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PereMaria.GestorHotel.Models;

public class InvoiceUsersModel
{
    [JsonProperty("bookingId")]
    public string BookingId { get; set; } = "";

    [JsonProperty("invoiceNumber")]
    public string InvoiceNumber { get; set; } = "";

    [JsonProperty("bookingDate")]
    public string BookingDate { get; set; } = "";

    [JsonProperty("totalPrice")]
    public double TotalPrice { get; set; }

    [JsonProperty("roomName")]
    public string RoomName { get; set; } = "";

    [JsonProperty("roomType")]
    public string RoomType { get; set; } = "";
}

