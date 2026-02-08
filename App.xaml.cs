using System.Configuration;
using System.Data;
using System.Collections.Specialized;
using System.Windows;
using PereMaria.GestorHotel.Views;

namespace PereMaria.GestorHotel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // TODO: Initialize login window if not authenticated
        MainWindow = new Login();
        MainWindow.Show();
        base.OnStartup(e);
    }
}