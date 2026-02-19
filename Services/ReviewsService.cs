using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

/// <summary>
/// Gestiona la obtencion, filtrado y analisis de resenas desde la API.
/// </summary>
public class ReviewsService
{
    /// <summary>
    /// Parametros para filtrar resultados directamente desde la API.
    /// </summary>
    public sealed class ReviewQuery
    {
        public string? RoomId { get; init; }
        public string? UserId { get; init; }
    }

    /// <summary>
    /// Parametros de filtrado aplicados una vez descargadas las resenas.
    /// </summary>
    public sealed class ReviewFilterOptions
    {
        public string? RoomId { get; init; }
        public string? UserId { get; init; }
        public double? MinRate { get; init; }
        public double? MaxRate { get; init; }
        public string? Search { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
    }

    /// <summary>
    /// Elemento reutilizable para representar opciones de filtro (habitaciones, clientes).
    /// </summary>
    public sealed class FilterItem
    {
        public string Id { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
    }

    /// <summary>
    /// Fuentes disponibles para la interfaz de filtrado.
    /// </summary>
    public sealed class ReviewFilterSources
    {
        public IReadOnlyList<FilterItem> Rooms { get; init; } = Array.Empty<FilterItem>();
        public IReadOnlyList<FilterItem> Customers { get; init; } = Array.Empty<FilterItem>();
    }

    /// <summary>
    /// Contenedor de estadisticas agregadas sobre las resenas.
    /// </summary>
    public sealed class ReviewStatistics
    {
        public int TotalReviews { get; init; }
        public double AverageRating { get; init; }
        public IReadOnlyList<RatingBucket> RatingBuckets { get; init; } = Array.Empty<RatingBucket>();
        public IReadOnlyList<RoomReviewSummary> TopRooms { get; init; } = Array.Empty<RoomReviewSummary>();
    }

    /// <summary>
    /// Numero de resenas por nivel de estrellas.
    /// </summary>
    public sealed class RatingBucket
    {
        public int Stars { get; init; }
        public int Count { get; init; }
        public double Percentage { get; init; }
    }

    /// <summary>
    /// Estadisticas resumidas por habitacion.
    /// </summary>
    public sealed class RoomReviewSummary
    {
        public string RoomId { get; init; } = string.Empty;
        public string RoomName { get; init; } = string.Empty;
        public double AverageRating { get; init; }
        public int ReviewsCount { get; init; }
    }

    private static ReviewsService? _instance;
    public static ReviewsService Instance => _instance ??= new ReviewsService();

    private readonly ApiService _apiService = ApiService.Instance;
    private readonly UserService _userService = UserService.Instance;
    private readonly RoomService _roomService = RoomService.Instance;

    private const string DateFormat = "dd/MM/yyyy";

    /// <summary>
    /// Recupera reseñas desde la API y aplica filtros locales opcionales.
    /// </summary>
    /// <param name="query">Parametros de filtrado enviados al backend.</param>
    /// <param name="filters">Filtros aplicados tras poblar los modelos.</param>
    public async Task<List<ReviewModel>> GetReviews(ReviewQuery? query = null, ReviewFilterOptions? filters = null)
    {
        try
        {
            var endpoint = BuildEndpoint(query);
            var res = await _apiService.Get<ReviewModel[]>(endpoint);

            if (res.Error != null)
            {
                if (res.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<ReviewModel>();
                }

                throw new HttpRequestException(res.Error.Message);
            }

            if (res.Data == null)
            {
                return new List<ReviewModel>();
            }

            var reviews = res.Data.ToList();
            await PopulateReviews(reviews);

            return filters == null ? reviews : ApplyFilters(reviews, filters).ToList();
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

    /// <summary>
    /// Aplica filtros en memoria sobre un conjunto de reseñas.
    /// </summary>
    public IEnumerable<ReviewModel> ApplyFilters(IEnumerable<ReviewModel> source, ReviewFilterOptions filters)
    {
        var filtered = source;

        if (!string.IsNullOrWhiteSpace(filters.RoomId))
        {
            filtered = filtered.Where(r => string.Equals(r.RoomId, filters.RoomId, StringComparison.Ordinal));
        }

        if (!string.IsNullOrWhiteSpace(filters.UserId))
        {
            filtered = filtered.Where(r => string.Equals(r.UserId, filters.UserId, StringComparison.Ordinal));
        }

        if (filters.MinRate.HasValue)
        {
            filtered = filtered.Where(r => r.Rate >= filters.MinRate.Value);
        }

        if (filters.MaxRate.HasValue)
        {
            filtered = filtered.Where(r => r.Rate <= filters.MaxRate.Value);
        }

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var term = filters.Search.Trim().ToLowerInvariant();
            filtered = filtered.Where(r =>
                (r.Comment ?? string.Empty).ToLowerInvariant().Contains(term) ||
                (r.GuestName ?? string.Empty).ToLowerInvariant().Contains(term) ||
                (r.RoomName ?? string.Empty).ToLowerInvariant().Contains(term));
        }

        if (filters.FromDate.HasValue)
        {
            filtered = filtered.Where(r => TryParseCreatedAt(r.CreatedAt, out var created) && created >= filters.FromDate.Value);
        }

        if (filters.ToDate.HasValue)
        {
            filtered = filtered.Where(r => TryParseCreatedAt(r.CreatedAt, out var created) && created <= filters.ToDate.Value);
        }

        return filtered;
    }

    /// <summary>
    /// Genera estadisticas agregadas a partir de un conjunto de reseñas.
    /// </summary>
    public ReviewStatistics BuildStatistics(IEnumerable<ReviewModel> source)
    {
        var reviews = source?.ToList() ?? new List<ReviewModel>();
        var total = reviews.Count;
        var average = total > 0 ? Math.Round(reviews.Average(r => r.Rate), 2) : 0;

        var buckets = new List<RatingBucket>();
        for (var stars = 5; stars >= 1; stars--)
        {
            var count = reviews.Count(r => NormalizeRatingBucket(r.Rate) == stars);
            var percentage = total > 0 ? Math.Round(count * 100d / total, 2) : 0;
            buckets.Add(new RatingBucket
            {
                Stars = stars,
                Count = count,
                Percentage = percentage,
            });
        }

        var topRooms = reviews
            .Where(r => !string.IsNullOrWhiteSpace(r.RoomId))
            .GroupBy(r => r.RoomId)
            .Select(group => new RoomReviewSummary
            {
                RoomId = group.Key,
                RoomName = group.First().Room?.Name ?? group.Key,
                AverageRating = Math.Round(group.Average(r => r.Rate), 2),
                ReviewsCount = group.Count(),
            })
            .OrderByDescending(r => r.AverageRating)
            .ThenByDescending(r => r.ReviewsCount)
            .ThenBy(r => r.RoomName, StringComparer.OrdinalIgnoreCase)
            .Take(3)
            .ToList();

        return new ReviewStatistics
        {
            TotalReviews = total,
            AverageRating = average,
            RatingBuckets = buckets,
            TopRooms = topRooms,
        };
    }

    /// <summary>
    /// Obtiene las fuentes de datos unicas para los filtros de la interfaz.
    /// </summary>
    public ReviewFilterSources BuildFilterSources(IEnumerable<ReviewModel> source)
    {
        var reviews = source?.ToList() ?? new List<ReviewModel>();

        var rooms = reviews
            .Where(r => !string.IsNullOrWhiteSpace(r.RoomId))
            .GroupBy(r => r.RoomId)
            .Select(group => new FilterItem
            {
                Id = group.Key,
                Label = group.First().Room?.Name ?? group.Key,
            })
            .OrderBy(item => item.Label, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var customers = reviews
            .Where(r => !string.IsNullOrWhiteSpace(r.UserId))
            .GroupBy(r => r.UserId)
            .Select(group =>
            {
                var customer = group.First().Customer;
                var name = customer == null
                    ? string.Empty
                    : $"{customer.FirstName} {customer.LastName}".Trim();

                return new FilterItem
                {
                    Id = group.Key,
                    Label = string.IsNullOrWhiteSpace(name) ? group.Key : name,
                };
            })
            .OrderBy(item => item.Label, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new ReviewFilterSources
        {
            Rooms = rooms,
            Customers = customers,
        };
    }

    /// <summary>
    /// Completa las reseñas con datos enriquecidos de usuarios y habitaciones.
    /// </summary>
    private async Task PopulateReviews(List<ReviewModel> reviews)
    {
        if (reviews.Count == 0)
        {
            return;
        }

        var customersResponse = await _userService.GetAllCustomers();
        if (!customersResponse.Success || customersResponse.Data == null)
        {
            var message = customersResponse.Error?.Message ?? "No se pudieron cargar los clientes";
            throw new Exception(message);
        }

        var roomsResponse = await _roomService.GetAllRooms();
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
            {
                review.Customer = customer;
            }

            if (roomsById.TryGetValue(review.RoomId, out var room))
            {
                review.Room = room;
            }
        }
    }

    /// <summary>
    /// Compone el endpoint REST en base a los filtros recibidos.
    /// </summary>
    private static string BuildEndpoint(ReviewQuery? query)
    {
        if (query == null)
        {
            return "reviews";
        }

        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(query.RoomId))
        {
            parts.Add($"roomId={Uri.EscapeDataString(query.RoomId)}");
        }

        if (!string.IsNullOrWhiteSpace(query.UserId))
        {
            parts.Add($"userId={Uri.EscapeDataString(query.UserId)}");
        }

        if (parts.Count == 0)
        {
            return "reviews";
        }

        return $"reviews?{string.Join("&", parts)}";
    }

    private static bool TryParseCreatedAt(string? value, out DateTime date)
    {
        return DateTime.TryParseExact(
            value ?? string.Empty,
            DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date);
    }

    private static int NormalizeRatingBucket(double rate)
    {
        var rounded = (int)Math.Round(rate, MidpointRounding.AwayFromZero);
        if (rounded < 1) return 1;
        if (rounded > 5) return 5;
        return rounded;
    }
}
