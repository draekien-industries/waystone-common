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
        Detail = "See the errors property for more information.";
        Title = "One or more validation errors occured.";

        IEnumerable<string> erroredPropertyNames = ex.Errors.Select(x => x.PropertyName).Distinct();

        foreach (string? propertyName in erroredPropertyNames)
        {
            Errors.Add(
                propertyName,
                ex.Errors.Where(x => x.PropertyName == propertyName).Select(x => x.ErrorMessage).ToArray());
        }
    }

    /// <inheritdoc />
    public BadRequestProblemDetails(Exception ex) : base(StatusCodes.Status400BadRequest)
    {
        Detail = ex.Message;
    }

    /// <summary>The errors that caused the bad request.</summary>
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.Ordinal);
}
