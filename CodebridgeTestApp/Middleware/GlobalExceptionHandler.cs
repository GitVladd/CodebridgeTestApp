using CodebridgeTestApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CodebridgeTestApp.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = CreateProblemDetails(exception);

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private ProblemDetails CreateProblemDetails(Exception exception)
        {
            var problemDetails = new ProblemDetails();

            switch (exception)
            {

                case EntityAlreadyExistsException enfex:
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Title = "Conflict";
                    problemDetails.Detail = enfex.Message;
                    break;

                case ArgumentNullException anex:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Argument Null";
                    problemDetails.Detail = anex.Message;
                    break;

                case ArgumentException argex:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Detail = argex.Message;
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Server Error";
                    problemDetails.Detail = "An unexpected error occurred.";
                    break;
            }

            return problemDetails;
        }
    }
}
