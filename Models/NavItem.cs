using PereMaria.GestorHotel.Commands;

namespace PereMaria.GestorHotel.Models;

public class NavItem
{
    public required string Label { get; init; }
    public required RelayCommand Command { get; init; }
    public required Action Navigate { get; init; }
}
