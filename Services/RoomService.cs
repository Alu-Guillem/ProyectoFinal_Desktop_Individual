using PereMaria.GestorHotel.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;

namespace PereMaria.GestorHotel.Services;

public class RoomService
{
    /*public async Task<ApiResult<List<RoomModel>>> GetAllRooms(string name, bool? occuped)
    {
        return await ApiService.Instance.Get<List<RoomModel>>($"rooms?name={name}&occuped={occuped.ToString()}");
    }*/

    public async Task<ApiResult<List<RoomModel>>> GetAllRooms(string? name, bool? occuped)
    {
        var url = "rooms";
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(name))
            query.Add($"name={Uri.EscapeDataString(name)}");

        if (occuped.HasValue)
            query.Add($"occuped={occuped.Value.ToString().ToLower()}");

        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        return await ApiService.Instance.Get<List<RoomModel>>(url);
    }

    public async Task<ApiResult<RoomModel>> GetRoom(string id)
    {
        return await ApiService.Instance.Get<RoomModel>($"rooms/{id}");
    }

    public async Task<ApiResult<RoomModel>> CreateRoom(RoomModel room)
    {
        return await ApiService.Instance.Post<RoomModel>("rooms", room);
    }

    public async Task<ApiResult<RoomModel>> UpdateRoom(RoomModel room)
    {
        return await ApiService.Instance.Put<RoomModel>($"rooms/{room.RoomId}", room);
    }

    public async Task<ApiResult<RoomModel>> DeleteRoom(string id)
    {
        return await ApiService.Instance.Delete<RoomModel>($"rooms/{id}");
    }
}