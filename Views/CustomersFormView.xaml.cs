using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PereMaria.GestorHotel.Views;

public partial class CustomersFormView : UserControl
{
    public CustomersFormView()
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
    private async void Invoice_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is UserFromViewModel vm && vm.User != null)
        {
            InvoiceViewModel.Instance.SelectedUser = vm.User;
            await InvoiceViewModel.Instance.GetInvoices(vm.User);
            NavigationViewModel.Instance.NavigateTo<InvoiceUsersList>();
        }
    }

    private void InvoiceRow_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is InvoiceUsersModel selectedInvoice)
        {
            InvoiceViewModel.Instance.MostrarPDF(selectedInvoice);
        }
    }

}