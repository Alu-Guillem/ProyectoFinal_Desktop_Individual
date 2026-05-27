using Microsoft.Win32;
using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PereMaria.GestorHotel.Views;

public partial class RoomsFormView : UserControl
{
    public RoomsFormView()
    {
        InitializeComponent();
        DataContext = RoomsViewModel.Instance;
    }

   

    private static readonly Regex _regexInt = new Regex("[^0-9]+");
    private void Price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = _regexInt.IsMatch(e.Text) && !e.Text.Equals(".");
    }

    private void Int_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = _regexInt.IsMatch(e.Text);

    }
    private static readonly Regex _regexString = new Regex("[^a-zA-Z]+");

    private void String_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = _regexString.IsMatch(e.Text);
    }

    private void Maintenance_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationViewModel.Instance.NavigateTo<RoomMaintenanceView>();
    }

    private void SelectImage_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "Imagenes|*.jpg;*.png;*.jpeg";

        if (dialog.ShowDialog() == true)
        {
            if (this.DataContext is RoomsViewModel vm)
            {
                // 1. Guardamos la ruta física para la subida posterior
                vm.SelectedImagePath = dialog.FileName;

                // 2. Asignamos la ruta local a la habitación
                vm.CurrentRoom.Image = dialog.FileName;

                // 3. ¡EL TRUCO TRIPLE PARA FORZAR A WPF!:
                // Forzamos la actualización de datos a nivel de código
                vm.OnPropertyChanged(nameof(vm.CurrentRoom));
                vm.OnPropertyChanged(nameof(vm.FullImageUrl));

                // Forzamos al control Image a actualizar su Binding de forma manual y obligatoria
                var bindingExpression = ImgPreview.GetBindingExpression(Image.SourceProperty);
                bindingExpression?.UpdateTarget();
            }
        }
    }

}