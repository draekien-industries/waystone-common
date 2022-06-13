namespace Waystone.Common.Api.ExceptionProblemDetails;

using Application.Contracts.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Unauthorized problem details.</summary>
public class UnauthorizedProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public UnauthorizedProblemDetails(UnauthorizedException ex) : base(StatusCodes.Status401Unauthorized)
    {
        Detail = ex.Message;
    }
}
