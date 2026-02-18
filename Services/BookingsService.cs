using System.Net.Http;
using System.Linq;
using System.Windows;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class BookingsService
{
    // Singleton
    private static BookingsService? _instance;
    public static BookingsService Instance => _instance ??= new BookingsService();

    private readonly ApiService _apiService = ApiService.Instance;

    public async Task<List<BookingModel>?> GetBookings()
    {
        try
        {
            var res = await _apiService.Get<BookingModel[]>("bookings");

            if (res.Error != null)
                throw new HttpRequestException(res.Error.Message);


            if (res.Data == null) return null;

            var bookings = res.Data.ToList();

            await PopulateBookings(bookings);

            return bookings;
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

    public async Task<BookingModel?> GetBooking(string id)
    {
        try
        {
            var res = await _apiService.Get<BookingModel>($"bookings/{id}");

            if (res.Error != null)
                throw new Exception(res.Error.Message);

            if (res.Data == null)
                return null;

            await PopulateBooking(res.Data);

            return res.Data;
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

    public async Task<BookingModel?> CreateBooking(BookingModel booking)
    {
        Console.Write(booking);

        try
        {
            var res = await _apiService.Post<BookingModel>("bookings", booking);

            return res.Error != null ? throw new HttpRequestException(res.Error.Message) : res.Data;
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

    public async Task<BookingModel?> UpdateBooking(string id, BookingModel booking)
    {
        try
        {
            var res = await _apiService.Put<BookingModel>($"bookings/{id}", booking);

            return res.Error != null ? throw new HttpRequestException(res.Error.Message) : res.Data;
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

    public async Task<bool> CancelBooking(string id)
    {
        try
        {
            var res = await _apiService.Put<object>($"bookings/{id}/cancel", new object());

            if (res.Error != null)
                throw new Exception(res.Error.Message);

            return true;
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

    public async Task<BookingModel?> ExtendBooking(string id, string endDate)
    {
        try
        {
            var res = await _apiService.Put<BookingModel>($"bookings/{id}/extend", new { endDate });

            return res.Error != null ? throw new Exception(res.Error.Message) : res.Data;
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

    public async Task<BookingModel?> PayBooking(string id)
    {
        try
        {
            var res = await _apiService.Put<BookingModel>($"bookings/{id}/pay", new object());

            return res.Error != null ? throw new Exception(res.Error.Message) : res.Data;
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

    public async Task<bool> DeleteBooking(string id)
    {
        try
        {
            var res = await _apiService.Delete<object>($"bookings/{id}");

            if (res.Error != null)
                throw new Exception(res.Error.Message);

            return true;
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

    private async Task PopulateBookings(List<BookingModel> bookings)
    {
        if (bookings.Count == 0) return;

        var customersResponse = await new UserService().GetAllCustomers();
        if (!customersResponse.Success || customersResponse.Data == null)
        {
            var message = customersResponse.Error?.Message ?? "No se pudieron cargar los clientes";
            throw new Exception(message);
        }

        var roomsResponse = await new RoomService().GetAllRooms("",null);
        if (!roomsResponse.Success || roomsResponse.Data == null)
        {
            var message = roomsResponse.Error?.Message ?? "No se pudieron cargar las habitaciones";
            throw new Exception(message);
        }

        var customersById = customersResponse.Data.ToDictionary(c => c.UserId);
        var roomsById = roomsResponse.Data.ToDictionary(r => r.RoomId);

        foreach (var booking in bookings)
        {
            if (customersById.TryGetValue(booking.UserId, out var customer))
                booking.Customer = customer;

            if (roomsById.TryGetValue(booking.RoomId, out var room))
                booking.Room = room;
        }
    }

    private async Task PopulateBooking(BookingModel booking)
    {
        await PopulateBookings([booking]);
    }
}