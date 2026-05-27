using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PereMaria.GestorHotel.Views;

/// <summary>
/// Lógica de interacción para InvoiceUsersList.xaml
/// </summary>
public partial class InvoiceUsersList : UserControl
{
    public InvoiceUsersList()
    {
        InitializeComponent();
    }

    private void InvoiceRow_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is InvoiceUsersModel selectedInvoice)
        {
            InvoiceViewModel.Instance.MostrarPDF(selectedInvoice); 
        }
    }


}
