namespace Waystone.Common.Api.DependencyInjection;

using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using Application.Contracts.Exceptions;
using ExceptionProblemDetails;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;

/// <summary>
///
/// </summary>
public static class WaystoneApiBuilderExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="apiName"></param>
    /// <param name="apiVersion"></param>
    /// <param name="apiDescription"></param>
    public static void AddDefaults(this IWaystoneApiBuilder builder, string apiName, string apiVersion, string apiDescription = "")
    {
        builder.AddControllers();
        builder.AddProblemDetailMaps(options => ConfigureDefaultProblemDetailMaps(options, builder.Environment));

        builder.AddSwaggerDocumentation(apiName, apiVersion, apiDescription);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IWaystoneApiBuilder AddControllers(this IWaystoneApiBuilder builder)
    {
        builder.Services.AddControllers(ConfigureMvcOptions)
               .AddProblemDetailsConventions()
               .AddNewtonsoftJson(ConfigureMvcNewtonsoftJson);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks();

        return builder;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IWaystoneApiBuilder AddProblemDetailMaps(
        this IWaystoneApiBuilder builder,
        Action<ProblemDetailsOptions> options)
    {
        builder.Services.AddProblemDetails(options);

        return builder;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="title"></param>
    /// <param name="version"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static IWaystoneApiBuilder AddSwaggerDocumentation(
        this IWaystoneApiBuilder builder,
        string title,
        string version,
        string description = "")
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocument(
            options =>
            {
                options.Title = title;
                options.Version = version;
                options.Description = description;
                options.UseXmlDocumentation = true;
            });

        return builder;
    }

    private static void ConfigureDefaultProblemDetailMaps(ProblemDetailsOptions options, IHostEnvironment environment)
    {
        options.OnBeforeWriteDetails = (context, details) =>
            details.Instance = Activity.Current?.Id ?? context.TraceIdentifier;

        options.IncludeExceptionDetails = (_, _) => !environment.IsProduction();

        options.Map<NotFoundException>(ex => new NotFoundProblemDetails(ex));
        options.Map<ValidationException>(ex => new BadRequestProblemDetails(ex));
        options.Map<ArgumentOutOfRangeException>(ex => new BadRequestProblemDetails(ex));
        options.Map<ArgumentNullException>(ex => new BadRequestProblemDetails(ex));
        options.Map<ArgumentException>(ex => new BadRequestProblemDetails(ex));
        options.Map<UnauthorizedException>(ex => new UnauthorizedProblemDetails(ex));
        options.Map<ForbiddenAccessException>(ex => new ForbiddenAccessProblemDetails(ex));

        options.Map<AggregateException>(
            ex =>
            {
                return ex.InnerException switch
                {
                    null => new UnknownProblemDetails(ex),
                    NotFoundException notFound => new NotFoundProblemDetails(notFound),
                    ValidationException validation => new BadRequestProblemDetails(validation),
                    ArgumentOutOfRangeException outOfRange => new BadRequestProblemDetails(outOfRange),
                    ArgumentNullException nullException => new BadRequestProblemDetails(nullException),
                    ArgumentException argumentException => new BadRequestProblemDetails(argumentException),
                    UnauthorizedException unauthorized => new UnauthorizedProblemDetails(unauthorized),
                    ForbiddenAccessException forbidden => new ForbiddenAccessProblemDetails(forbidden),
                    var _ => new UnknownProblemDetails(ex),
                };
            });

        options.Map<Exception>(ex => new UnknownProblemDetails(ex));
    }

    private static void ConfigureMvcOptions(MvcOptions options)
    {
        options.ReturnHttpNotAcceptable = true;
        options.RespectBrowserAcceptHeader = true;
        options.SuppressAsyncSuffixInActionNames = true;
    }

    private static void ConfigureMvcNewtonsoftJson(MvcNewtonsoftJsonOptions options)
    {
        options.UseCamelCasing(true);
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    }
}
