using System.ComponentModel;
using System.Runtime.CompilerServices;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public enum ViewType
{
    Habitaciones,
    Reservas,
    Huespedes,
    Empleados,
    Reseñas
}

public class NavigationController : INotifyPropertyChanged
{
    // Singleton
    private static NavigationController? _instance;
    public static NavigationController Instance => _instance ??= new NavigationController();

    private readonly NavigationService _navigationService = NavigationService.Instance;

    // ÚNICA FUENTE DE VERDAD
    private readonly Dictionary<ViewType, Action> _routes;

    public IReadOnlyDictionary<ViewType, RelayCommand> ROUTES { get; }

    private ViewType _currentView;
    public ViewType CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView == value) return;

            _currentView = value;
            OnPropertyChanged();
            Navigate(value);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private NavigationController()
    {
        _routes = new Dictionary<ViewType, Action>
        {
            { ViewType.Habitaciones, () => _navigationService.NavigateTo<RoomsView>() },
            { ViewType.Reservas,     () => _navigationService.NavigateTo<BookingsView>() },
            { ViewType.Huespedes,    () => _navigationService.NavigateTo<CustomersView>() },
            { ViewType.Empleados,    () => _navigationService.NavigateTo<EmployeesView>() },
            { ViewType.Reseñas,      () => _navigationService.NavigateTo<EmployeesView>() }
        };

        // Generación automática de comandos
        ROUTES = _routes.ToDictionary(
            r => r.Key,
            r => new RelayCommand(_ => CurrentView = r.Key)
        );
    }

    private void Navigate(ViewType view)
    {
        if (_routes.TryGetValue(view, out var navigate))
        {
            navigate();
        }
    }
}