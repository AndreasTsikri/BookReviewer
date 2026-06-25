using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace BookReviewer.Middlewares;
public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _lgr;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> lgr)
    {
        this._next = next;
        this._lgr = lgr;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            _next(httpContext);
        }
        catch(Exception ex)
        {
             _lgr.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(httpContext, ex);
            throw;
        }
    }

    public async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
         httpContext.Response.ContentType = "application/json";

        //await httpContext.Response.WriteAsync(new ErrorDetails()
        //{
         //   StatusCode = httpContext.Response.StatusCode,
        //    Message = "Internal Server Error."
        //}.ToString());
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Status = (int)StatusCodes.Status500InternalServerError,
            Instance = httpContext.Request.Path,
            Detail = $"Internal server error occured, traceId : { Guid.NewGuid()}"

        };
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

}

public static class CustomExceptionMiddlewareExtensions
{    public static IApplicationBuilder UseRequestCulture(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionMiddleware>();
    }
}

