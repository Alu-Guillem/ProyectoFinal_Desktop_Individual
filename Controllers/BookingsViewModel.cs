using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel.Controllers;

public class BookingsViewModel : BaseViewModel
{
    // Singleton
    private static BookingsViewModel? _instance;
    public static BookingsViewModel Instance => _instance ??= new BookingsViewModel();

    private BookingsViewModel()
    {
        SessionService.Instance.SetToken(
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI2OTc3ODFjMDBiZTMyOTdlZmFiNTM1MGEiLCJyb2xlIjoiYWRtaW4iLCJpYXQiOjE3Njk0NDAwMzAsImV4cCI6MTc3MDczNjAzMH0.vygSqWOAGZhzLOkXljy5jJkwXbwxzL-1QERf0xzkGo8");

        _currentBooking = new BookingModel();

        FilteredCustomers = CollectionViewSource.GetDefaultView(Customers);
        FilteredCustomers.Filter = FilterCustomers;

        FilteredRooms = CollectionViewSource.GetDefaultView(Rooms);
        FilteredRooms.Filter = FilterRooms;

        _ = LoadBookings();
        _ = LoadCustomers();
        LoadRoomsLocal();
    }

    // La lista de todos los Bookings
    public ObservableCollection<BookingModel> Bookings { get; } = [];

    public ObservableCollection<CustomerModel> Customers { get; } = [];
    public ObservableCollection<RoomModel> Rooms { get; } = [];

    public ICollectionView FilteredCustomers { get; }
    public ICollectionView FilteredRooms { get; }

    private string _userSearchText = "";

    public string UserSearchText
    {
        get => _userSearchText;
        set
        {
            if (value == _userSearchText) return;
            _userSearchText = value;
            OnPropertyChanged(nameof(UserSearchText));
            FilteredCustomers.Refresh();
        }
    }

    private string _roomSearchText = "";

    public string RoomSearchText
    {
        get => _roomSearchText;
        set
        {
            if (value == _roomSearchText) return;
            _roomSearchText = value;
            OnPropertyChanged(nameof(RoomSearchText));
            FilteredRooms.Refresh();
        }
    }

    private CustomerModel? _selectedCustomer;

    public CustomerModel? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            if (Equals(value, _selectedCustomer)) return;
            _selectedCustomer = value;
            OnPropertyChanged(nameof(SelectedCustomer));

            CurrentBooking.UserId = _selectedCustomer?.UserId ?? "";
            if (_selectedCustomer != null)
                UserSearchText = _selectedCustomer.Dni ?? "";
        }
    }

    private RoomModel? _selectedRoom;

    public RoomModel? SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            if (Equals(value, _selectedRoom)) return;
            _selectedRoom = value;
            OnPropertyChanged(nameof(SelectedRoom));

            CurrentBooking.RoomId = _selectedRoom?.RoomId ?? "";
            if (_selectedRoom != null)
                RoomSearchText = _selectedRoom.Name ?? "";
        }
    }

    // El booking que se está editando/creando actualmente
    private BookingModel _currentBooking;

    public BookingModel CurrentBooking
    {
        get => _currentBooking;
        set
        {
            if (value == _currentBooking) return;
            _currentBooking = value;
            OnPropertyChanged(nameof(CurrentBooking));
            OnPropertyChanged(nameof(IsEditing));
            OnPropertyChanged(nameof(IsCreating));
            OnPropertyChanged(nameof(FormTitle));
            SyncSelectedCustomer();
            SyncSelectedRoom();
        }
    }

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (value == _isLoading) return;
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(IsNotLoading));
        }
    }

    public bool IsNotLoading => !IsLoading;

    public string FormTitle => IsEditing ? "Modificar reserva" : "Crear reserva";
    public bool IsEditing => CurrentBooking.BookingId != null;
    public bool IsCreating => CurrentBooking.BookingId == null;

    private async Task LoadBookings()
    {
        try
        {
            IsLoading = true;
            Bookings.Clear();

            var res = await BookingsService.Instance.GetBookings();

            res?.ForEach(b => Bookings.Add(b));
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadCustomers()
    {
        try
        {
            var res = await new UserService().GetAllCustomers();

            if (!res.Success || res.Data == null)
            {
                var message = res.Error?.Message ?? "No se pudieron cargar los clientes";
                throw new Exception(message);
            }

            Customers.Clear();
            res.Data.ForEach(c => Customers.Add(c));

            FilteredCustomers.Refresh();
            SyncSelectedCustomer();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadRoomsLocal()
    {
        Rooms.Clear();
        Rooms.Add(new RoomModel
        {
            RoomId = "69530df689fb7880d5983c24",
            Name = "Habitacion 1",
            Offer = 20,
            PricePerNight = 70,
            OccupancyLimit = 2,
        });

        FilteredRooms.Refresh();
        SyncSelectedRoom();
    }

    private void SyncSelectedCustomer()
    {
        if (string.IsNullOrWhiteSpace(CurrentBooking.UserId))
        {
            SelectedCustomer = null;
            UserSearchText = "";
            return;
        }

        var match = Customers.FirstOrDefault(c => c.UserId == CurrentBooking.UserId);
        UserSearchText = match?.Dni ?? "";
        if (!Equals(match, SelectedCustomer))
        {
            SelectedCustomer = match;
        }
    }

    private void SyncSelectedRoom()
    {
        if (string.IsNullOrWhiteSpace(CurrentBooking.RoomId))
        {
            SelectedRoom = null;
            RoomSearchText = "";
            return;
        }

        var match = Rooms.FirstOrDefault(r => r.RoomId == CurrentBooking.RoomId);
        RoomSearchText = match?.Name ?? "";
        if (!Equals(match, SelectedRoom))
        {
            SelectedRoom = match;
        }
    }

    private bool FilterCustomers(object item)
    {
        if (item is not CustomerModel customer) return false;
        if (string.IsNullOrWhiteSpace(UserSearchText)) return true;

        return customer.Dni?.IndexOf(UserSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private bool FilterRooms(object item)
    {
        if (item is not RoomModel room) return false;
        if (string.IsNullOrWhiteSpace(RoomSearchText)) return true;

        return room.Name?.IndexOf(RoomSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public RelayCommand NavigateFormCommand => new(parameter =>
    {
        NavigationViewModel.Instance.NavigateTo<BookingsFormView>();
        CurrentBooking = parameter as BookingModel ?? new BookingModel();
    });

    public RelayCommand SaveBookingCommand => new(async void (_) =>
    {
        try
        {
            IsLoading = true;

            if (IsCreating)
            {
                // Crear nueva reserva
                Console.WriteLine(CurrentBooking);
                var createdBooking = await BookingsService.Instance.CreateBooking(CurrentBooking);
                if (createdBooking != null)
                {
                    MessageBox.Show("Reserva creada exitosamente", "Éxito", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
            {
                // Actualizar reserva existente
                var updatedBooking =
                    await BookingsService.Instance.UpdateBooking(CurrentBooking.BookingId!, CurrentBooking);
                if (updatedBooking != null)
                {
                    MessageBox.Show("Reserva actualizada exitosamente", "Éxito", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }

            await LoadBookings();


            CurrentBooking = Bookings.ToList().Find(b => b.BookingId!.Equals(CurrentBooking.BookingId)) ??
                             new BookingModel();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    });

    public RelayCommand CancelCommand => NavigationViewModel.Instance.BackCommand;

    public RelayCommand CancelBookingCommand => new(async void (_) =>
    {
        var result = MessageBox.Show(
            "¿Está seguro de que desea cancelar esta reserva?",
            "Confirmar cancelación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            IsLoading = true;
            var success = await BookingsService.Instance.CancelBooking(CurrentBooking.BookingId!);
            if (success)
            {
                MessageBox.Show("Reserva cancelada exitosamente", "Éxito", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                CurrentBooking.Status = "canceled";
                OnPropertyChanged(nameof(CurrentBooking));
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }, _ => IsEditing && CurrentBooking.Status != "canceled");

    public RelayCommand ReloadCommand => new(async void (_) => await LoadBookings());
}