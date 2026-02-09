using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class ReviewsViewModel : BaseViewModel
{
    // Singleton
    private static ReviewsViewModel? _instance;
    public static ReviewsViewModel Instance => _instance ??= new ReviewsViewModel();

    private ReviewsViewModel()
    {
        _currentReview = new ReviewModel();
    }

    // La lista de todas las rooms
    public ObservableCollection<ReviewModel> Reviews { get; } = new();

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
}