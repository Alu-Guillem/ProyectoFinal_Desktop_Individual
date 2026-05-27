using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Windows;
using Newtonsoft.Json;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

/// <summary>
/// Cliente HTTP base para llamadas REST de la aplicación escritorio.
/// Encapsula serialización, parseo de errores y resultados tipados.
/// </summary>
public class ApiService
{
    // Singleton
    private static ApiService? _instance;
    public static ApiService Instance => _instance ??= new ApiService();

    public HttpClient HttpClient => _httpClient;

    private HttpClient _httpClient
        = new();

    private ApiService()
    {
        _httpClient.BaseAddress = new Uri(GlobalConfig.Default.ApiUri);
        _httpClient.Timeout = new TimeSpan(0, 0, 15);
    }

    /// <summary>
    /// Configura el token JWT usado en la cabecera Authorization para peticiones posteriores.
    /// </summary>
    /// <param name="token">Token Bearer emitido por backend.</param>
    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }


    /// <summary>
    /// Ejecuta una petición GET y devuelve el resultado tipado o error normalizado.
    /// </summary>
    public async Task<ApiResult<T>> Get<T>(string route) where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(route);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Data = JsonConvert.DeserializeObject<T>(content),
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResult<T>
            {
                Success = false,
                Error = TryParseError(content),
                StatusCode = response.StatusCode
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResult<T>
            {
                Success = false,
                Error = new ApiError { Message = "No se pudo conectar con el servidor" },
                StatusCode = 0
            };
        }
    }

    /// <summary>
    /// Ejecuta una petición POST serializando el body en JSON.
    /// </summary>
    public async Task<ApiResult<T>> Post<T>(string route, object? o) where T : class
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(route, o);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Data = JsonConvert.DeserializeObject<T>(content),
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResult<T>
            {
                Success = false,
                Error = TryParseError(content),
                StatusCode = response.StatusCode
            };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.HttpRequestError);
            Console.WriteLine(ex.Message);
            return new ApiResult<T>
            {
                Success = false,
                Error = new ApiError { Message = "No se pudo conectar con el servidor" },
                StatusCode = 0
            };
        }
    }

    /// <summary>
    /// Ejecuta una petición PUT serializando el body en JSON.
    /// </summary>
    public async Task<ApiResult<T>> Put<T>(string route, object? o) where T : class
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(route, o);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Data = JsonConvert.DeserializeObject<T>(content),
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResult<T>
            {
                Success = false,
                Error = TryParseError(content),
                StatusCode = response.StatusCode
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResult<T>
            {
                Success = false,
                Error = new ApiError { Message = "No se pudo conectar con el servidor" },
                StatusCode = 0
            };
        }
    }

    /// <summary>
    /// Ejecuta una petición PATCH
    /// </summary>
    public async Task<ApiResult<T>> Patch<T>(string route, object? o) where T : class
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync(route, o);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Data = JsonConvert.DeserializeObject<T>(content),
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResult<T>
            {
                Success = false,
                Error = TryParseError(content),
                StatusCode = response.StatusCode
            };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.HttpRequestError);
            Console.WriteLine(ex.Message);
            return new ApiResult<T>
            {
                Success = false,
                Error = new ApiError { Message = "No se pudo conectar con el servidor" },
                StatusCode = 0
            };
        }
    }

    /// <summary>
    /// Ejecuta una petición DELETE y devuelve el resultado tipado.
    /// </summary>
    public async Task<ApiResult<T>> Delete<T>(string route) where T : class
    {
        try
        {
            var response = await _httpClient.DeleteAsync(route);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Data = JsonConvert.DeserializeObject<T>(content),
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResult<T>
            {
                Success = false,
                Error = TryParseError(content),
                StatusCode = response.StatusCode
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResult<T>
            {
                Success = false,
                Error = new ApiError { Message = "No se pudo conectar con el servidor" },
                StatusCode = 0
            };
        }
    }


    /// <summary>
    /// Intenta deserializar el payload de error del backend al contrato de <see cref="ApiError"/>.
    /// </summary>
    private static ApiError TryParseError(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<ApiError>(json)
                   ?? new ApiError { Message = "Error desconocido" };
        }
        catch
        {
            return new ApiError { Message = "Error desconocido" };
        }
    }
}