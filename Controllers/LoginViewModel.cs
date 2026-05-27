using System.Windows;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;


namespace PereMaria.GestorHotel.Controllers;

/// <summary>
/// ViewModel de autenticación para la ventana de acceso al panel de escritorio.
/// Gestiona credenciales, login contra API y transición a ventana principal.
/// </summary>
public class LoginViewModel : BaseViewModel
{
    private static LoginViewModel? _instance;
    public static LoginViewModel Instance => _instance ??= new LoginViewModel();

    private readonly AuthService _authService = AuthService.Instance;

    private LoginViewModel()
    {
    }

    private string _email;

    /// <summary>
    /// Correo usado para autenticarse en backend.
    /// </summary>
    public String Email
    {
        get => _email;
        set
        {
            if (value == _email) return;
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    private string _password;

    /// <summary>
    /// Contraseña usada para autenticarse en backend.
    /// </summary>
    public String Password
    {
        get => _password;
        set
        {
            if (value == _password) return;
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    /// <summary>
    /// Ejecuta el flujo de autenticación y redirige a la aplicación principal al completarse.
    /// </summary>
    public RelayCommand LoginCommand => new RelayCommand(async _ => await Login());

    /// <summary>
    /// Valida credenciales contra la API y bloquea el acceso para usuarios con rol customer.
    /// </summary>
    private async Task Login()
    {
        Console.WriteLine("Logeando");

        var result = await _authService.Login(Email, Password);

        if (result.Success)
        {
            SessionService.Instance.SetToken(result.Data.Token);
            await SessionService.Instance.LoadUserInfo();

            if (SessionService.Instance.CurrentUser?.Role == "customer")
            {
                ShowMessageBox("Un customer no se puede loguear", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ChangeWindow();
        }
        else
        {
            ShowMessageBox(result.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Cierra la ventana de login y abre la ventana principal de gestión.
    /// </summary>
    private void ChangeWindow()
    {
        Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is Login)
                    {
                        window.Close();
                        break;
                    }
                }
            }
        );
    }
}