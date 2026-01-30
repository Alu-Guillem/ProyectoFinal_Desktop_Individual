using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using System.Windows;

namespace PereMaria.GestorHotel.Views;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string email = TxtBoxEmailBox.Text;
        string password = TxtBoxPasswordBox.Password; 

        Console.WriteLine($"Email: {email}, Password: {password}");
        CheckLogin(email, password);
    }

    private async Task<bool> CheckLogin(String email, String password)
    {

        try 
        {
            var resultado = await ApiService.Instance.Post<UserModel>("auth/login", new UserModel(email, password));
            

            if (resultado.Success)
            {
                Console.WriteLine("Login exitoso");
                Console.WriteLine($"Resultado: {resultado}");

                return true; 
            }
            else
            {
                MessageBox.Show(resultado.Error.Message);
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }
}