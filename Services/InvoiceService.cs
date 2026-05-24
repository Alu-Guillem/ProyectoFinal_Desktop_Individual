using PereMaria.GestorHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PereMaria.GestorHotel.Services;

class InvoiceService
{
    public static InvoiceService? _instance;
    public static InvoiceService Instance => _instance ??= new InvoiceService();
    private readonly ApiService _api = ApiService.Instance;

    public async Task<ApiResult<List<InvoiceUsersModel>>> GetInvoice(string userId)
    {
        return await ApiService.Instance.Get<List<InvoiceUsersModel>>($"invoices?userId={Uri.EscapeDataString(userId)}");
    }

}
