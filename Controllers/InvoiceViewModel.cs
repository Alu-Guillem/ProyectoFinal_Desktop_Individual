using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;
using PereMaria.GestorHotel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PereMaria.GestorHotel.Controllers;
public class InvoiceViewModel : BaseViewModel
{

    private static InvoiceViewModel? _instance;
    public static InvoiceViewModel Instance => _instance ??= new InvoiceViewModel();

    private UserModel _selectedUser;
    public UserModel SelectedUser
    {
        get => _selectedUser;
        set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
    }


    public ObservableCollection<InvoiceUsersModel> InvoicesList { get; set; } = new();


    // Mostrar todas las facturas de un usuario
    public async Task GetInvoices(UserModel user)
    {
        if (user == null) return;
        SelectedUser = user;

        InvoicesList.Clear();

        string userIdToQuery = user.UserId ?? "";
        var result = await InvoiceService.Instance.GetInvoice(userIdToQuery);

        if (result.Success && result.Data != null)
        {
            foreach (var invoice in result.Data)
            {
                InvoicesList.Add(invoice);
            }
        }
        else
        {
            MessageBox.Show(result.Error?.Message ?? "Error al recuperar el historial de facturación del servidor.");
        }
    }


    public  async void MostrarPDF(InvoiceUsersModel invoice)
    {
        if (invoice == null || string.IsNullOrEmpty(invoice.BookingId))
        {
            MessageBox.Show("No se puede generar la factura de una reserva inexistente o sin ID.", "Aviso");
            return;
        }

        try
        {
            string? pdfFilePath = await BookingsService.Instance.GetInvoice(invoice.BookingId);

            if (!string.IsNullOrEmpty(pdfFilePath) && File.Exists(pdfFilePath))
            {
                // Abre el PDF automáticamente usando el visor predeterminado del sistema
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = pdfFilePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("El archivo de la factura no se pudo encontrar en el disco local.", "Error");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error al procesar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    //
}

