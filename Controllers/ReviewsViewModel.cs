using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;

namespace PereMaria.GestorHotel.Controllers;

public class ReviewsViewModel : BaseViewModel
{
    // Singleton
    private static ReviewsViewModel? _instance;
    public static ReviewsViewModel Instance => _instance ??= new ReviewsViewModel();

    private readonly ReviewsService _reviewsService = ReviewsService.Instance;

    private ReviewsViewModel()
    {
        _currentReview = new ReviewModel();
        _ = LoadReviews();
    }

    public ObservableCollection<ReviewModel> Reviews { get; } = new();

    public ObservableCollection<RatingBucket> RatingBreakdown { get; } = new();

    public ObservableCollection<RoomReviewSummary> TopRooms { get; } = new();

    // La room que se está editando/creando actualmente
    private ReviewModel _currentReview;

    public ReviewModel CurrentReview
    {
        get => _currentReview;
        set
        {
            if (value == _currentReview) return;
            _currentReview = value;
            OnPropertyChanged(nameof(CurrentReview));
        }
    }

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (value == _isLoading) return;
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(IsNotLoading));
        }
    }

    public bool IsNotLoading => !IsLoading;

    private int _totalReviews;

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

    private double _averageRating;

    public double AverageRating
    {
        get => _averageRating;
        private set
        {
            if (Math.Abs(value - _averageRating) < 0.01) return;
            _averageRating = value;
            OnPropertyChanged(nameof(AverageRating));
        }
    }

    public bool HasReviews => TotalReviews > 0;

    public bool NoReviews => TotalReviews == 0;

    public string TotalReviewsLabel =>
        TotalReviews == 1 ? "Basado en 1 reseña" : $"Basado en {TotalReviews} reseñas";

    private async Task LoadReviews()
    {
        try
        {
            IsLoading = true;
            Reviews.Clear();

            var reviews = await _reviewsService.GetReviews();
            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }

            UpdateSummary();
        }
        catch (Exception e)
        {
            ShowMessageBox(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void UpdateSummary()
    {
        TotalReviews = Reviews.Count;
        AverageRating = TotalReviews == 0 ? 0 : Reviews.Average(r => r.Rate);

        RatingBreakdown.Clear();
        for (var stars = 5; stars >= 1; stars--)
        {
            var count = Reviews.Count(r => r.Rate == stars);
            var percentage = TotalReviews == 0 ? 0 : (double)count / TotalReviews * 100;
            RatingBreakdown.Add(new RatingBucket
            {
                Stars = stars,
                Count = count,
                Percentage = percentage,
            });
        }

        TopRooms.Clear();
        var topRooms = Reviews
            .GroupBy(r => r.RoomId)
            .Select(group => new RoomReviewSummary
            {
                RoomName = group.First().Room?.Name ?? group.Key,
                AverageRating = group.Average(r => r.Rate),
                ReviewsCount = group.Count(),
            })
            .OrderByDescending(r => r.AverageRating)
            .ThenByDescending(r => r.ReviewsCount)
            .Take(3);

        foreach (var room in topRooms)
        {
            TopRooms.Add(room);
        }

        OnPropertyChanged(nameof(TotalReviewsLabel));
    }

    public class RatingBucket
    {
        public int Stars { get; init; }
        public int Count { get; init; }
        public double Percentage { get; init; }
    }

    public class RoomReviewSummary
    {
        public string RoomName { get; init; } = "";
        public double AverageRating { get; init; }
        public int ReviewsCount { get; init; }
    }
}