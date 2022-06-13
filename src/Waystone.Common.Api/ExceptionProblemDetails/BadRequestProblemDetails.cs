namespace Waystone.Common.Api.ExceptionProblemDetails;

using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Bad request problem details.</summary>
public class BadRequestProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public BadRequestProblemDetails(ValidationException ex) : base(StatusCodes.Status400BadRequest)
    {
        Detail = string.Join(", ", ex.Errors.Select(x => x.ErrorMessage));
    }

    /// <inheritdoc />
    public BadRequestProblemDetails(Exception ex) : base(StatusCodes.Status400BadRequest)
    {
        Detail = ex.Message;
    }
}
