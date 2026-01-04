using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class CustomersController : INotifyPropertyChanged
{
    // Singleton
    private static CustomersController? _instance;
    public static CustomersController Instance => _instance ??= new CustomersController();

    private CustomersController()
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

    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
