using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PereMaria.GestorHotel.Models
{
    public class RoomOccupancyModel
    {
        [JsonProperty("totalBookings")] public int TotalBookings { get; set; }
        [JsonProperty("month")] public int Month { get; set; }
        [JsonProperty("totalRevenue")] public int TotalRevenue { get; set; }
        [JsonProperty("avgOccupants")] public double AvgOccupants { get; set; }
        [JsonProperty("avgNights")] public double AvgNights { get; set; }
        public string MonthName => Month switch
        {
            1 => "Enero",
            2 => "Febrero",
            3 => "Marzo",
            4 => "Abril",
            5 => "Mayo",
            6 => "Junio",
            7 => "Julio",
            8 => "Agosto",
            9 => "Septiembre",
            10 => "Octubre",
            11 => "Noviembre",
            12 => "Diciembre",
            _ => $"Mes {Month}"
        };

    }
}
