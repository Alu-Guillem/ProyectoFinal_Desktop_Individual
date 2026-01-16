using System.ComponentModel;
using System.Windows.Controls;

namespace PereMaria.GestorHotel.Services;

public class NavigationService : INotifyPropertyChanged
{
    // Singleton
    private static NavigationService? _instance;
    public static NavigationService Instance => _instance ??= new NavigationService();


    private NavigationService()
    {
    }


    // La vista actual
    private UserControl? _currentView;

    public UserControl? CurrentView
    {
        get => _currentView;
        private set
        {
            if (value == _currentView) return;
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }

    /// <summary>
    /// Navega a un UserControl del tipo especificado
    /// </summary>
    /// <typeparam name="T">Tipo del UserControl</typeparam>
    public void NavigateTo<T>() where T : UserControl, new()
    {
        CurrentView = new T();
    }

    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}