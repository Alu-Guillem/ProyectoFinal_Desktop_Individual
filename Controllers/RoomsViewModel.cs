using System.Collections.ObjectModel;
using System.ComponentModel;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Controllers;

public class RoomsViewModel : INotifyPropertyChanged
{
    // Singleton
    private static RoomsViewModel? _instance;
    public static RoomsViewModel Instance => _instance ??= new RoomsViewModel();

    private RoomsViewModel()
    {
        _currentRoom = new RoomModel();
    }

    // La lista de todas las rooms
    public ObservableCollection<RoomModel> Rooms { get; } = new();

    // La room que se está editando/creando actualmente
    private RoomModel _currentRoom;
    public RoomModel CurrentRoom
    {
        get => _currentRoom;
        set
        {
            if (value == _currentRoom) return;
            _currentRoom = value;
            OnPropertyChanged(nameof(CurrentRoom));
        }
    }

    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
