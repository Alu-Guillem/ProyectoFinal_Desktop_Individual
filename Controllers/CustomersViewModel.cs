using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public class CustomersViewModel : BaseViewModel
{
    // Singleton
    private static CustomersViewModel? _instance;
    public static CustomersViewModel Instance => _instance ??= new CustomersViewModel();

    private readonly UserService _userService = new UserService();

    
    private CustomersViewModel()
    {
        _currentCustomer = new CustomerModel();
        
        CustomersView = CollectionViewSource.GetDefaultView(Customers);

        CustomersView.Filter = ChangedEmail;

    }

    // La lista de todos los customers
    public ObservableCollection<CustomerModel> Customers { get; } = new();
    public ICollectionView CustomersView { get; }

    public async Task LoadCustomers()
    {
        var result = await _userService.GetAllCustomers();
        
        Customers.Clear();

        foreach (CustomerModel customer in result.Data)
        {
            Customers.Add(customer);
        }
    }
    
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

    public string _fitrerText;
    public String FiltrerText
    {
        get => _fitrerText;
        set
        {
            _fitrerText = value;
            OnPropertyChanged(nameof(FiltrerText));
            CustomersView.Refresh();
        }
    }


    private RelayCommand _openCustomerFormCommand;
    public RelayCommand OpenCustomerFormCommand =>
        _openCustomerFormCommand ??= new RelayCommand(OpenCustomerForm);
    private void OpenCustomerForm(object parameter)
    {
        Console.WriteLine("Entro en Form");
        
        if (parameter is not CustomerModel customer) return;

        CustomerFromViewModel.Instance.Customer = customer;

        NavigationViewModel.Instance.NavigateTo<CustomersFormView>();
    }
    
    private bool ChangedEmail(object obj)
    {
        if (obj is not CustomerModel c)
            return false;

        if (string.IsNullOrWhiteSpace(FiltrerText))
            return true;

        return c.Email.Contains(FiltrerText, StringComparison.OrdinalIgnoreCase)
               || c.Dni.Contains(FiltrerText, StringComparison.OrdinalIgnoreCase)
               || c.FirstName.Contains(FiltrerText, StringComparison.OrdinalIgnoreCase);        
    }

}
