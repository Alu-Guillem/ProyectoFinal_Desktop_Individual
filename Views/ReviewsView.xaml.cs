using System.Windows;
using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

namespace PereMaria.GestorHotel.Views;

/// <summary>
/// Vista de análisis y consulta de reseñas en la aplicación de escritorio.
/// </summary>
public partial class ReviewsView : UserControl
{
    /// <summary>
    /// Inicializa la vista de reseñas.
    /// </summary>
    public ReviewsView()
    {
        InitializeComponent();
        Loaded += ReviewsView_Loaded;
    }

    private async void ReviewsView_Loaded(object sender, RoutedEventArgs e)
    {
        await ReviewsViewModel.Instance.LoadReviews();
    }
}