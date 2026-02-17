using PereMaria.GestorHotel.Controllers;
using System.Windows.Controls;

namespace PereMaria.GestorHotel.Views;

public partial class RoomsView : UserControl
{
    public RoomsView()
    {
        InitializeComponent();
        DataContext = RoomsViewModel.Instance;

    }

    private void Habitaciones_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void Create_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationViewModel.Instance.NavigateTo<RoomsFormView>();
    }
}