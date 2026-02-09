using System.Net;
using System.Net.Http;
using System.Linq;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class ReviewsService
{
    private static ReviewsService? _instance;
    public static ReviewsService Instance => _instance ??= new ReviewsService();

    private readonly ApiService _apiService = ApiService.Instance;

    public async Task<List<ReviewModel>> GetReviews()
    {
        try
        {
            var res = await _apiService.Get<ReviewModel[]>("reviews");

            if (res.Error != null)
            {
                if (res.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<ReviewModel>();
                }

                throw new HttpRequestException(res.Error.Message);
            }

            if (res.Data == null) return new List<ReviewModel>();

            var reviews = res.Data.ToList();
            await PopulateReviews(reviews);

            return reviews;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("Error al conectar con el servidor");
        }
    }

    private async Task PopulateReviews(List<ReviewModel> reviews)
    {
        if (reviews.Count == 0) return;

        var customersResponse = await new UserService().GetAllCustomers();
        if (!customersResponse.Success || customersResponse.Data == null)
        {
            var message = customersResponse.Error?.Message ?? "No se pudieron cargar los clientes";
            throw new Exception(message);
        }

        var roomsResponse = await new RoomService().GetAllRooms();
        if (!roomsResponse.Success || roomsResponse.Data == null)
        {
            var message = roomsResponse.Error?.Message ?? "No se pudieron cargar las habitaciones";
            throw new Exception(message);
        }

        var customersById = customersResponse.Data.ToDictionary(c => c.UserId);
        var roomsById = roomsResponse.Data.ToDictionary(r => r.RoomId);

        foreach (var review in reviews)
        {
            if (customersById.TryGetValue(review.UserId, out var customer))
                review.Customer = customer;

            if (roomsById.TryGetValue(review.RoomId, out var room))
                review.Room = room;
        }
    }
}
