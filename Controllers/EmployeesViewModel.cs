using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public class EmployeesViewModel : BaseViewModel
{
    // Singleton
    private static EmployeesViewModel? _instance;
    public static EmployeesViewModel Instance => _instance ??= new EmployeesViewModel();

    private readonly UserService _userService = new UserService();


    private EmployeesViewModel()
    {
        _currentEmployee = new EmployeeModel();
    }

    public ObservableCollection<EmployeeModel> Employees { get; } = new();

    public async Task LoadEmployees()
    {
        var result = await _userService.GetAllEmployees();

        Employees.Clear();

        foreach (EmployeeModel employee in result.Data)
        {
            Employees.Add(employee);
        }
    }

    private EmployeeModel _currentEmployee;

    public EmployeeModel CurrentEmployee
    {
        get => _currentEmployee;
        set
        {
            if (value == _currentEmployee) return;
            _currentEmployee = value;
            OnPropertyChanged(nameof(CurrentEmployee));
        }
    }

    private RelayCommand _openEmployeeFormCommand;

    public RelayCommand OpenEmployeeFormCommand =>
        _openEmployeeFormCommand ??= new RelayCommand(OpenEmployeeForm);

    private void OpenEmployeeForm(object parameter)
    {
        if (SessionService.Instance.CurrentUser?.Role != "admin") return;

        if (parameter is EmployeeModel employee)
        {
            UserFromViewModel.Instance.User = employee;
            UserFromViewModel.Instance.IsEditing = true;
        }
        else
        {
            UserFromViewModel.Instance.User = new EmployeeModel();
            UserFromViewModel.Instance.IsEditing = false;
        }

        NavigationViewModel.Instance.NavigateTo<EmployeesFormView>();
    }

    private RelayCommand _delteEmployeeCommand;

    public RelayCommand DeleteEmployeeCommand =>
        _delteEmployeeCommand ??= new RelayCommand(async parameter => DeleteEmployee(parameter));

    private async Task DeleteEmployee(object parameter)
    {
        if (parameter is not EmployeeModel employee) return;
        Console.WriteLine($"Deleting {employee.UserId}");

        try
        {
            var result = await _userService.DeleteUser(employee.UserId);

            if (result.Success)
            {
                var employeeToRemove = Employees.First(e => e.UserId == employee.UserId);
                Employees.Remove(employeeToRemove);
            }
            else
            {
                Console.WriteLine("No se elimino");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar: {ex.Message}");
        }
    }
}