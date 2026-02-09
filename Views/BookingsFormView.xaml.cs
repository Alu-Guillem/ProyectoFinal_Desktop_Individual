using System.Windows.Controls;
using PereMaria.GestorHotel.Controllers;

namespace PereMaria.GestorHotel.Views;

public partial class BookingsFormView : UserControl
{
    public BookingsFormView()
    {
        InitializeComponent();
    }

    private void CustomerSearchBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        SetCustomerPopupOpen(true);
    }

    private void CustomerSearchBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (CustomerList.IsKeyboardFocusWithin) return;
        SetCustomerPopupOpen(false);
    }

    private void CustomerSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!CustomerSearchBox.IsKeyboardFocusWithin) return;
        SetCustomerPopupOpen(true);
    }

    private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SetCustomerPopupOpen(false);
    }

    private void CustomerList_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (CustomerSearchBox.IsKeyboardFocusWithin) return;
        SetCustomerPopupOpen(false);
    }

    private void RoomSearchBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        SetRoomPopupOpen(true);
    }

    private void RoomSearchBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (RoomList.IsKeyboardFocusWithin) return;
        SetRoomPopupOpen(false);
    }

    private void RoomSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!RoomSearchBox.IsKeyboardFocusWithin) return;
        SetRoomPopupOpen(true);
    }

    private void RoomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SetRoomPopupOpen(false);
    }

    private void RoomList_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (RoomSearchBox.IsKeyboardFocusWithin) return;
        SetRoomPopupOpen(false);
    }

    private void SetCustomerPopupOpen(bool isOpen)
    {
        if (DataContext is not BookingsViewModel vm) return;
        vm.IsCustomerPopupOpen = isOpen && vm.IsCreating;
    }

    private void SetRoomPopupOpen(bool isOpen)
    {
        if (DataContext is not BookingsViewModel vm) return;
        vm.IsRoomPopupOpen = isOpen && vm.IsCreating;
    }
}