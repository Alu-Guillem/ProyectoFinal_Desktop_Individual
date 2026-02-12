using System.Windows.Controls;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Services;

public class NavigationService
{
    // Singleton
    private static NavigationService? _instance;
    public static NavigationService Instance => _instance ??= new NavigationService();


    private List<UserControl> _viewsStack = [new RoomsView()];

    public UserControl CurrentView
    {
        get => ViewsStack.First();
        private set
        {
            if (value == ViewsStack.First()) return;
            _viewsStack.Insert(0, value);
        }
    }

    public List<UserControl> ViewsStack
    {
        get => _viewsStack;
    }

    /// <summary>
    /// Navega a un UserControl del tipo especificado
    /// </summary>
    /// <typeparam name="T">Tipo del UserControl</typeparam>
    public void NavigateTo<T>() where T : UserControl, new()
    {
        var view = ViewsStack.Find(v => v is T) ?? new T();
        CurrentView = view;
    }

    public void NavigateBack()
    {
        if (ViewsStack.Count == 1) return;
        ViewsStack.RemoveAt(0);
    }
}