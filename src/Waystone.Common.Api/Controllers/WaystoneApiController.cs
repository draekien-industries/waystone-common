namespace Waystone.Common.Api.Controllers;

using System.Net;
using System.Net.Mime;
using Application.Contracts.Pagination;
using ConfigurationOptions;
using Domain.Contracts.Results;
using ExceptionProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

/// <summary>
/// The base controller that should be used for all API controllers. Provides access to Mediator and utility
/// methods for generating paginated results.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(UnknownProblemDetails), StatusCodes.Status500InternalServerError)]
public abstract class WaystoneApiController : ControllerBase
{
    private IConfiguration? _configuration;
    private IMediator? _mediator;

    /// <summary>
    /// Provides access to the API's configuration without dependency injection.
    /// </summary>
    protected IConfiguration Configuration =>
        _configuration ??= HttpContext.RequestServices.GetRequiredService<IConfiguration>();

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

    protected IActionResult HandleResult<T>(Result<T> result, Func<T, IActionResult> successFactory)
    {
        if (result.Succeeded)
        {
            return successFactory(result.Value);
        }

        return CreateProblem(result);
    }

    protected IActionResult HandleResult(
        Result result)
    {
        if (result.Succeeded)
        {
            return NoContent();
        }

        return CreateProblem(result);
    }

    private IActionResult CreateProblem(Result result)
    {
        CorrelationIdHeaderOptions correlationIdHeaderOptions = Configuration
                                                               .GetSection(CorrelationIdHeaderOptions.SectionName)
                                                               .Get<CorrelationIdHeaderOptions>()
                                                             ?? new CorrelationIdHeaderOptions();

        string headerName = correlationIdHeaderOptions.HeaderName;

        HttpContext.Request.Headers.TryGetValue(headerName, out StringValues correlationIdHeader);

        if (result.Errors.All(error => error is not HttpError))
        {
            return Problem(
                result.Error,
                correlationIdHeader,
                StatusCodes.Status500InternalServerError,
                HttpStatusCode.InternalServerError.ToString());
        }

        Error error = result.Errors.First(e => e is HttpError);
        var httpError = (HttpError)error;

        return Problem(
            result.Error,
            correlationIdHeader,
            (int)httpError.HttpStatusCode,
            httpError.HttpStatusCode.ToString());
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
