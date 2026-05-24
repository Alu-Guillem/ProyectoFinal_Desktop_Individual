using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;

namespace PereMaria.GestorHotel.Controllers;

public class UsersViewModel : BaseViewModel
{
    private static UsersViewModel? _instance;
    public static UsersViewModel Instance => _instance ??= new UsersViewModel();

    private readonly UserService _userService = new UserService();

    public ObservableCollection<UserModel> Users { get; } = new();

    private UsersViewModel()
    {
        _ = LoadUsersAsync();
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

    public async Task LoadUsersAsync()
    {
        try
        {
            var result = await _userService.GetAllUsers();

            if (result != null && result.Success && result.Data != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Users.Clear();
                    foreach (var user in result.Data)
                    {
                        Users.Add(user);
                    }
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cargando usuarios: {ex.Message}");
        }
    }


//
}