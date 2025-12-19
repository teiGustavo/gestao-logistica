namespace GestaoLogisticaAPI.Application.Middleware;

public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context) {
        try {
            await next(context);
        } catch (Exception ex) {
            logger.LogError(ex, "Ocorreu uma exceção!");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Ocorreu um erro no servidor.");
        }
    }
}