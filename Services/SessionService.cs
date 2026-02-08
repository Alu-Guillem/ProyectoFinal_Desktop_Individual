using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class SessionService
{
    public string? JWT { get; set; }

    
    private static SessionService? _instance;
    public static SessionService Instance => _instance ??= new SessionService();

    private UserService? _userService;
    private UserService UserService => _userService ??= new UserService();
    
    public event Action? SessionChanged;

    public void SetToken(string token) {
        JWT = token;
        ApiService.Instance.SetToken(token);
    }
    
    private UserModel? _currentUser;
    public UserModel? CurrentUser
    {
        get => _currentUser;
        set
        {
            if (value == _currentUser) return;
            _currentUser = value;
            SessionChanged?.Invoke();

        }
    }

    
    
    
    
    public async Task LoadUserInfo()
    {
        try
        {
            var result = await UserService.GetMe();
            CurrentUser = result.Data;
        }
        catch (Exception e)
        {
            _currentUser = null;
        }
    }

}