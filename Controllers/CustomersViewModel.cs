using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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
        try
        {
            var result = await _userService.GetAllCustomers();


            if (result.Success)
            {
                Customers.Clear();

                foreach (CustomerModel customer in result.Data)
                {
                    Customers.Add(customer);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
        if (parameter is CustomerModel customer)
        {
            UserFromViewModel.Instance.User = customer;
            UserFromViewModel.Instance.IsEditing = true;
        }
        else
        {
            UserFromViewModel.Instance.User = new CustomerModel();
            UserFromViewModel.Instance.IsEditing = false;
        }

        NavigationViewModel.Instance.NavigateTo<CustomersFormView>();
    }

    private RelayCommand _delteCustomerCommand;

    public RelayCommand DeleteCustomerCommand =>
        _delteCustomerCommand ??= new RelayCommand(async parameter => DeleteCustomer(parameter));

    private async Task DeleteCustomer(object parameter)
    {
        
        
        if (parameter is not CustomerModel customer) return;

        
        var confirmarEliminar =
            ShowMessageBox($"Seguro que quieres eliminar al usuario: {customer.FirstName} {customer.LastName}",
                "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (confirmarEliminar == MessageBoxResult.Yes)
        {
            try
            {
                var result = await _userService.DeleteUser(customer.UserId);

                if (result.Success)
                {
                    var customerToRemove = Customers.First(c => c.UserId == customer.UserId);
                    Customers.Remove(customerToRemove);
                }
                else
                {
                    ShowMessageBox(result.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
            }
        }
        

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