namespace ReportProject.Api.Middleware;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


public class LogMiddleware//- תיעוד כל הבקשות והתגובות
{//מודפס בשורת פקודה 
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        System.Console.WriteLine($"------------------Request: {context.Request.Method} {context.Request.Path}--------------");
        await _next(context);
        System.Console.WriteLine($"------------------Resonse: {context.Response.StatusCode}-------------------------");
    }
}
