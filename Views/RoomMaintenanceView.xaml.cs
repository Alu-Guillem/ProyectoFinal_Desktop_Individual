using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PereMaria.GestorHotel.Views
{
    /// <summary>
    /// Lógica de interacción para RoomMaintenanceView.xaml
    /// </summary>
    public partial class RoomMaintenanceView : UserControl
    {
        public RoomMaintenanceView()
        {
            InitializeComponent();
        }
        private void Int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return;
            }
            if (sender is TextBox textBox)
            {
                string prospectiveText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

                if (int.TryParse(prospectiveText, out int hourValue))
                {
                    if (hourValue > 24)
                    {
                        e.Handled = true; 
                    }
                }
            }
        }
    }
}
