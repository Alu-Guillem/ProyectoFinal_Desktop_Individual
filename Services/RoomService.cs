using PereMaria.GestorHotel.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PereMaria.GestorHotel.Services;

/// <summary>
/// Servicio de habitaciones para operaciones de catálogo y mantenimiento.
/// </summary>
public class RoomService
{
    /*public async Task<ApiResult<List<RoomModel>>> GetAllRooms(string name, bool? occuped)
    {
        return await ApiService.Instance.Get<List<RoomModel>>($"rooms?name={name}&occuped={occuped.ToString()}");
    }*/

    private static RoomService? _instance;
    public static RoomService Instance => _instance ??= new RoomService();

    private readonly ApiService _api = ApiService.Instance;

    public async Task<ApiResult<List<RoomModel>>> GetAllRooms(string? name, bool? occuped, bool? maintenance, bool? closed)
    {
        var url = "rooms";
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(name))
            query.Add($"name={Uri.EscapeDataString(name)}");

        if (occuped.HasValue)
            query.Add($"occuped={occuped.Value.ToString().ToLower()}");

        if (maintenance.HasValue)
            query.Add($"maintenance={maintenance.Value.ToString().ToLower()}");

        if (closed.HasValue)
            query.Add($"closed={closed.Value.ToString().ToLower()}");

        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        return await ApiService.Instance.Get<List<RoomModel>>(url);
    }

    /// <summary>
    /// Recupera una habitación por identificador.
    /// </summary>
    public async Task<ApiResult<RoomModel>> GetRoom(string id)
    {
        return await ApiService.Instance.Get<RoomModel>($"rooms/{id}");
    }

    /// <summary>
    /// Crea una nueva habitación.
    /// </summary>
    public async Task<ApiResult<RoomModel>> CreateRoom(RoomModel room)
    {
        return await ApiService.Instance.Post<RoomModel>("rooms", room);
    }

    /// <summary>
    /// Actualiza los datos de una habitación existente.
    /// </summary>
    public async Task<ApiResult<RoomModel>> UpdateRoom(RoomModel room)
    {
        return await ApiService.Instance.Put<RoomModel>($"rooms/{room.RoomId}", room);
    }

    /// <summary>
    /// Elimina una habitación del catálogo.
    /// </summary>
    public async Task<ApiResult<RoomModel>> DeleteRoom(string id)
    {
        return await ApiService.Instance.Delete<RoomModel>($"rooms/{id}");
    }

    //Cerrar la Habitacion
    public async Task<ApiResult<RoomModel>> CloseRoom(RoomModel room)
    {
        return await ApiService.Instance.Put<RoomModel>($"rooms/{room.RoomId}/close", room);
    }

    //Stadisticas
    public async Task<ApiResult<List<RoomStatModel>>> RoomStats(string year)
    {
        return await ApiService.Instance.Get<List<RoomStatModel>>($"rooms/stats/{year}");
    }

    //Stadisticas por Habitacion
    public async Task<ApiResult<List<RoomOccupancyModel>>> RoomOccupancy(string id, string year)
    {
        return await ApiService.Instance.Get<List<RoomOccupancyModel>>($"rooms/{id}/occupancy/{year}");
    }

    //Guardar Horas de Limpieza
    public async Task<ApiResult<RoomModel>> EditCleaningTime(string id, int cleaningTime)
    {
        var hours = new{ hours = cleaningTime };
        return await ApiService.Instance.Patch<RoomModel>($"rooms/{id}/cleaningTime", hours);
    }

    //Poner habitacion en mantenimiento
    public async Task<ApiResult<RoomModel>> SetMaintenance(string id, string date, int hour, string reason)
    {
        var maintenanceBody = new
        {
            date = date,           
            hours = hour,
            reason = reason           
        };
        return await ApiService.Instance.Put<RoomModel>($"rooms/{id}/setMaintenance", maintenanceBody);
    }

    //Cancelar mantenimiento

    public async Task<ApiResult<RoomModel>> CancelMaintenance(RoomModel room)
    {
        return await ApiService.Instance.Put<RoomModel>($"rooms/{room.RoomId}/cancelMaintenance", room);
    }

    //Subir Imagen

    public async Task<bool> UploadRoomImage(string roomId, string imagePath)
    {
        try
        {
            if (!File.Exists(imagePath)) return false;

            var client = ApiService.Instance.HttpClient;

            using var form = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(imagePath);
            var fileContent = new StreamContent(fileStream);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(fileContent, "image", Path.GetFileName(imagePath));
            var response = await client.PostAsync($"rooms/{roomId}/image", form);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }


}