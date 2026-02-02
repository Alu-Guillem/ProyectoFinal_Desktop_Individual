using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

namespace PereMaria.GestorHotel.Views;

public partial class CustomersView : UserControl
{
    public CustomersView()
    {
        InitializeComponent();
        Loaded += CustomersView_Loaded;
    }
    
    private async void CustomersView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await CustomersViewModel.Instance.LoadCustomers();
    }
    
}