using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class BookingsViewModel : INotifyPropertyChanged
{
    // Singleton
    private static BookingsViewModel? _instance;
    public static BookingsViewModel Instance => _instance ??= new BookingsViewModel();

    private BookingsViewModel()
    {
        _currentBooking = new BookingModel();
    }

    // La lista de todos los Bookings
    public ObservableCollection<BookingModel> Bookings { get; } = new();

    // El booking que se está editando/creando actualmente
    private BookingModel _currentBooking;
    public BookingModel CurrentBooking
    {
        get => _currentBooking;
        set
        {
            if (value == _currentBooking) return;
            _currentBooking = value;
            OnPropertyChanged(nameof(CurrentBooking));
        }
    }

    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
