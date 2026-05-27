using System.ComponentModel;
using System.Linq;
using System.Windows;

public class BaseViewModel : INotifyPropertyChanged
{
    // ========== INotifyPropertyChanged ==========
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected MessageBoxResult ShowMessageBox(
        string message,
        string caption = "Aviso",
        MessageBoxButton buttons = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.None)
    {
        var owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                    ?? Application.Current?.MainWindow;

        return owner != null
            ? MessageBox.Show(owner, message, caption, buttons, icon)
            : MessageBox.Show(message, caption, buttons, icon);
    }
}