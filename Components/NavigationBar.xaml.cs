using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Views;
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
        NavigationViewModel.Instance.NavigateTo<PreviewControlsView>();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void HistoryButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationViewModel.Instance.NavigateTo<AuditView>();
    }
}