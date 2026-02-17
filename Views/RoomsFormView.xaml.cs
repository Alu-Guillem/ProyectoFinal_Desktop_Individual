using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System.Windows.Controls;

namespace PereMaria.GestorHotel.Views;

public partial class RoomsFormView : UserControl
{
    public RoomsFormView()
    {
        InitializeComponent();
        DataContext = RoomsViewModel.Instance;
    }

}