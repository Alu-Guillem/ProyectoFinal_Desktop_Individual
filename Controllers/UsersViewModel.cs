using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class UsersViewModel : BaseViewModel
{
    // Singleton
    private static UsersViewModel? _instance;
    public static UsersViewModel Instance => _instance ??= new UsersViewModel();

    private UsersViewModel()
    {
        _currentUser = new UserModel(null, null);
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
}
