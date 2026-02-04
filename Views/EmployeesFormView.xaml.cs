using System.Windows;
using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

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