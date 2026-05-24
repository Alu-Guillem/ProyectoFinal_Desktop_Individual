using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PereMaria.GestorHotel.Models
{
    public class RoomStatModel : BaseViewModel
    {
        [JsonProperty("roomId")] public string RoomId { get; set; } = "";
        [JsonProperty("name")] public string Name { get; set; } = "";
        [JsonProperty("type")] public string Type { get; set; } = "";
        [JsonProperty("totalBookings")] public int TotalBookings { get; set; }
        [JsonProperty("totalRevenue")] public double TotalRevenue { get; set; }

        [JsonProperty("occuped")] public bool Occuped { get; set; }
        [JsonProperty("reserved")] public bool Reserved { get; set; }
        [JsonProperty("maintenance")] public bool Maintenance { get; set; }
        [JsonProperty("closed")] public bool Closed { get; set; }

    }
}
