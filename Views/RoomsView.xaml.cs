using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System.Windows.Controls;

namespace PereMaria.GestorHotel.Views;

public partial class RoomsView : UserControl
{
    public RoomsView()
    {
        InitializeComponent();
        DataContext = RoomsViewModel.Instance;
        Loaded += async (_, __) => await RoomsViewModel.Instance.LoadRooms();

    }

    private void Create_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        RoomsViewModel.Instance.CurrentRoom = new RoomModel();
        NavigationViewModel.Instance.NavigateTo<RoomsFormView>();
    }
}