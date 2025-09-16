using System;
using System.Text.Json;
using DrzewaAPI.Dtos.Auth;
using DrzewaAPI.Middleware.Exceptions;
using Microsoft.Data.SqlClient;

namespace DrzewaAPI.Middleware;

public class GlobalExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<GlobalExceptionMiddleware> _logger;
	private readonly IWebHostEnvironment _environment;

	public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment environment)
	{
		_next = next;
		_logger = logger;
		_environment = environment;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var response = new ErrorResponseDto();

		// Handling specific exceptions
		if (exception is BusinessException businessException)
		{
			_logger.LogWarning(exception, "Błąd biznesowy: {ErrorCode}", businessException.ErrorCode);

			response.Error = businessException.Message;
			response.Code = businessException.ErrorCode;
			context.Response.StatusCode = businessException.StatusCode;
		}
		// Handling standard .NET exceptions
		else
		{
			_logger.LogError(exception, "Nieoczekiwany błąd systemu");

			switch (exception)
			{
				case UnauthorizedAccessException:
					response.Error = "Brak uprawnień do wykonania tej operacji";
					response.Code = "UNAUTHORIZED";
					context.Response.StatusCode = 401;
					break;

				case ArgumentException ex:
					response.Error = $"Nieprawidłowe dane: {ex.ParamName}";
					response.Code = "INVALID_ARGUMENT";
					context.Response.StatusCode = 400;
					break;

				case InvalidOperationException:
					response.Error = "Operacja nie może być wykonana w tym momencie";
					response.Code = "INVALID_OPERATION";
					context.Response.StatusCode = 409;
					break;

				case TimeoutException:
					response.Error = "Operacja przekroczyła limit czasu";
					response.Code = "TIMEOUT";
					context.Response.StatusCode = 408;
					break;

				case SqlException ex when ex.Number == -2: // Timeout
					response.Error = "Zapytanie do bazy danych przekroczyło limit czasu";
					response.Code = "DATABASE_TIMEOUT";
					context.Response.StatusCode = 408;
					break;

				case SqlException ex when ex.Number == 2: // Connection timeout
					response.Error = "Nie można połączyć się z bazą danych";
					response.Code = "DATABASE_CONNECTION_ERROR";
					context.Response.StatusCode = 503;
					break;

				default:
					response.Error = "Wystąpił nieoczekiwany błąd serwera";
					response.Code = "INTERNAL_SERVER_ERROR";
					context.Response.StatusCode = 500;
					break;
			}
		}

		// In development environment add error details
		if (_environment.IsDevelopment())
		{
			response.Details = exception.Message;
			response.InnerException = exception.InnerException;
		}

		context.Response.ContentType = "application/json";

		var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = _environment.IsDevelopment()
		});

		await context.Response.WriteAsync(jsonResponse);
	}
}
