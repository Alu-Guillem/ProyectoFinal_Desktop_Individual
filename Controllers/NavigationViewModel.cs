using System.ComponentModel;
using System.Runtime.CompilerServices;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public class NavigationViewModel : BaseViewModel
{
    // Singleton
    private static NavigationViewModel? _instance;
    public static NavigationViewModel Instance => _instance ??= new NavigationViewModel();

    private readonly NavigationService _navigationService = NavigationService.Instance;

    public IReadOnlyList<NavItem> MenuItems { get; }

    private NavItem? _currentItem;

    public NavItem? CurrentItem
    {
        get => _currentItem;
        set
        {
            if (_currentItem == value) return;

            _currentItem = value;
            OnPropertyChanged(nameof(CurrentItem));
            _currentItem?.Navigate();
        }
    }

    private NavigationViewModel()
    {
        MenuItems =
        [
            new NavItem
            {
                Label = "🏠 Habitaciones", Command = new RelayCommand(_ => CurrentItem = MenuItems?[0]),
                Navigate = () => _navigationService.NavigateTo<RoomsView>()
            },
            new NavItem
            {
                Label = "📅 Reservas", Command = new RelayCommand(_ => CurrentItem = MenuItems?[1]),
                Navigate = () => _navigationService.NavigateTo<BookingsView>()
            },
            new NavItem
            {
                Label = "👽 Huéspedes", Command = new RelayCommand(_ => CurrentItem = MenuItems?[2]),
                Navigate = () => _navigationService.NavigateTo<CustomersView>()
            },
            new NavItem
            {
                Label = "👤 Empleados", Command = new RelayCommand(_ => CurrentItem = MenuItems?[3]),
                Navigate = () => _navigationService.NavigateTo<EmployeesView>()
            },
            new NavItem
            {
                Label = "⭐ Reseñas", Command = new RelayCommand(_ => CurrentItem = MenuItems?[4]),
                Navigate = () => _navigationService.NavigateTo<ReviewsView>()
            } // TODO: ReviewsView
        ];
    }
}