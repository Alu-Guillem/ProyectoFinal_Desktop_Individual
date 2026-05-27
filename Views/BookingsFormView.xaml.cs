using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

namespace PereMaria.GestorHotel.Views;

/// <summary>
/// Vista de formulario de reserva con selectores asistidos de cliente y habitación.
/// </summary>
public partial class BookingsFormView : UserControl
{
    /// <summary>
    /// Inicializa el formulario de reservas.
    /// </summary>
    public BookingsFormView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Abre el popup de sugerencias de cliente al enfocar el buscador.
    /// </summary>
    private void CustomerSearchBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        SetCustomerPopupOpen(true);
    }

    /// <summary>
    /// Cierra el popup de cliente al perder foco salvo que la lista aún lo tenga.
    /// </summary>
    private void CustomerSearchBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (CustomerList.IsKeyboardFocusWithin) return;
        SetCustomerPopupOpen(false);
    }

    /// <summary>
    /// Mantiene abierto el popup de cliente mientras se escribe en el buscador.
    /// </summary>
    private void CustomerSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!CustomerSearchBox.IsKeyboardFocusWithin) return;
        SetCustomerPopupOpen(true);
    }

    /// <summary>
    /// Cierra el popup al seleccionar un cliente de la lista.
    /// </summary>
    private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SetCustomerPopupOpen(false);
    }

    /// <summary>
    /// Cierra el popup de cliente cuando ni buscador ni lista tienen foco.
    /// </summary>
    private void CustomerList_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (CustomerSearchBox.IsKeyboardFocusWithin) return;
        SetCustomerPopupOpen(false);
    }

    /// <summary>
    /// Abre el popup de sugerencias de habitación al enfocar el buscador.
    /// </summary>
    private void RoomSearchBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        SetRoomPopupOpen(true);
    }

    /// <summary>
    /// Cierra el popup de habitación al perder foco salvo que la lista aún lo tenga.
    /// </summary>
    private void RoomSearchBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (RoomList.IsKeyboardFocusWithin) return;
        SetRoomPopupOpen(false);
    }

    /// <summary>
    /// Mantiene abierto el popup de habitación mientras se filtran resultados.
    /// </summary>
    private void RoomSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!RoomSearchBox.IsKeyboardFocusWithin) return;
        SetRoomPopupOpen(true);
    }

    /// <summary>
    /// Cierra el popup tras seleccionar una habitación.
    /// </summary>
    private void RoomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SetRoomPopupOpen(false);
    }

    /// <summary>
    /// Cierra el popup de habitación cuando se pierde el foco de búsqueda/lista.
    /// </summary>
    private void RoomList_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (RoomSearchBox.IsKeyboardFocusWithin) return;
        SetRoomPopupOpen(false);
    }

    /// <summary>
    /// Actualiza el estado de visibilidad del popup de clientes en el ViewModel.
    /// </summary>
    private void SetCustomerPopupOpen(bool isOpen)
    {
        if (DataContext is not BookingsViewModel vm) return;
        vm.IsCustomerPopupOpen = isOpen && vm.IsCreating;
    }

    /// <summary>
    /// Actualiza el estado de visibilidad del popup de habitaciones en el ViewModel.
    /// </summary>
    private void SetRoomPopupOpen(bool isOpen)
    {
        if (DataContext is not BookingsViewModel vm) return;
        vm.IsRoomPopupOpen = isOpen && vm.IsCreating;
    }
}