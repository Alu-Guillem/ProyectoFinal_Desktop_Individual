using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

/// <summary>
/// Servicio de usuarios para operaciones CRUD de empleados y clientes.
/// </summary>
public class UserService
{
    private static UserService? _instance;
    public static UserService Instance => _instance ??= new UserService();

    private readonly ApiService _api = ApiService.Instance;

    public UserService() { }

    /// <summary>
    /// Recupera todos los usuarios del sistema.
    /// </summary>
    public async Task<ApiResult<List<UserModel>>> GetAllUsers()
    {
        return await _api.Get<List<UserModel>>("users");
    }

    /// <summary>
    /// Recupera únicamente usuarios con rol empleado.
    /// </summary>
    public async Task<ApiResult<List<EmployeeModel>>> GetAllEmployees()
    {
        return await _api.Get<List<EmployeeModel>>("users/employees");
    }

    /// <summary>
    /// Recupera únicamente usuarios con rol cliente.
    /// </summary>
    public async Task<ApiResult<List<CustomerModel>>> GetAllCustomers()
    {
        return await _api.Get<List<CustomerModel>>("users/customers");
    }

    /// <summary>
    /// Obtiene un usuario por identificador.
    /// </summary>
    public async Task<ApiResult<List<UserModel>>> GetUser(string id)
    {
        return await _api.Get<List<UserModel>>($"users/{id}");
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    public async Task<ApiResult<CustomerModel>> CreateCustomer(CustomerModel customerModel)
    {
        return await _api.Post<CustomerModel>("users/customer", customerModel);
    }

    /// <summary>
    /// Crea un nuevo empleado.
    /// </summary>
    public async Task<ApiResult<EmployeeModel>> CreateEmployee(EmployeeModel employeeModel)
    {
        return await _api.Post<EmployeeModel>("users/employee", employeeModel);
    }

    /// <summary>
    /// Actualiza los datos de un usuario existente.
    /// </summary>
    public async Task<ApiResult<UserModel>> UpdateUser(UserModel userModel)
    {
        return await _api.Put<UserModel>($"users/{userModel.UserId}", userModel);
    }

    /// <summary>
    /// Recupera el perfil del usuario autenticado.
    /// </summary>
    public async Task<ApiResult<UserModel>> GetMe()
    {
        return await _api.Get<UserModel>("users/me");
    }

    /// <summary>
    /// Elimina un usuario por identificador.
    /// </summary>
    public async Task<ApiResult<object>> DeleteUser(string id)
    {
        return await _api.Delete<object>($"users/{id}");
    }
}