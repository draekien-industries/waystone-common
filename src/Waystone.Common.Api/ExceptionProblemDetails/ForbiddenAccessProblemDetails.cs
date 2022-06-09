namespace Waystone.Common.Api.ExceptionProblemDetails;

using Application.Contracts.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Problem details for a forbidden request.</summary>
public class ForbiddenAccessProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public ForbiddenAccessProblemDetails(ForbiddenAccessException ex) : base(StatusCodes.Status403Forbidden)
    {
        Detail = ex.Message;
    }
}
