using Newtonsoft.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace PereMaria.GestorHotel.Models;

public class RoomModel
{
	[JsonProperty("_id")] public string RoomId { get; set; } = "";

	[JsonProperty("name")] public string Name { get; set; } = "";

    [JsonProperty("type")] public string Type { get; set; } = "";

    [JsonProperty("number")] public int Number { get; set; }

    [JsonProperty("offer")] public int Offer { get; set; }

	[JsonProperty("pricePerNight")] public double PricePerNight { get; set; }

	[JsonProperty("occuped")] public bool Occuped { get; set; }
    [JsonProperty("reserved")] public bool Reserved { get; set; }
    [JsonProperty("maintenance")] public bool Maintenance { get; set; }
    [JsonProperty("maintenanceTime")] public DateTime MaintenanceTime { get; set; }
    [JsonProperty("maintenanceReason")] public string MaintenanceReason { get; set; } = "";

    [JsonProperty("cleaningTime")] public int CleaningTime { get; set; }

    [JsonProperty("closed")] public bool Closed { get; set; }


    [JsonProperty("occupancyLimit")] public int OccupancyLimit { get; set; }

    [JsonProperty("description")] public string Description { get; set; } = "";

    [JsonProperty("image")] public string Image { get; set; } = "";

    [Newtonsoft.Json.JsonIgnore] 
    public string GridImageUrl
    {
        get
        {
 
            if (string.IsNullOrWhiteSpace(Image))
            {
                return "pack://application:,,,/Resources/logo.png";
            }

            if (Image.Contains(":"))
            {
                return Image; 
            }

            string nombreArchivoLimpio = Path.GetFileName(Image);
            return $"http://localhost:3000/uploads/{nombreArchivoLimpio}";
        }
    }

    public String ToString() {
        return $"ID:{RoomId}, Name: {Name}, Type: {Type}, Number: {Number}, Offer: {Offer}, Price: {PricePerNight}, Occuped: {Occuped}, Limit: {OccupancyLimit}, Description: {Description}";
    }
}