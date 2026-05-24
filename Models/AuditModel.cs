using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PereMaria.GestorHotel.Models
{
    public class AuditModel
    {
        [JsonProperty("id")] public string HistoryId { get; set; } = "";
        [JsonProperty("bookingId")] public string BookingId { get; set; } = "";
        [JsonProperty("action")] public string Action { get; set; } = ""; 
        [JsonProperty("actorId")] public string ActorId { get; set; } = "";
        [JsonProperty("actorType")] public string ActorType { get; set; } = ""; 
        [JsonProperty("previousState")] public object PreviousState { get; set; } = new object();
        [JsonProperty("newState")] public object? NewState { get; set; } = null;
        [JsonProperty("timestamp")] public DateTime Timestamp { get; set; } = DateTime.Now;

        [JsonIgnore]
        public string BookingName { get; set; } = "Cargando...";

        [JsonIgnore]
        public string ActorName { get; set; } = "Cargando...";

        [JsonIgnore]
        public string ActionFormatted => Action.ToLower() switch
        {
            "create" => "➕ Creación",
            "update" => "📝 Modificación",
            "delete" => "❌ Eliminación",
            "cancel" => "🚫 Cancelación",
            "pay" => "💳 Pago Registrado",
            "extend" => "⏳ Estadía Extendida",
            _ => ActionFormatted,
        };

        [JsonIgnore]
        public string ActorTypeFormatted => ActorType.ToLower() switch
        {
            "customer" => "Cliente",
            "employee" => "Empleado",
            "admin" => "Admin.",
            _ => ActorType
        };




    }
}
