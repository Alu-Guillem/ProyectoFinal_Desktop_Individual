using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;

namespace PereMaria.GestorHotel.Controllers;

/// <summary>
/// ViewModel encargado de la gestion y analisis de resenas.
/// </summary>
public class ReviewsViewModel : BaseViewModel
{
    private static ReviewsViewModel? _instance;
    public static ReviewsViewModel Instance => _instance ??= new ReviewsViewModel();

    private const string DateFormat = "dd/MM/yyyy";

    private readonly ReviewsService _reviewsService = ReviewsService.Instance;
    private readonly List<ReviewModel> _allReviews = new();

    private bool _suppressFilterApply;
    private ReviewModel _currentReview = new();
    private bool _isLoading;
    private int _totalReviews;
    private double _averageRating;
    private string _filterError = string.Empty;
    private ReviewsService.FilterItem? _selectedRoomFilter;
    private ReviewsService.FilterItem? _selectedCustomerFilter;
    private string _filterSearchText = string.Empty;
    private string _filterMinRateText = string.Empty;
    private string _filterMaxRateText = string.Empty;
    private string _filterFromDateText = string.Empty;
    private string _filterToDateText = string.Empty;

    private RelayCommand? _refreshCommand;
    private RelayCommand? _clearFiltersCommand;
    private RelayCommand? _applyFiltersCommand;

    private ReviewsViewModel()
    {
    }

    public ObservableCollection<ReviewModel> Reviews { get; } = new();
    public ObservableCollection<ReviewsService.RatingBucket> RatingBreakdown { get; } = new();
    public ObservableCollection<ReviewsService.RoomReviewSummary> TopRooms { get; } = new();
    public ObservableCollection<ReviewsService.FilterItem> RoomFilterOptions { get; } = new();
    public ObservableCollection<ReviewsService.FilterItem> CustomerFilterOptions { get; } = new();

    public ReviewModel CurrentReview
    {
        get => _currentReview;
        private set
        {
            if (value == _currentReview) return;
            _currentReview = value;
            OnPropertyChanged(nameof(CurrentReview));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (value == _isLoading) return;
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(IsNotLoading));
        }
    }

    public bool IsNotLoading => !IsLoading;

    public int TotalReviews
    {
        get => _totalReviews;
        private set
        {
            if (value == _totalReviews) return;
            _totalReviews = value;
            OnPropertyChanged(nameof(TotalReviews));
            OnPropertyChanged(nameof(HasReviews));
            OnPropertyChanged(nameof(NoReviews));
        }
    }

    public double AverageRating
    {
        get => _averageRating;
        private set
        {
            if (Math.Abs(value - _averageRating) < 0.001) return;
            _averageRating = value;
            OnPropertyChanged(nameof(AverageRating));
        }
    }

    public bool HasReviews => TotalReviews > 0;
    public bool NoReviews => TotalReviews == 0;
    public string TotalReviewsLabel => TotalReviews == 1 ? "Basado en 1 resena" : $"Basado en {TotalReviews} resenas";

    public string FilterError
    {
        get => _filterError;
        private set
        {
            if (string.Equals(value, _filterError, StringComparison.Ordinal)) return;
            _filterError = value;
            OnPropertyChanged(nameof(FilterError));
            OnPropertyChanged(nameof(HasFilterError));
        }
    }

    public bool HasFilterError => !string.IsNullOrWhiteSpace(FilterError);

    public ReviewsService.FilterItem? SelectedRoomFilter
    {
        get => _selectedRoomFilter;
        set
        {
            if (Equals(value, _selectedRoomFilter)) return;
            _selectedRoomFilter = value;
            OnPropertyChanged(nameof(SelectedRoomFilter));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public ReviewsService.FilterItem? SelectedCustomerFilter
    {
        get => _selectedCustomerFilter;
        set
        {
            if (Equals(value, _selectedCustomerFilter)) return;
            _selectedCustomerFilter = value;
            OnPropertyChanged(nameof(SelectedCustomerFilter));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public string FilterSearchText
    {
        get => _filterSearchText;
        set
        {
            var normalized = value ?? string.Empty;
            if (string.Equals(normalized, _filterSearchText, StringComparison.Ordinal)) return;
            _filterSearchText = normalized;
            OnPropertyChanged(nameof(FilterSearchText));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public string FilterMinRateText
    {
        get => _filterMinRateText;
        set
        {
            var normalized = value ?? string.Empty;
            if (string.Equals(normalized, _filterMinRateText, StringComparison.Ordinal)) return;
            _filterMinRateText = normalized;
            OnPropertyChanged(nameof(FilterMinRateText));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public string FilterMaxRateText
    {
        get => _filterMaxRateText;
        set
        {
            var normalized = value ?? string.Empty;
            if (string.Equals(normalized, _filterMaxRateText, StringComparison.Ordinal)) return;
            _filterMaxRateText = normalized;
            OnPropertyChanged(nameof(FilterMaxRateText));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public string FilterFromDateText
    {
        get => _filterFromDateText;
        set
        {
            var normalized = value ?? string.Empty;
            if (string.Equals(normalized, _filterFromDateText, StringComparison.Ordinal)) return;
            _filterFromDateText = normalized;
            OnPropertyChanged(nameof(FilterFromDateText));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public string FilterToDateText
    {
        get => _filterToDateText;
        set
        {
            var normalized = value ?? string.Empty;
            if (string.Equals(normalized, _filterToDateText, StringComparison.Ordinal)) return;
            _filterToDateText = normalized;
            OnPropertyChanged(nameof(FilterToDateText));
            if (!_suppressFilterApply) ApplyFiltersInternal();
        }
    }

    public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(async _ => await LoadReviews());
    public RelayCommand ClearFiltersCommand => _clearFiltersCommand ??= new RelayCommand(_ => ResetFilters());
    public RelayCommand ApplyFiltersCommand => _applyFiltersCommand ??= new RelayCommand(_ => ApplyFiltersInternal());

    /// <summary>
    /// Carga reseñas desde API, reinicia el estado local de filtros y refresca métricas.
    /// </summary>
    public async Task LoadReviews()
    {
        try
        {
            IsLoading = true;

            var reviews = await _reviewsService.GetReviews();
            Console.WriteLine(Reviews.Count);
            _allReviews.Clear();
            _allReviews.AddRange(reviews);

            UpdateFilterSources();
            ResetFilters(skipApply: true);
            ApplyFiltersInternal();
        }
        catch (Exception ex)
        {
            ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Reconstruye catálogos de filtros (habitaciones/clientes) según el dataset cargado.
    /// </summary>
    private void UpdateFilterSources()
    {
        var sources = _reviewsService.BuildFilterSources(_allReviews);

        _suppressFilterApply = true;

        RoomFilterOptions.Clear();
        RoomFilterOptions.Add(new ReviewsService.FilterItem { Id = string.Empty, Label = "Todas las habitaciones" });
        foreach (var room in sources.Rooms)
        {
            RoomFilterOptions.Add(room);
        }

        CustomerFilterOptions.Clear();
        CustomerFilterOptions.Add(new ReviewsService.FilterItem { Id = string.Empty, Label = "Todos los clientes" });
        foreach (var customer in sources.Customers)
        {
            CustomerFilterOptions.Add(customer);
        }

        SelectedRoomFilter = RoomFilterOptions.FirstOrDefault();
        SelectedCustomerFilter = CustomerFilterOptions.FirstOrDefault();

        _suppressFilterApply = false;
    }

    /// <summary>
    /// Restablece todos los filtros de búsqueda y rango a su estado inicial.
    /// </summary>
    private void ResetFilters(bool skipApply = false)
    {
        _suppressFilterApply = true;

        SelectedRoomFilter = RoomFilterOptions.FirstOrDefault();
        SelectedCustomerFilter = CustomerFilterOptions.FirstOrDefault();
        FilterSearchText = string.Empty;
        FilterMinRateText = string.Empty;
        FilterMaxRateText = string.Empty;
        FilterFromDateText = string.Empty;
        FilterToDateText = string.Empty;
        FilterError = string.Empty;

        _suppressFilterApply = false;

        if (!skipApply)
        {
            ApplyFiltersInternal();
        }
    }

    /// <summary>
    /// Aplica filtros en memoria, ordena resultados y actualiza resumen estadístico.
    /// </summary>
    private void ApplyFiltersInternal()
    {
        if (_suppressFilterApply || IsLoading)
        {
            return;
        }

        if (!TryBuildFilterOptions(out var filterOptions))
        {
            return;
        }

        var filtered = _reviewsService
            .ApplyFilters(_allReviews, filterOptions)
            .OrderByDescending(GetCreatedAtOrMin)
            .ThenByDescending(r => r.Rate)
            .ToList();

        ReplaceReviews(filtered);
        UpdateSummary(filtered);
    }

    /// <summary>
    /// Valida y compone el objeto de filtros que consumirá el servicio de reseñas.
    /// </summary>
    private bool TryBuildFilterOptions(out ReviewsService.ReviewFilterOptions options)
    {
        options = new ReviewsService.ReviewFilterOptions();

        if (!TryParseRate(FilterMinRateText, out var minRate, out var rateError))
        {
            FilterError = rateError ?? "Calificacion minima invalida";
            return false;
        }

        if (!TryParseRate(FilterMaxRateText, out var maxRate, out rateError))
        {
            FilterError = rateError ?? "Calificacion maxima invalida";
            return false;
        }

        if (minRate.HasValue && maxRate.HasValue && minRate > maxRate)
        {
            FilterError = "La calificacion minima no puede ser mayor que la maxima";
            return false;
        }

        if (!TryParseDate(FilterFromDateText, out var fromDate, out var dateError))
        {
            FilterError = dateError ?? "Fecha inicial invalida";
            return false;
        }

        if (!TryParseDate(FilterToDateText, out var toDate, out dateError))
        {
            FilterError = dateError ?? "Fecha final invalida";
            return false;
        }

        if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
        {
            FilterError = "La fecha inicial no puede ser posterior a la final";
            return false;
        }

        FilterError = string.Empty;

        options = new ReviewsService.ReviewFilterOptions
        {
            RoomId = string.IsNullOrWhiteSpace(SelectedRoomFilter?.Id) ? null : SelectedRoomFilter.Id,
            UserId = string.IsNullOrWhiteSpace(SelectedCustomerFilter?.Id) ? null : SelectedCustomerFilter.Id,
            MinRate = minRate,
            MaxRate = maxRate,
            Search = string.IsNullOrWhiteSpace(FilterSearchText) ? null : FilterSearchText.Trim(),
            FromDate = fromDate,
            ToDate = toDate,
        };

        return true;
    }

    /// <summary>
    /// Parsea y valida una calificación textual en rango permitido [0.5, 5.0].
    /// </summary>
    private static bool TryParseRate(string input, out double? rate, out string? error)
    {
        rate = null;
        error = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            return true;
        }

        if (!double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
        {
            error = "La calificacion debe ser numerica";
            return false;
        }

        if (parsed < 0.5 || parsed > 5)
        {
            error = "La calificacion debe estar entre 0.5 y 5";
            return false;
        }

        rate = Math.Round(parsed, 2);
        return true;
    }

    /// <summary>
    /// Parsea fechas de filtro en formato DD/MM/YYYY y reporta error de formato.
    /// </summary>
    private static bool TryParseDate(string input, out DateTime? date, out string? error)
    {
        date = null;
        error = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            return true;
        }

        var trimmed = input.Trim();
        if (trimmed.Length < DateFormat.Length)
        {
            return true;
        }

        if (!DateTime.TryParseExact(trimmed, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            error = "La fecha debe tener formato DD/MM/YYYY";
            return false;
        }

        date = parsed;
        return true;
    }

    /// <summary>
    /// Obtiene fecha de creación de la reseña o DateTime.MinValue si no es válida.
    /// </summary>
    private static DateTime GetCreatedAtOrMin(ReviewModel review)
    {
        return DateTime.TryParseExact(
            review.CreatedAt ?? string.Empty,
            DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsed)
            ? parsed
            : DateTime.MinValue;
    }

    /// <summary>
    /// Reemplaza el contenido observable de la tabla de reseñas.
    /// </summary>
    private void ReplaceReviews(IEnumerable<ReviewModel> source)
    {
        Reviews.Clear();
        foreach (var review in source)
        {
            Reviews.Add(review);
        }
    }

    /// <summary>
    /// Actualiza KPIs, distribución de estrellas y top de habitaciones.
    /// </summary>
    private void UpdateSummary(IReadOnlyList<ReviewModel> source)
    {
        var stats = _reviewsService.BuildStatistics(source);

        TotalReviews = stats.TotalReviews;
        AverageRating = stats.AverageRating;

        RatingBreakdown.Clear();
        foreach (var bucket in stats.RatingBuckets)
        {
            RatingBreakdown.Add(bucket);
        }

        TopRooms.Clear();
        foreach (var room in stats.TopRooms)
        {
            TopRooms.Add(room);
        }

        OnPropertyChanged(nameof(TotalReviewsLabel));
    }
}
