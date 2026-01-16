using System.Windows.Input;

namespace PereMaria.GestorHotel.Commands;

public class RelayCommand(Action<object> execute, Predicate<object> canExecute) : ICommand
{
    private readonly Action<object> _execute = execute;
    private readonly Predicate<object>? _canExecute = canExecute;

    public RelayCommand(Action<object> execute) : this(execute, null)
    {
    }

    public bool CanExecute(object? parameter) =>
        _canExecute?.Invoke(parameter) ?? true;


    public void Execute(object? parameter) =>
        _execute(parameter);


    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}