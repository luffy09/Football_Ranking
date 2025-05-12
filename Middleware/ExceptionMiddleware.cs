using System.Net;
using System.Text.Json;

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

	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await _next(httpContext);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unhandled exception has occurred.");
			await HandleExceptionAsync(httpContext, ex);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var response = new
		{
			StatusCode = context.Response.StatusCode,
			Message = _env.IsDevelopment() ? exception.Message : "Internal Server Error.",
			StackTrace = _env.IsDevelopment() ? exception.StackTrace : null
		};

		return context.Response.WriteAsync(JsonSerializer.Serialize(response));
	}
}