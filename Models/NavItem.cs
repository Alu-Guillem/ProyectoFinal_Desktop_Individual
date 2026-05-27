using PereMaria.GestorHotel.Commands;

namespace PereMaria.GestorHotel.Models;

public class NavItem
{
    public required string Label { get; init; }
    public required RelayCommand Command { get; init; }
    public required string ViewName { get; init; }
}
