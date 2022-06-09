namespace Waystone.Common.Api.ExceptionProblemDetails;

using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Problem details for a bad request.</summary>
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
