using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class EmployeesController : INotifyPropertyChanged
{
    // Singleton
    private static EmployeesController? _instance;
    public static EmployeesController Instance => _instance ??= new EmployeesController();

    private EmployeesController()
    {
        _currentEmployee = new EmployeeModel();
    }

    // La lista de todos los employees
    public ObservableCollection<EmployeeModel> Employees { get; } = new();

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

    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
