using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PereMaria.GestorHotel.Controllers;

public class RoomsViewModel : BaseViewModel
{
    //Boton Cancelar
    public RelayCommand CancelCommand => NavigationViewModel.Instance.BackCommand;

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

    private RelayCommand backCommand;
    public ICommand BackCommand => backCommand ??= new RelayCommand(Back);

    private void Back(object commandParameter)
    {
    }



    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _type;
    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged(nameof(Type));
        }
    }

    private double _price;
    public double Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    private int _limit;
    public int Limit
    {
        get => _limit;
        set
        {
            _limit = value;
            OnPropertyChanged(nameof(Limit));
        }
    }

    private int _number;
    public int Number
    {
        get => _number;
        set
        {
            _number = value;
            OnPropertyChanged(nameof(Number));
        }
    }



    private RelayCommand saveCommand;
    public ICommand SaveCommand => saveCommand ??= new RelayCommand(Save);

    private void Save(object? obj)
    {
        MessageBox.Show($"SAVE ejecutado \n{CurrentRoom.name}\n{CurrentRoom.type}\n{CurrentRoom.pricePerNight}\n{CurrentRoom.occupancyLimit}\n{CurrentRoom.number}");
        
    }




}
