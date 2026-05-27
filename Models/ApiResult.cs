using System.Net;

namespace PereMaria.GestorHotel.Models;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}