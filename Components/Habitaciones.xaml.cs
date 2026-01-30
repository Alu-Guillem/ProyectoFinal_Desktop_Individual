using PereMaria.GestorHotel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PereMaria.GestorHotel.Components
{
    /// <summary>
    /// Interaction logic for Habitaciones.xaml
    /// </summary>
    public partial class Habitaciones : UserControl
    {
        public Habitaciones()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            Services.NavigationService.Instance.NavigateTo<RoomsFormView>();
        }
    }
}
