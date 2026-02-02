using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using System.Windows;
using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

namespace PereMaria.GestorHotel.Views;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
        {
            vm.Password = ((PasswordBox)sender).Password;
        }
    }
}