using System.Net;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class RoomService
{
    public async Task<ApiResult<List<RoomModel>>> GetAllRooms()
    {
        var rooms = new List<RoomModel>
        {
            new()
            {
                RoomId = "69530df689fb7880d5983c24",
                Name = "Habitacion 1",
                Offer = 20,
                PricePerNight = 70,
                OccupancyLimit = 2,
            }
        };

        return await Task.FromResult(new ApiResult<List<RoomModel>>
        {
            Success = true,
            Data = rooms,
            StatusCode = HttpStatusCode.OK
        });
    }

    public async Task<ApiResult<RoomModel>> GetRoom(string id)
    {
        var room = new RoomModel
        {
            RoomId = "69530df689fb7880d5983c24",
            Name = "Habitacion 1",
            Offer = 20,
            PricePerNight = 70,
            OccupancyLimit = 2,
        };

        return await Task.FromResult(new ApiResult<RoomModel>
        {
            Success = true,
            Data = room,
            StatusCode = HttpStatusCode.OK
        });
    }
}
