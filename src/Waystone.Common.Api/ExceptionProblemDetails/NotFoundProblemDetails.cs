namespace Waystone.Common.Api.ExceptionProblemDetails;

using Application.Contracts.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

public class NotFoundProblemDetails : StatusCodeProblemDetails
{
    /// <inheritdoc />
    public NotFoundProblemDetails(NotFoundException exception) : base(StatusCodes.Status404NotFound)
    { }
}
