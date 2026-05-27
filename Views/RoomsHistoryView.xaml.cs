using PereMaria.GestorHotel.Controllers;
using PereMaria.GestorHotel.Models;
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
    /// Lógica de interacción para RoomsHistoryView.xaml
    /// </summary>
    public partial class RoomsHistoryView : UserControl
    {
        public RoomsHistoryView()
        {
            InitializeComponent();
        }

        private void RoomRow_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is RoomStatModel selectedStat)
            {
                RoomsViewModel.Instance.SelectedStatRoom = selectedStat;
                RoomsViewModel.Instance.SelectedYearOccupancy = RoomsViewModel.Instance.SelectedYear;
                NavigationViewModel.Instance.NavigateTo<RoomsStatsView>();
            }
        }

    }
}
