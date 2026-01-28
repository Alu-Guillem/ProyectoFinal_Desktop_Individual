using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Windows;
using Newtonsoft.Json;
using PereMaria.GestorHotel.Commands;
using PereMaria.GestorHotel.Models;

namespace PereMaria.GestorHotel.Services;

public class ApiService
{
    // TODO IMPLEMENT SESSION SERVICE
    private string JWT =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI2OTc3ODFjMDBiZTMyOTdlZmFiNTM1MGEiLCJyb2xlIjoiYWRtaW4iLCJpYXQiOjE3Njk0NDAwMzAsImV4cCI6MTc3MDczNjAzMH0.vygSqWOAGZhzLOkXljy5jJkwXbwxzL-1QERf0xzkGo8";

    // Singleton
    private static ApiService? _instance;
    public static ApiService Instance => _instance ??= new ApiService();

    private HttpClient _httpClient = new();

    private ApiService()
    {
        _httpClient.BaseAddress = new Uri(GlobalConfig.Default.ApiUri);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT);
        _httpClient.Timeout = new TimeSpan(0, 0, 15);
    }

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
            return new ApiResult<T>
            {
                Success = false,
                Error = new ApiError { Message = "No se pudo conectar con el servidor" },
                StatusCode = 0
            };
        }
    }


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


    /*
    public async Task<string> TestConnection()
    {
        try
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            throw;
        }
    }*/
}