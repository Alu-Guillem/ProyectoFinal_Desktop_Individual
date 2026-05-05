using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
using System.Text.RegularExpressions;
using System.Windows.Controls;

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
}