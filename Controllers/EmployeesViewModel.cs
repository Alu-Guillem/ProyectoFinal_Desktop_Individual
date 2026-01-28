using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class EmployeesViewModel : BaseViewModel
{
    // Singleton
    private static EmployeesViewModel? _instance;
    public static EmployeesViewModel Instance => _instance ??= new EmployeesViewModel();

    private EmployeesViewModel()
    {
        _currentEmployee = new EmployeeModel("Pere", "admin@peremaria.es", "admin");
    }

    // La lista de todos los employees
    public ObservableCollection<EmployeeModel> Employees { get; } =
    [
        new("Pere", "pere@peremaria.es", "admin"),
        new("Juan", "juan@peremaria.es", "employee"),
        new("Paco", "paco@peremaria.es", "employee")
    ];

    // El employee que se está editando/creando actualmente
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
}
