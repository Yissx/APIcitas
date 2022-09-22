using AppCitas.Service.Errors;
using System.Net;
using System.Text.Json;

namespace AppCitas.Service.ExcpetionMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    public async Task InvokeAsync(HttpContext context) //Diferencia entre thread y task --> thread es la autopista y task son los vehículos que conducen sobre ella
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "pplication/json";

            var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message,
                      ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "Internal server error");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
