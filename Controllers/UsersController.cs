using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class UsersController : INotifyPropertyChanged
{
    // Singleton
    private static UsersController? _instance;
    public static UsersController Instance => _instance ??= new UsersController();

    private UsersController()
    {
        _currentUser = new UserModel();
    }

    // La lista de todos los users
    public ObservableCollection<UserModel> Users { get; } = new();

    // El user que se está editando/creando actualmente
    private UserModel _currentUser;
    public UserModel CurrentUser
    {
        get => _currentUser;
        set
        {
            if (value == _currentUser) return;
            _currentUser = value;
            OnPropertyChanged(nameof(CurrentUser));
        }
    }

    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
