using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class CustomersViewModel : BaseViewModel
{
    // Singleton
    private static CustomersViewModel? _instance;
    public static CustomersViewModel Instance => _instance ??= new CustomersViewModel();

    private CustomersViewModel()
    {
        _currentCustomer = new CustomerModel();
    }

    // La lista de todos los customers
    public ObservableCollection<CustomerModel> Customers { get; } = new();

    // El customer que se está editando/creando actualmente
    private CustomerModel _currentCustomer;
    public CustomerModel CurrentCustomer
    {
        get => _currentCustomer;
        set
        {
            if (value == _currentCustomer) return;
            _currentCustomer = value;
            OnPropertyChanged(nameof(CurrentCustomer));
        }
    }
}
