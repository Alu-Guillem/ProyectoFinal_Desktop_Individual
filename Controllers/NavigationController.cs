using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public class NavigationController
{
    // Singleton
    private static NavigationController? _instance;
    public static NavigationController Instance => _instance ??= new NavigationController();
    private static readonly NavigationService _navigationService = NavigationService.Instance;

    public static readonly Dictionary<string, RelayCommand> ROUTES = new()
    {
        { "Habitaciones", new RelayCommand(_ => _navigationService.NavigateTo<RoomsView>()) },
        { "Reservas", new RelayCommand(_ => _navigationService.NavigateTo<BookingsView>()) },
        { "Huespedes", new RelayCommand(_ => _navigationService.NavigateTo<CustomersView>()) },
        { "Empleados", new RelayCommand(_ => _navigationService.NavigateTo<EmployeesView>()) },
        { "Reseñas", new RelayCommand(_ => _navigationService.NavigateTo<EmployeesView>()) }
    };
}