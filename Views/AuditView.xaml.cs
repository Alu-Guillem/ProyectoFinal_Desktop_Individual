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
    /// Lógica de interacción para AuditView.xaml
    /// </summary>
    public partial class AuditView : UserControl
    {
        public AuditView()
        {
            InitializeComponent();
        }

        private void HistoryRow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Border border && border.DataContext is PereMaria.GestorHotel.Models.AuditModel auditoriaSeleccionada)
            {
                PereMaria.GestorHotel.Controllers.AuditViewModel.Instance.SelectedAudit = auditoriaSeleccionada;
            }
        }

    }
}
