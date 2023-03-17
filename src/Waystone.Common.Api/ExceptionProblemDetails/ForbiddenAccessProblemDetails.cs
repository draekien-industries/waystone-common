namespace Waystone.Common.Api.ExceptionProblemDetails;

using Domain.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Forbidden access problem details.</summary>
public class ForbiddenAccessProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public ForbiddenAccessProblemDetails(ForbiddenAccessException ex) : base(StatusCodes.Status403Forbidden)
    {
        Detail = ex.Message;
    }
}
