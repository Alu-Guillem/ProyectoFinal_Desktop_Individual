using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;
using Views;

namespace PereMaria.GestorHotel.Components;

public partial class NavigationBar : UserControl
{
    public NavigationBar()
    {
        InitializeComponent();
    }

    private void PaletteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationViewModel.Instance.NavigateTo<PreviewControlsView>();
    }

}