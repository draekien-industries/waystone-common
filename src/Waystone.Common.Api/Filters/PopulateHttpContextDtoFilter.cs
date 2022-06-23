namespace Waystone.Common.Api.Filters;

using Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// A filter that populates the <see cref="HttpContextDto" />.
/// </summary>
internal class PopulateHttpContextDtoFilter : IAsyncActionFilter
{
    /// <inheritdoc />
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var dto = context.HttpContext.RequestServices.GetService<HttpContextDto>();

        if (dto == null) return next();

        dto.ControllerName = context.Controller.ToString();
        dto.ActionName = context.ActionDescriptor.DisplayName;
        dto.RouteData = context.RouteData.Values.ToDictionary(data => data.Key, data => data.Value);

        return next();
    }
}
