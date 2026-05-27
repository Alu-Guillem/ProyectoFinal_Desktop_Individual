using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

/// <summary>
/// ViewModel de navegación principal: expone menú lateral, vista activa y acciones globales de sesión.
/// </summary>
public class NavigationViewModel : BaseViewModel
{
    // Singleton
    private static NavigationViewModel? _instance;
    public static NavigationViewModel Instance => _instance ??= new NavigationViewModel();

    private readonly NavigationService _navigationService = NavigationService.Instance;

    private readonly SessionService _session = SessionService.Instance;
    private readonly AuthService _authService = AuthService.Instance;


    /// <summary>
    /// Devuelve y actualiza el ítem de navegación seleccionado en función de la vista actual.
    /// </summary>
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

    /// <summary>
    /// Recalcula los datos de cabecera cuando cambia la sesión activa.
    /// </summary>
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

    /// <summary>
    /// Vuelve a la vista anterior de la pila y actualiza bindings de navegación.
    /// </summary>
    public void NavigateBack()
    {
        _navigationService.NavigateBack();
        OnPropertyChanged(nameof(CurrentView));
        OnPropertyChanged(nameof(SelectedNavItem));
    }

    /// <summary>
    /// Navega a una vista concreta y refresca el estado visual del menú.
    /// </summary>
    /// <typeparam name="T">Tipo de vista destino.</typeparam>
    public void NavigateTo<T>() where T : UserControl, new()
    {
        _navigationService.NavigateTo<T>();
        OnPropertyChanged(nameof(CurrentView));
        OnPropertyChanged(nameof(SelectedNavItem));
    }

    /// <summary>
    /// Comando para regresar a la vista anterior.
    /// </summary>
    public RelayCommand BackCommand => new(_ => NavigateBack());

    /// <summary>
    /// Comando para cerrar sesión y volver a la pantalla de login.
    /// </summary>
    public RelayCommand LogoutCommand => new(_ => Logout());

    /// <summary>
    /// Cierra la sesión actual, reinicia estado de navegación y muestra la ventana de autenticación.
    /// </summary>
    public void Logout()
    {
        SessionService.Instance.SetToken(null);
        SessionService.Instance.CurrentUser = null;

        LoginViewModel.Instance.Email = string.Empty;

        Application.Current.Dispatcher.Invoke(() =>
            {
                var login = new Login();
                login.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        NavigateTo<RoomsView>();
                        window.Close();
                        break;
                    }
                }
            }
        );



    }


}