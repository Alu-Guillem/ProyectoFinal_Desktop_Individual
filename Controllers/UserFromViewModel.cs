using System.Windows;
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


    public string FullName => $"{User.FirstName} {User.LastName}";
    
    private RelayCommand _saveCommand;
    public RelayCommand SaveUserCommand => _saveCommand ??= new RelayCommand(async _ => await SaveUser());

    private async Task SaveUser()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            return;
        }

        try
        {
            var result = await _userService.UpdateUser(User);
            Console.WriteLine(result.Data);
            if (result.Success)
            {
                

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