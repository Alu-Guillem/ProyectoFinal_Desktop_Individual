using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PereMaria.GestorHotel.Controllers;

public class AuditViewModel : BaseViewModel
{

    private ICommand _refreshCommand;
    public ICommand RefreshCommand => _refreshCommand ??= new RelayCommand(async _ => await LoadHistoryAsync());

    private static AuditViewModel? _instance;
    public static AuditViewModel Instance => _instance ??= new AuditViewModel();

    private readonly BookingsService _bookingsService = new();

    public ObservableCollection<AuditModel> BookingHistoryList { get; } = new();

    private AuditViewModel()
    {
        _ = LoadHistoryAsync();
    }

    public List<string> AvailableActions { get; } = new() { "Todos", "create", "update", "delete", "cancel", "pay", "extend" };
    public List<string> AvailableRoles{ get; } = new() { "Todos", "admin", "employee", "customer" };

    private string _selectedAction = "Todos";
    private string _selectedRole = "Todos";
    public string SelectedAction
    {
        get => _selectedAction;
        set
        {
            if (value == _selectedAction) return;
            _selectedAction = value;
            OnPropertyChanged(nameof(SelectedAction));

            _ = LoadHistoryAsync();
        }
    }

    public string SelectedRole
    {
        get => _selectedRole;
        set
        {
            if (value == _selectedRole) return;
            _selectedRole = value;
            OnPropertyChanged(nameof(SelectedRole));

            _ = LoadHistoryAsync();
        }
    }

    public async Task LoadHistoryAsync()
    {
        try
        {
            string? accionFiltrada = SelectedAction == "Todos" ? null : SelectedAction;
            string? rolFiltrado = SelectedRole == "Todos" ? null : SelectedRole;

            var registrosApi = await _bookingsService.GetBookingAudit(accionFiltrada, rolFiltrado);
            if (registrosApi == null) return;

            int intentos = 0;
            while ((RoomsViewModel.Instance.Rooms.Count == 0 ||
                    BookingsViewModel.Instance.Bookings.Count == 0 ||
                    UsersViewModel.Instance.Users.Count == 0) && intentos < 6)
            {
                await Task.Delay(500);
                intentos++;
            }

            var registrosOrdenados = registrosApi.OrderByDescending(h => h.Timestamp).ToList();

            foreach (var item in registrosOrdenados)
            {
                var usuario = UsersViewModel.Instance.Users.FirstOrDefault(u => u.UserId == item.ActorId);
                item.ActorName = usuario != null ? $"{usuario.FirstName} {usuario.LastName}" : $"ID: {item.ActorId}";

                var reserva = BookingsViewModel.Instance.Bookings.FirstOrDefault(b => b.BookingId == item.BookingId);
                var room = RoomsViewModel.Instance.Rooms.FirstOrDefault(r => r.RoomId == reserva?.RoomId);

                if (reserva != null && room != null)
                {
                    item.BookingName = $"{room.Name} - {room.Type}";
                }
                else if (reserva != null)
                {
                    item.BookingName = "Reserva encontrada (Habitación no encontrada)";
                }
                else
                {
                    item.BookingName = $"ID Reserva: {item.BookingId}";
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                BookingHistoryList.Clear();
                foreach (var registro in registrosOrdenados)
                {
                    BookingHistoryList.Add(registro);
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en AuditViewModel: {ex.Message}");
        }
    }

    private AuditModel _selectedAudit;
    public AuditModel SelectedAudit
    {
        get => _selectedAudit;
        set
        {
            if (value == _selectedAudit) return;
            _selectedAudit = value;
            OnPropertyChanged(nameof(SelectedAudit));

            // Disparamos la actualización del texto formateado en JSON
            OnPropertyChanged(nameof(PreviousStateJson));
            OnPropertyChanged(nameof(NewStateJson));
        }
    }

    // Propiedades dinámicas que serializan los objetos crudos a JSON limpio para la interfaz
    [Newtonsoft.Json.JsonIgnore]
    public string PreviousStateJson => SelectedAudit?.PreviousState != null
        ? Newtonsoft.Json.JsonConvert.SerializeObject(SelectedAudit.PreviousState, Newtonsoft.Json.Formatting.Indented)
        : "";

    [Newtonsoft.Json.JsonIgnore]
    public string NewStateJson => SelectedAudit?.NewState != null
        ? Newtonsoft.Json.JsonConvert.SerializeObject(SelectedAudit.NewState, Newtonsoft.Json.Formatting.Indented)
        : "";





    //
}
