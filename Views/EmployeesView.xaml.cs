using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

namespace PereMaria.GestorHotel.Views;

public partial class EmployeesView : UserControl
{
    public EmployeesView()
    {
        InitializeComponent();

        Loaded += EmployeesView_Loaded;

    }
    
    private async void EmployeesView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await EmployeesViewModel.Instance.LoadEmployees();
    }
}