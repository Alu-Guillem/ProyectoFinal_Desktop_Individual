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
       InitalizeUser();
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
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(Initial));
            OnPropertyChanged(nameof(Role));
        }
    }


    public string FirstName => CurrentUser?.FirstName ?? "Invitado";

    public string Initial => CurrentUser.FirstName.Split("")[0];
    
    public string Role => CurrentUser.Role;
    

    private async void InitalizeUser()
    {
        await LoadUserInfo();
    }
    
    public async Task LoadUserInfo()
    {
        try
        {
            var result = await _userService.GetMe();
            CurrentUser = result.Data;
        }
        catch (Exception e)
        {
            _currentUser = null;
        }
    }
    
}
