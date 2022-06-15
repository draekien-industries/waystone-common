namespace Waystone.Common.Api.Controllers;

using System.Net.Mime;
using Application.Contracts.Pagination;
using ExceptionProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The base controller that should be used for all API controllers. Provides access to Mediator and utility
/// methods for generating paginated results.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(NotFoundProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(UnknownProblemDetails), StatusCodes.Status500InternalServerError)]
public class WaystoneApiController : ControllerBase
{
    private IMediator? _mediator;

    /// <summary>Provides access to an instance of <see cref="IMediator" /> without dependency injection.</summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>Creates the links for a paginated response.</summary>
    /// <param name="actionName">The action providing the pagination.</param>
    /// <param name="request">The <see cref="PaginatedRequest{T}" /> used in the original request.</param>
    /// <param name="response">The <see cref="PaginatedResponse{T}" /> that will be returned as the response.</param>
    /// <typeparam name="T">The type of the object that is being paginated.</typeparam>
    /// <returns>Links to the current, next, and previous page.</returns>
    protected Links CreatePaginationLinks<T>(
        string actionName,
        PaginatedRequest<T> request,
        PaginatedResponse<T> response)
    {
        if (actionName.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
        {
            actionName = actionName[..^"Async".Length];
        }

        string? self = Url.Action(actionName, request);
        string? next = GetNextOrDefault(actionName, request, response);
        string? previous = GetPreviousOrDefault(actionName, request);

        return BuildLinks(self, next, previous);
    }

    private string? GetNextOrDefault<T>(string actionName, PaginatedRequest<T> request, PaginatedResponse<T> response)
    {
        string? next = default;

        if (request.Cursor + request.Limit >= response.Total) return next;

        request.Cursor += request.Limit;

        next = Url.Action(actionName, request);

        request.Cursor -= request.Limit;

        return next;
    }

    private string? GetPreviousOrDefault<T>(string actionName, PaginatedRequest<T> request)
    {
        string? previous = default;

        if (request.Cursor <= 0) return previous;

        if (request.Cursor <= request.Limit)
        {
            request.Cursor = 0;
        }
        else
        {
            request.Cursor -= request.Limit;
        }

        previous = Url.Action(actionName, request);

        return previous;
    }

    private static Links BuildLinks(string? self, string? next, string? previous)
    {
        Links links = new();

        if (!string.IsNullOrWhiteSpace(self))
        {
            links.Self = new Uri(self, UriKind.Relative);
        }

        if (!string.IsNullOrWhiteSpace(next))
        {
            links.Next = new Uri(next, UriKind.Relative);
        }

        if (!string.IsNullOrWhiteSpace(previous))
        {
            links.Previous = new Uri(previous, UriKind.Relative);
        }

        return links;
    }
}
