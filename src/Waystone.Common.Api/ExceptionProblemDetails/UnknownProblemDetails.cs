namespace Waystone.Common.Api.ExceptionProblemDetails;

using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

public class UnknownProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public UnknownProblemDetails(Exception ex) : base(StatusCodes.Status500InternalServerError)
    {
        Detail = ex.Message;
    }
}
