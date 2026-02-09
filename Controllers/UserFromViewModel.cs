using System.Windows;
using Newtonsoft.Json;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;

namespace PereMaria.GestorHotel.Controllers;

public class UserFromViewModel : BaseViewModel
{
    private static UserFromViewModel? _instance;
    public static UserFromViewModel Instance => _instance ??= new();

    private readonly UserService _userService = new UserService();

    private UserModel _user;

    public UserModel User
    {
        get => _user;
        set
        {
            _user = value;
            OnPropertyChanged(nameof(User));
        }
    }


    private bool _isEditing;

    public bool IsEditing
    {
        get => _isEditing;
        set
        {
            _isEditing = value;
            OnPropertyChanged(nameof(IsEditing));
            OnPropertyChanged(nameof(IsTextBoxEnabled));
        }
    }

    public bool IsTextBoxEnabled => !IsEditing;
    public string Visibility => IsEditing ? "Hidden" : "Visibility";

    public string FirstName
    {
        get => User.FirstName;
        set
        {
            User.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(FullName));
        }
    }

    public string LastName
    {
        get => User.LastName;
        set
        {
            User.LastName = value;
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(FullName));
        }
    }

    public string Password
    {
        get => User.Password;
        set
        {
            User.Password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string Email
    {
        get => User.Email;
        set
        {
            User.Email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string Role
    {
        get => User.Role;
        set
        {
            User.Role = value.ToLower();
            OnPropertyChanged(nameof(Role));
        }
    }

    public string Gender
    {
        get { return User is CustomerModel customer ? customer.Gender : string.Empty; }
        set
        {
            if (User is CustomerModel customer)
            {
                customer.Gender = value.ToLower();
                OnPropertyChanged(nameof(Gender));
            }
        }
    }

    public string Dni
    {
        get { return User is CustomerModel customer ? customer.Dni : string.Empty; }
        set
        {
            if (User is CustomerModel customer)
            {
                customer.Dni = value;
                OnPropertyChanged(nameof(Dni));
            }
        }
    }

    public string BirthDate
    {
        get
        {
            if (User is CustomerModel customer)
            {
                return customer.BirthDate;
            }

            return DateTime.UtcNow.ToString("dd/MM/yyyy");
        }
        set
        {
            if (User is CustomerModel customer)
            {
                customer.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
    }

    public string City
    {
        get { return User is CustomerModel customer ? customer.City : string.Empty; }
        set
        {
            if (User is CustomerModel customer)
            {
                customer.City = value;
                OnPropertyChanged(nameof(City));
            }
        }
    }

    public string FullName => $"{User.FirstName} {User.LastName}";

    private RelayCommand _saveCommand;
    public RelayCommand SaveUserCommand => _saveCommand ??= new RelayCommand(async _ => await SaveUser());

    private async Task SaveUser()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            return;
        }

        if (IsEditing)
        {
            try
            {
                var result = await _userService.UpdateUser(User);
                Console.WriteLine(result.Data);
                if (result.Success)
                {
                    ShowMessageBox($"Usuario actualizado: {result.Data.FirstName}", "Usuario",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ShowMessageBox($"Error al guardar: {result.Error}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            if (User is CustomerModel customerModel)
            {
                try
                {
                    customerModel.Role = "customer";
                    var result = await _userService.CreateCustomer(customerModel);
                    Console.WriteLine(result.Data);
                    if (result.Success)
                    {
                        ShowMessageBox($"Usuario creado: {result.Data.FirstName}", "Usuario",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        ShowMessageBox($"Error al crear: {result.Error}", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    var result = await _userService.CreateEmployee(User as EmployeeModel);

                    Console.WriteLine(result.Data);
                    if (result.Success)
                    {
                        ShowMessageBox($"Usuario creado: {result.Data.FirstName}", "Usuario",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        ShowMessageBox($"Error al crear: {result.Error}", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}