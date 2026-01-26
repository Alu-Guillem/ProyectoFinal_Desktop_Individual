using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class BookingsViewModel : BaseViewModel
{
    // Singleton
    private static BookingsViewModel? _instance;
    public static BookingsViewModel Instance => _instance ??= new BookingsViewModel();

    private BookingsViewModel()
    {
        _currentBooking = new BookingModel("Paco");
    }

    // La lista de todos los Bookings
    public ObservableCollection<BookingModel> Bookings { get; } = [new ("Paco"), new ("Marta"), new ("Carlos"), new ("Enrique")];

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
}