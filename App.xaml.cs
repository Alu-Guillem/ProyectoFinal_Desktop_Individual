using System.Configuration;
using System.Data;
using System.Collections.Specialized;
using System.Windows;

namespace PereMaria.GestorHotel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // TODO: Initialize login window if not authenticated
        MainWindow = new MainWindow();
        MainWindow.Show();
        base.OnStartup(e);
    }
}