using System.Windows.Input;

namespace PereMaria.GestorHotel.Commands;

/// <summary>
/// Implementacion simple de ICommand que delega la logica en delegados.
/// </summary>
public class RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand
{
    private readonly Action<object?> _execute = execute;
    private readonly Predicate<object?>? _canExecute = canExecute;

    /// <inheritdoc />
    public bool CanExecute(object? parameter) =>
        _canExecute?.Invoke(parameter) ?? true;


    /// <inheritdoc />
    public void Execute(object? parameter) =>
        _execute(parameter);


    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}