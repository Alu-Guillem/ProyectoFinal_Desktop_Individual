using System.Windows;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;


namespace PereMaria.GestorHotel.Controllers;

public class LoginViewModel : BaseViewModel
{
    private static LoginViewModel? _instance;
    public static LoginViewModel Instance => _instance ??= new LoginViewModel();

    private readonly AuthService _authService = new AuthService();

    private LoginViewModel()
    {
    }

    private string _email;

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

    public RelayCommand LoginCommand => new RelayCommand(async _ => await Login());

    private async Task Login()
    {
        Console.WriteLine("Logeando");

        var result = await _authService.Login(Email, Password);


        if (result.Success)
        {
            SessionService.Instance.SetToken(result.Data.Token);

            await SessionService.Instance.LoadUserInfo();


            ChangeWindow();
        }
        else
        {
            Console.WriteLine(result.Error.Message);
        }
    }

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