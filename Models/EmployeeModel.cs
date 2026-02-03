using System.ComponentModel.DataAnnotations;

namespace PereMaria.GestorHotel.Models;

public class EmployeeModel : UserModel
{
    public EmployeeModel(string userId, string email, string role, string password, string firstName, string lastName, string photo) :
        base(userId, email, role, password, firstName, lastName, photo)
    { }
    
    public EmployeeModel() {}
}