using System.Net;


public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;
}

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            switch (ex)
            {
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            await context.Response.WriteAsync(new ErrorDetail
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
            }.ToString()!);
        }
    }
}
