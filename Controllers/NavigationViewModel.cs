using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
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

    private readonly SessionService _session = SessionService.Instance;


    // Devuelve el NavItem correspondiente a la vista actual
    public NavItem? SelectedNavItem
    {
        get
        {
            var currentType = CurrentView.GetType();

            return MenuItems.FirstOrDefault(item => currentType.Name.Contains(item.ViewName));
        }
        set
        {
            if (value == null) return;
            value.Command.Execute(null);
            OnPropertyChanged(nameof(SelectedNavItem));
            OnPropertyChanged(nameof(CurrentView));
        }
    }


    public IReadOnlyList<NavItem> MenuItems { get; }

    public UserControl CurrentView => _navigationService.CurrentView;

    private NavigationViewModel()
    {
        _session.SessionChanged += OnSessionChanged;

        MenuItems =
        [
            new NavItem
            {
                Label = "🏠 Habitaciones",
                Command = new RelayCommand(_ => NavigateTo<RoomsView>()),
                ViewName = typeof(RoomsView).Name.Replace("View", ""),
            },
            new NavItem
            {
                Label = "📅 Reservas",
                Command = new RelayCommand(_ => NavigateTo<BookingsView>()),
                ViewName = typeof(BookingsView).Name.Replace("View", ""),
            },
            new NavItem
            {
                Label = "👽 Huéspedes",
                Command = new RelayCommand(_ => NavigateTo<CustomersView>()),
                ViewName = typeof(CustomersView).Name.Replace("View", ""),
            },
            new NavItem
            {
                Label = "👤 Empleados",
                Command = new RelayCommand(_ => NavigateTo<EmployeesView>()),
                ViewName = typeof(EmployeesView).Name.Replace("View", ""),
            },
            new NavItem
            {
                Label = "⭐ Reseñas",
                Command = new RelayCommand(_ => NavigateTo<ReviewsView>()),
                ViewName = typeof(ReviewsView).Name.Replace("View", ""),
            }
        ];
    }

    private void OnSessionChanged()
    {
        OnPropertyChanged(nameof(FirstName));
        OnPropertyChanged(nameof(Initial));
        OnPropertyChanged(nameof(Role));
    }

    public string FirstName => _session.CurrentUser?.FirstName ?? "Invitado";

    public string Initial =>
        string.IsNullOrEmpty(_session.CurrentUser?.FirstName)
            ? "?"
            : _session.CurrentUser.FirstName[0].ToString();

    public string Role => _session.CurrentUser?.Role ?? "";

    public void NavigateBack()
    {
        _navigationService.NavigateBack();
        OnPropertyChanged(nameof(CurrentView));
        OnPropertyChanged(nameof(SelectedNavItem));
    }

    public void NavigateTo<T>() where T : UserControl, new()
    {
        _navigationService.NavigateTo<T>();
        OnPropertyChanged(nameof(CurrentView));
        OnPropertyChanged(nameof(SelectedNavItem));
    }

    public RelayCommand BackCommand => new(_ => NavigateBack());
}