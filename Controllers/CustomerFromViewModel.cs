using System.Windows;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public class CustomerFromViewModel : BaseViewModel
{
    private static CustomerFromViewModel? _instance;
    public static CustomerFromViewModel Instance => _instance ??= new();

    private readonly UserService _userService = new UserService();

    
    private CustomerModel _customer;
    public CustomerModel Customer
    {
        get => _customer;
        set
        {
            _customer = value;
            OnPropertyChanged(nameof(Customer));
        }
    }
    
    public string FirstName
    {
        get => Customer.FirstName;
        set
        {
            Customer.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(FullName)); // actualizar TextBlock
        }
    }

    public string LastName
    {
        get => Customer.LastName;
        set
        {
            Customer.LastName = value;
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(FullName));
        }
    }

    public string FullName => $"{Customer.FirstName} {Customer.LastName}";
    
    private RelayCommand _saveCommand;
    public RelayCommand SaveCustomerCommand => _saveCommand ??= new RelayCommand(async _ => await SaveCustomerAsync());

    private async Task SaveCustomerAsync()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            return;
        }


        try
        {
            var result = await _userService.UpdateCustomer(Customer);
            Console.WriteLine(result.Data);
            if (result.Success)
            {
                var customersVM = CustomersViewModel.Instance;
                var originalCustomer = customersVM.Customers.FirstOrDefault(c => c.UserId == Customer.UserId);

                if (originalCustomer != null)
                {
                    // 2. Actualizamos las propiedades del objeto que ya está en la lista
                    originalCustomer.FirstName = result.Data.FirstName;
                    originalCustomer.LastName = result.Data.LastName;
                    // ... actualizar el resto de campos
                }

                MessageBox.Show($"Usuario actualizado: {result.Data.FirstName}");

            }
            else
            {
                MessageBox.Show($"Error al guardar: {result.Error}");
            }
        }
        catch (Exception ex)
        {

        }
    }

}

