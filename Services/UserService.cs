using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class UserService
{
    private readonly ApiService _api = ApiService.Instance;

    public async Task<ApiResult<List<UserModel>>> GetAllUsers()
    {
        return await _api.Get<List<UserModel>>("users");
    }
    
    public async Task<ApiResult<List<EmployeeModel>>> GetAllEmployees()
    {
        return await _api.Get<List<EmployeeModel>>("users/employees");
    }
    
    public async Task<ApiResult<List<CustomerModel>>> GetAllCustomers()
    {
        return await _api.Get<List<CustomerModel>>("users/customers");
    }

    public async Task<ApiResult<List<UserModel>>> GetUser(string id)
    {
        return await _api.Get<List<UserModel>>($"users/{id}");
    }
    
    public async Task<ApiResult<CustomerModel>> CreateCustomer(CustomerModel customerModel)
    {
        return await _api.Post<CustomerModel>("users/customer", customerModel);
    }
    
    public async Task<ApiResult<EmployeeModel>> CreateEmployee(EmployeeModel employeeModel)
    {
        return await _api.Post<EmployeeModel>("users/employee", employeeModel);
    }
    
    public async Task<ApiResult<UserModel>> UpdateUser(UserModel userModel)
    {
        return await _api.Patch<UserModel>($"users/{userModel.UserId}", userModel);
    }

    public async Task<ApiResult<UserModel>> GetMe()
    {
        return await _api.Get<UserModel>("users/me");
    }
    
    public async Task<ApiResult<object>> DeleteUser(string id)
    {
        return await _api.Delete<object>($"users/{id}");
    }



}