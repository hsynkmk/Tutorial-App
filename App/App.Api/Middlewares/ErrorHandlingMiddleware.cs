using App.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace App.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next.Invoke(context);
		}
		catch (NotFoundException ex)
		{
			logger.LogError(ex, ex.Message);
			await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
		}
		catch (ValidationException ex)
		{
			logger.LogError(ex, ex.Message);
			var validationErrors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
			var response = new { Message = "Validation errors occurred.", Errors = validationErrors };
			await HandleExceptionAsync(context, HttpStatusCode.BadRequest, response);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, ex.Message);
			await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, object responseContent)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;

		var response = responseContent is string
			? JsonSerializer.Serialize(new { Message = responseContent })
			: JsonSerializer.Serialize(responseContent);

		await context.Response.WriteAsync(response);
	}
}
