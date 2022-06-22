namespace Waystone.Common.Api.ExceptionProblemDetails;

using Domain.Contracts.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

/// <summary>Not found problem details.</summary>
public class NotFoundProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public NotFoundProblemDetails(NotFoundException ex) : base(StatusCodes.Status404NotFound)
    {
        Detail = ex.Message;
    }
}
