using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PereMaria.GestorHotel.Models
{
    public class RoomStatModel
    {
        [JsonProperty("roomId")] public string RoomId { get; set; } = "";

        [JsonProperty("name")] public string Name { get; set; } = "";

        [JsonProperty("type")] public string Type { get; set; } = "";

        [JsonProperty("totalBookings")] public int TotalBookings { get; set; }

        [JsonProperty("totalRevenue")] public double TotalRevenue { get; set; }
    }
}
