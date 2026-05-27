using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System.Windows;
using System.Windows.Controls;

namespace PereMaria.GestorHotel.Views;

public partial class EmployeesFormView : UserControl
{
    public EmployeesFormView()
    {
        InitializeComponent();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is UserFromViewModel vm)
        {
            vm.Password = ((PasswordBox)sender).Password;
        }    
        
    }

}