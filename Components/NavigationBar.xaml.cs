using System.Windows.Controls;
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
        Services.NavigationService.Instance.NavigateTo<PreviewControlsView>();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}