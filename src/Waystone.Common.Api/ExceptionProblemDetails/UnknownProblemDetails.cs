namespace Waystone.Common.Api.ExceptionProblemDetails;

using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Internal server error problem details.</summary>
public class UnknownProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public UnknownProblemDetails(Exception ex) : base(StatusCodes.Status500InternalServerError)
    {
        Detail = ex.Message;
    }
}
