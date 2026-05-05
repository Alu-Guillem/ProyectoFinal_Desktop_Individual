using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Controls;
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
        ShowStats(null);
    }

    private readonly RoomService _roomService = new();


    private readonly BookingsService _bookingsService = new();


    // La lista de todas las rooms
    public ObservableCollection<RoomModel> Rooms { get; } = new();

    //La lista de todas las reservas
    public ObservableCollection<BookingModel> Bookings { get; } = new();

    //Lista de las estadisticas de las habitaciones
    public ObservableCollection<RoomStatModel> RoomStatsList { get; } = new();

    // Filtos
    //Nombre
    private string _searchName = "";
    public string SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            OnPropertyChanged(nameof(SearchName));
            _ = LoadRooms();
        }
    }

    //Ocupacion
    public List<string> OccupedFilters { get; } = new()
    {
        "", "Libre", "Ocupado", "Mantenimiento", "Cerrado"
    };
    private bool? ParseOccupedFilter()
    {
        return FilterOccuped switch
        {
            "Libre" => false,
            "Ocupado" => true,
            "Mantenimiento" => null, // Aquí podrías implementar lógica adicional para filtrar por mantenimiento
            "Cerrado" => null, // Aquí podrías implementar lógica adicional para filtrar por cerrado
            _ => null
        };
    }

    private string _filterOccuped;
    public string FilterOccuped
    {
        get => _filterOccuped;
        set
        {
            _filterOccuped = value;
            OnPropertyChanged(nameof(FilterOccuped));
            _ = LoadRooms();
        }
    }

   

    /*TIPO DE HABITACION*/
    public List<string> RoomTypes { get; } = new()
    {
        "", "Estandar", "Suite", "Double", "Simple"
    };

    //Cargar las habitaciones
    public async Task LoadRooms()
    {
        bool? occuped = ParseOccupedFilter();
        string name = SearchName;

        var result = await _roomService.GetAllRooms(name, occuped);

        if (result.Success && result.Data != null)
        {
            Rooms.Clear();
            foreach (var room in result.Data)
                Rooms.Add(room);
        }
        else
        {
            MessageBox.Show(result.Error?.Message ?? "Error cargando habitaciones");
        }
    }



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

    private async void Save(object? parameter)
    {
        await SaveAsync();
    }

    private RelayCommand saveCommand;
    public ICommand SaveCommand => saveCommand ??= new RelayCommand(Save);

    //Guardar (POST/PUT)
    private async Task SaveAsync()
    {
        List<string> errors = new();

        if (string.IsNullOrWhiteSpace(CurrentRoom.Name))
            errors.Add("Nombre");

        if (string.IsNullOrWhiteSpace(CurrentRoom.Type))
            errors.Add("Tipo");

        if (CurrentRoom.PricePerNight < 1)
            errors.Add("Precio");

        if (CurrentRoom.OccupancyLimit < 1 || CurrentRoom.OccupancyLimit > 10)
            errors.Add("Ocupacion Maxima (1-10)");

        if (CurrentRoom.Number <= 0)
            errors.Add("Numero");

        if (CurrentRoom.Offer < 0 || CurrentRoom.Offer > 100)
            errors.Add("Oferta (0-100)");

        if (errors.Any())
        {
            MessageBox.Show("Faltan o son incorrectos:\n" + string.Join("\n", errors),
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return;
        }

        ApiResult<RoomModel> result;

        if (string.IsNullOrEmpty(CurrentRoom.RoomId))
        {
            result = await _roomService.CreateRoom(CurrentRoom);
        }
        else
        {
            result = await _roomService.UpdateRoom(CurrentRoom);
        }

        if (!result.Success)
        {
            MessageBox.Show(result.Error?.Message ?? "Error guardando habitación");
            return;
        }

        MessageBox.Show("Guardado correctamente");

        await LoadRooms();
        NavigationViewModel.Instance.BackCommand.Execute(null);
    }


    private async Task DeleteAsync()
    {

        if (MessageBox.Show("¿Seguro que quieres eliminar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            if (MessageBox.Show("¿Seguro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //ELIMINAR (DELETE)
                if (!CurrentRoom.Occuped)
                {
                    var result = await _roomService.DeleteRoom(CurrentRoom.RoomId);
                    if (!result.Success)
                    {
                        MessageBox.Show(result.Error?.Message ?? "Error eliminando habitación");
                        return;
                    }

                    await LoadRooms();
                    NavigationViewModel.Instance.BackCommand.Execute(null);
                }else{
                    MessageBox.Show("No se puede eliminar habitaciones reservadas", "Habitacion Ocupada", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
            }
            else { return; }   
        }
        else { return; }

    }

    

    private RelayCommand deleteCommand;
    public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(Delete);
    private async void Delete(object? parameter)
    {
        await DeleteAsync();
    }

    //Boton cerrar habitacion
    public RelayCommand closeCommand;
    public ICommand CloseCommand => closeCommand ??= new RelayCommand(CloseRoom);
    private async void CloseRoom(object? parameter)
    {
        if (CurrentRoom.Occuped)
        {
            MessageBox.Show("No se puede cerrar una habitación ocupada", "Habitacion Ocupada", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var confirm = "";
        if (CurrentRoom.Closed == true) { 
            confirm = "La habitación está cerrada. ¿Seguro que quieres reabrir esta habitación?";
        }else{
            confirm = "¿Seguro que quieres cerrar esta habitación?";
        }
        if (MessageBox.Show(confirm, "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var result = await _roomService.CloseRoom(CurrentRoom);
            if (!result.Success)
            {
                MessageBox.Show(result.Error?.Message ?? "Error cerrando habitación");
                return;
            }
        }
        
        await LoadRooms();
        NavigationViewModel.Instance.BackCommand.Execute(null);
    }

    //Boton Mantenimiento
    public RelayCommand mantenimentCommand;
    public ICommand MantenimentCommand => mantenimentCommand ??= new RelayCommand(MaintenanceRoom);
    private async void MaintenanceRoom(object? parameter)
    {
        
        if (MessageBox.Show("¿Poner la habitación en mantenimiento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var result = await _roomService.CloseRoom(CurrentRoom);
            if (!result.Success)
            {
                MessageBox.Show(result.Error?.Message ?? "Error cerrando habitación");
                return;
            }
        }

        await LoadRooms();
        NavigationViewModel.Instance.BackCommand.Execute(null);
    }


    /*Apartado Estadisticas / Historial*/

    public int TotalRooms => Rooms.Count;
    public int OccupedRooms => Rooms.Count(r => r.Occuped);


    // 1. Definir los criterios de ordenación
    private string _currentSortOrder = "Revenue"; // Por defecto: más rentables

    // 2. Comandos para cambiar el orden
    private RelayCommand? _sortByRevenueCommand;
    public ICommand SortByRevenueCommand => _sortByRevenueCommand ??= new RelayCommand(_ => { _currentSortOrder = "Revenue"; _ = ShowStats(null); });

    private RelayCommand? _sortByBookingsCommand;
    public ICommand SortByBookingsCommand => _sortByBookingsCommand ??= new RelayCommand(_ => { _currentSortOrder = "Bookings"; _ = ShowStats(null); });

    // 3. Modificar ShowStats para ordenar
    private async Task ShowStats(object? parameter)
    {
        var result = await _roomService.RoomStats();

        if (result.Success && result.Data != null)
        {
            // Aplicamos ordenación principal y secundaria para desempatar
            var sortedData = _currentSortOrder == "Revenue"
                ? result.Data
                    .OrderByDescending(s => s.TotalRevenue)
                    .ThenByDescending(s => s.TotalBookings) // Si empatan en dinero, gana la que más reservas tenga
                    .ToList()
                : result.Data
                    .OrderByDescending(s => s.TotalBookings)
                    .ThenByDescending(s => s.TotalRevenue) // Si empatan en reservas, gana la que más dinero haya generado
                    .ToList();

            Application.Current.Dispatcher.Invoke(() => {
                RoomStatsList.Clear();
                foreach (var stat in sortedData)
                {
                    RoomStatsList.Add(stat);
                }
            });
        }
    }





    //   
}
