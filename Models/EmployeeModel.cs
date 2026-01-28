using System.ComponentModel.DataAnnotations;

namespace PereMaria.GestorHotel.Models;

public class EmployeeModel(string name, string email, string role)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Role { get; set; } = role;
}