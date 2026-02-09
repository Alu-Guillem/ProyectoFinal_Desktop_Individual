using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;

namespace PereMaria.GestorHotel.Controllers;

public class UsersViewModel : BaseViewModel
{
    // Singleton
    private static UsersViewModel? _instance;
    public static UsersViewModel Instance => _instance ??= new UsersViewModel();

    private readonly UserService _userService = new UserService();

    private UsersViewModel()
    {
    }


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