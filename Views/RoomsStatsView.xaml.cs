using PereMaria.GestorHotel.Controllers;
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

namespace PereMaria.GestorHotel.Views
{
    /// <summary>
    /// Lógica de interacción para RoomsStatsView.xaml
    /// </summary>
    public partial class RoomsStatsView : UserControl
    {
        public RoomsStatsView()
        {
            InitializeComponent();
            this.Loaded += async (s, e) =>
            {
                await RoomsViewModel.Instance.LoadRoomOccupancy();
            };
        }
    }
}
