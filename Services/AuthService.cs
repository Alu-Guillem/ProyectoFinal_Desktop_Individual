using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

/// <summary>
/// Servicio de autenticación para login de empleados/administradores en escritorio.
/// </summary>
public class AuthService
{
    // Singleton
    private static AuthService? _instance;
    public static AuthService Instance => _instance ??= new AuthService();

    private readonly ApiService _api = ApiService.Instance;


    /// <summary>
    /// Solicita autenticación al backend y devuelve un JWT de sesión.
    /// </summary>
    public async Task<ApiResult<LoginResponse>> Login(string email, string password)
    {
        return await _api.Post<LoginResponse>($"auth/login", new { email, password });
    }
}

/// <summary>
/// Contrato de respuesta para login exitoso.
/// </summary>
public class LoginResponse
{
    public string Token { get; set; }
}