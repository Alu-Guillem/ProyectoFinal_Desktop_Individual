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
    
    private void ShowPassword(object sender, RoutedEventArgs e)
    {
        TxtPassword.Text = PwdBox.Password;
        TxtPassword.Visibility = Visibility.Visible;
        PwdBox.Visibility = Visibility.Collapsed;
    }

    private void HidePassword(object sender, RoutedEventArgs e)
    {
        PwdBox.Password = TxtPassword.Text;
        PwdBox.Visibility = Visibility.Visible;
        TxtPassword.Visibility = Visibility.Collapsed;
    }


}