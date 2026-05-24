using Microsoft.VisualBasic;
using Microsoft.Win32;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using PereMaria.GestorHotel.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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

    //Model Room Occupancy
    public ObservableCollection<RoomOccupancyModel> RoomOccupancyDetails { get; } = new();

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
        "Todos", "Libre", "Ocupado", "Mantenimiento", "Cerrado"
    };
    

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

        bool? occuped = null;
        bool? maintenance = null;
        bool? closed = null;

        switch (FilterOccuped)
        {
            case "Ocupado":
                occuped = true;
                break;
            case "Mantenimiento":
                maintenance = true;
                break;
            case "Cerrado":
                closed = true;
                break;
            case "Libre":
                occuped = false;
                maintenance = false;
                closed = false;
                break;
            default:
                break;
        }

        string name = SearchName;
        
        var result = await _roomService.GetAllRooms(name, occuped, maintenance, closed);

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

    //Ruta de la imagen seleccionada para subir al servidor
    private string _selectedImagePath = "";
    public string SelectedImagePath
    {
        get => _selectedImagePath;
        set
        {
            _selectedImagePath = value;
            OnPropertyChanged(nameof(SelectedImagePath));
        }
    }

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

        //Subida de imagen
        if (!string.IsNullOrEmpty(SelectedImagePath))
        {
            string roomId = string.IsNullOrEmpty(CurrentRoom.RoomId) ? result.Data?.RoomId : CurrentRoom.RoomId;

            if (!string.IsNullOrEmpty(roomId))
            {
                bool fotoSubida = await _roomService.UploadRoomImage(roomId, SelectedImagePath);

                if (fotoSubida)
                {
                    if (result.Data != null)
                    {
                        CurrentRoom = result.Data;
                    }
                    else
                    {
                        CurrentRoom.Image = Path.GetFileName(SelectedImagePath);
                    }
                }
                else
                {
                    MessageBox.Show("La habitación se guardó, pero la imagen no se pudo subir.", "Aviso");
                }
            }

            SelectedImagePath = "";
        }

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            OnPropertyChanged(nameof(CurrentRoom));
            OnPropertyChanged(nameof(FullImageUrl));
        });

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
                }
                else
                {
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
        if (CurrentRoom.Closed == true)
        {
            confirm = "La habitación está cerrada. ¿Seguro que quieres reabrir esta habitación?";
        }
        else
        {
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

    private string _currentSortOrder = "Revenue"; // Por defecto: más rentables


    //Años para el filtro
    public ObservableCollection<string> AvailableYears { get; } = new()
    {
       "2023", "2024", "2025", "2026"
    };

    static string CYear = DateTime.Now.Year.ToString();

    private string _selectedYear = CYear;
    public string SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            OnPropertyChanged(nameof(SelectedYear));
            _ = ShowStats(null); // Recargar estadísticas al cambiar
        }
    }

    //Diferencia

    private double _totalRevenueDisplay;
    public double TotalRevenueDisplay
    {
        get => _totalRevenueDisplay;
        set { _totalRevenueDisplay = value; OnPropertyChanged(nameof(TotalRevenueDisplay)); }
    }

    private string _revenueComparison; // Almacenará el "+100€ (100%)"
    public string RevenueComparison
    {
        get => _revenueComparison;
        set { _revenueComparison = value; OnPropertyChanged(nameof(RevenueComparison)); }
    }


    private RelayCommand? _sortByRevenueCommand;
    public ICommand SortByRevenueCommand => _sortByRevenueCommand ??= new RelayCommand(_ => { _currentSortOrder = "Revenue"; _ = ShowStats(null); });

    private RelayCommand? _sortByBookingsCommand;
    public ICommand SortByBookingsCommand => _sortByBookingsCommand ??= new RelayCommand(_ => { _currentSortOrder = "Bookings"; _ = ShowStats(null); });

    //Metodo motrar Estadisticas.
    private async Task ShowStats(object? parameter)
    {
        var result = await _roomService.RoomStats(SelectedYear == CYear ? null : SelectedYear);

        double lastYearRevenue = await _roomService.RoomStats(SelectedYear == CYear ? (int.Parse(SelectedYear) - 1).ToString() : null).ContinueWith(t => t.Result.Success && t.Result.Data != null ? t.Result.Data.Sum(s => s.TotalRevenue) : 0);

        if (result.Success && result.Data != null)
        {
            double currentYearRevenue = result.Data.Sum(s => s.TotalRevenue);
            TotalRevenueDisplay = currentYearRevenue;


            if (SelectedYear == CYear)
            { // Calculo de la diferencia con el año anterior
                double difference = currentYearRevenue - lastYearRevenue;
                double percentage = lastYearRevenue == 0 ? 100.0 : (difference / lastYearRevenue) * 100; // Evito la división por cero
                                        //if (difference == 0) 100%
                RevenueComparison = $"{difference:C} ({percentage:0}%)";
            }
            else
            {
                RevenueComparison = string.Empty;
            }
            //
            if (result.Success && result.Data != null)
            {
                var sortedData = _currentSortOrder == "Revenue"
                    ? result.Data.OrderByDescending(s => s.TotalRevenue).ToList()
                    : result.Data.OrderByDescending(s => s.TotalBookings).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    RoomStatsList.Clear();
                    foreach (var stat in sortedData) RoomStatsList.Add(stat);
                });
            }
        }

       
    }

    //Vista estadisticas por habitaciones

    private string _selectedYearOccupancy = DateTime.Now.Year.ToString();
    public string SelectedYearOccupancy
    {
        get => _selectedYearOccupancy;
        set
        {
            _selectedYearOccupancy = value;
            OnPropertyChanged(nameof(SelectedYearOccupancy));
            // Solo lanzamos la petición de ocupación detallada
            _ = LoadRoomOccupancy();
        }
    }

    private RoomStatModel? _selectedStatRoom;
    public RoomStatModel? SelectedStatRoom
    {
        get => _selectedStatRoom;
        set { _selectedStatRoom = value; OnPropertyChanged(nameof(SelectedStatRoom)); }
    }

    private int _totalBookingsYear;
    public int TotalBookingsYear
    {
        get => _totalBookingsYear;
        set { _totalBookingsYear = value; OnPropertyChanged(nameof(TotalBookingsYear)); }
    }

    private double _totalRevenueYear;
    public double TotalRevenueYear
    {
        get => _totalRevenueYear;
        set { _totalRevenueYear = value; OnPropertyChanged(nameof(TotalRevenueYear)); }
    }



    public async Task LoadRoomOccupancy()
    {
        if (SelectedStatRoom == null) return;

        string year = _selectedYearOccupancy;

        var result = await _roomService.RoomOccupancy(SelectedStatRoom.RoomId, year);

        if (result.Success && result.Data != null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RoomOccupancyDetails.Clear();
                foreach (var item in result.Data)
                {
                    RoomOccupancyDetails.Add(item);
                }

                TotalBookingsYear = RoomOccupancyDetails.Sum(x => x.TotalBookings);
                TotalRevenueYear = RoomOccupancyDetails.Sum(x => x.TotalRevenue);
            });
        }
    }


    //EXPORTAR CSV
    private RelayCommand? _exportCsvCommand;
    public ICommand ExportCsvCommand => _exportCsvCommand ??= new RelayCommand(_ => ExportToCsv());

    private void ExportToCsv()
    {
        if (RoomStatsList == null || RoomStatsList.Count == 0)
        {
            MessageBox.Show("No hay datos para exportar.", "Exportar CSV");
            return;
        }

        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Archivo CSV (*.csv)|*.csv",
            FileName = $"Estadisticas_Habitaciones_{DateTime.Now:yyyyMMdd}",
            DefaultExt = ".csv"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Cabecera (Ajusta los nombres según necesites)
                csvContent.AppendLine("Habitación;Categoría;Total Reservas;Ingresos Totales");

                // Filas
                foreach (var item in RoomStatsList)
                {
                    csvContent.AppendLine($"{item.Name};{item.Type};{item.TotalBookings};{item.TotalRevenue}");
                }

                // Guardar con codificación UTF-8 para que Excel lea bien las tildes
                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show("Archivo exportado con éxito.", "Exportar CSV", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    //CONTROLES DEL MANTENIMIENTO 

    //Guardar tiempo de limpieza
    private async void SaveCT(object? parameter)
    {
        await SaveCleaningTime();
    }

    private RelayCommand saveCtCommand;
    public ICommand SaveCtCommand => saveCtCommand ??= new RelayCommand(SaveCT);

    private async Task SaveCleaningTime()
    {
        if (CurrentRoom == null) return;
        if (CurrentRoom.CleaningTime <= 0)
        {
            MessageBox.Show("El tiempo de limpieza debe ser mayor a 0 horas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var result = await _roomService.EditCleaningTime(CurrentRoom.RoomId, CurrentRoom.CleaningTime);
        if (!result.Success)
        {
            MessageBox.Show(result.Error?.Message ?? "Error actualizando tiempo de limpieza");
            return;
        }
        MessageBox.Show("Tiempo de limpieza actualizado");
        await LoadRooms();
    }

    //Poner en mantenimiento a una habitacion en un tiempo determinado
    public List<string> MaintenanceReasons { get; } = new()
    {
       "Limpieza", "Reparacion", "Mantenimiento", "Otro"
    };


    private int _maintenanceHour = 0;
    public int MaintenanceHour
    {
        get => _maintenanceHour;
        set { _maintenanceHour = value; OnPropertyChanged(nameof(MaintenanceHour)); }
    }

    private string _customReason = "";
    public string CustomReason
    {
        get => _customReason;
        set { _customReason = value; OnPropertyChanged(nameof(CustomReason)); }
    }

    private RelayCommand? _setMaintenanceCommand;
    public ICommand SetMaintenanceCommand => _setMaintenanceCommand ??= new RelayCommand(async _ => await ExecuteSetMaintenance());

    private string _dateText = DateTime.Now.ToString("dd/MM/yyyy");
    public string DateText
    {
        get => _dateText;
        set
        {
            var normalized = value ?? string.Empty;
            
            if (normalized == _dateText) return;
            _dateText = normalized;
            OnPropertyChanged(nameof(DateText));
        }
    }

    private string _selectReason;
    public string SelectReason
    {
        get => _selectReason;
        set
        {
            _selectReason = value;
            OnPropertyChanged(nameof(SelectReason));
            _ = LoadRooms();
        }
    }

    private async void SetMaint(object? parameter)
    {
        await ExecuteSetMaintenance();
    }

    private RelayCommand setMaintCommand;
    public ICommand SetMaintCommand => setMaintCommand ??= new RelayCommand(SetMaint);

    // Método que se encarga de validar los datos y llamar al servicio para poner la habitación en mantenimiento
    private async Task ExecuteSetMaintenance()
    {
        if (CurrentRoom == null || string.IsNullOrEmpty(CurrentRoom.RoomId))
        {
            MessageBox.Show("No hay ninguna habitación seleccionada", "Aviso");
            return;
        }
        
        string selectedDate = DateText;
        if (string.IsNullOrEmpty(selectedDate))
        {
            MessageBox.Show("Por favor, selecciona una fecha", "Error");
            return;
        }

        if (DateTime.TryParseExact(selectedDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            selectedDate = parsedDate.ToString("yyyy/MM/dd");
        }

        int selectedHour = MaintenanceHour;

        string finalReason = "";
        if (SelectReason == "Otro")
        {
            finalReason = CustomReason;
        }
        else
        {
            finalReason = SelectReason;
        }

        if (string.IsNullOrEmpty(finalReason))
        {
            MessageBox.Show("Por favor, introduce un motivo", "Error");
            return;
        }

        var result = await _roomService.SetMaintenance(CurrentRoom.RoomId, selectedDate, selectedHour, finalReason);

        // 6. Evaluamos respuesta
        if (result.Success)
        {
            MessageBox.Show($"La habitación estará hasta {selectedDate} en Mantenimiento por {finalReason}.", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationViewModel.Instance.BackCommand.Execute(null);
        }
        else
        {
            MessageBox.Show(result.Error?.Message ?? "El servidor rechazó la solicitud de mantenimiento.", "Error de API");
        }
    }

    //Cancelar mantenimiento
    
    private RelayCommand? _quitMaintenanceCommand;
    public ICommand QuitMaintenanceCommand => _quitMaintenanceCommand ??= new RelayCommand(async param => await ExecuteCancelMaintenance(param));

    private async Task ExecuteCancelMaintenance(object param)
    {
        if (CurrentRoom == null || string.IsNullOrEmpty(CurrentRoom.RoomId))
        {
            MessageBox.Show("No hay ninguna habitación seleccionada.", "Error");
            return;
        }

        var confirmacion = MessageBox.Show(
            $"¿Estás seguro de que deseas quitar la habitación {CurrentRoom.Name} - {CurrentRoom.Type} de mantenimiento?", "Confirmar acción",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );

        if (confirmacion == MessageBoxResult.Yes)
        {
            var result = await _roomService.CancelMaintenance(CurrentRoom);
            if (result.Success)
            {
                MessageBox.Show("El mantenimiento se ha cancelado con éxito.","Mantenimiento Cancelado");
            }
            else
            {
                MessageBox.Show(
                    result.Error?.Message ?? "Error del Servidor",
                    "Error de API"
                );
            }
        }

    }
    
    [Newtonsoft.Json.JsonIgnore]
    public object FullImageUrl
    {
        get
        {
            // 1. Si no hay habitación o el texto está vacío -> Logo por defecto
            if (CurrentRoom == null || string.IsNullOrWhiteSpace(CurrentRoom.Image))
            {
                return "pack://application:,,,/Resources/logo.png";
            }

            // 2. Si contiene ':' (ej: C:\Users...) significa que el usuario acaba de seleccionar una foto local
            if (CurrentRoom.Image.Contains(":"))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(CurrentRoom.Image);
                    bitmap.EndInit();
                    return bitmap; // Retorna la vista previa local
                }
                catch
                {
                    return "pack://application:,,,/Resources/logo.png";
                }
            }

            // 3. PARA TODO LO DEMÁS (Rutas que vienen de la API de Node/Mongo):
            // Path.GetFileName extrae SOLO "123.png" ignorando los "/src/rooms/uploads/" molestos
            string nombreArchivoLimpio = Path.GetFileName(CurrentRoom.Image);

            // Forzamos la URL exacta que sí te carga en Google Chrome
            string urlFinalWeb = $"http://localhost:3000/uploads/{nombreArchivoLimpio}";

            return urlFinalWeb;
        }
    }


    //
}
