// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Diagnostics;
using System.Reflection;
using AspNetCore.Http;
using AspNetCore.Http.Extensions;
using AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Hosting;
using Logging;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using Options;
using Waystone.Common.Api.ConfigurationOptions;
using Waystone.Common.Api.ExceptionProblemDetails;
using Waystone.Common.Api.Filters;
using Waystone.Common.Api.Logging;
using Waystone.Common.Api.SwaggerDocument;
using Waystone.Common.Domain.Contracts.Exceptions;
using ZymLabs.NSwag.FluentValidation;

/// <summary>Extensions for configuring the Waystone Common API dependency injection.</summary>
public static class WaystoneApiBuilderExtensions
{
    /// <summary>Accept the default configuration for the Waystone Common API.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <param name="apiName">The name of the api.</param>
    /// <param name="apiVersion">The version of the api.</param>
    /// <param name="apiDescription">The description of the api.</param>
    public static void AcceptDefaults(
        this IWaystoneApiServiceBuilder serviceBuilder,
        string apiName,
        string apiVersion,
        string apiDescription = "")
    {
        serviceBuilder.AddHttpContextDtoMiddleware();
        serviceBuilder.AddControllers()
                      .AddRoutingConventions()
                      .AddProblemDetailMaps(
                           options => ConfigureDefaultProblemDetailMaps(options, serviceBuilder.Environment))
                      .AddSwaggerDocumentation(apiName, apiVersion, apiDescription)
                      .ConfigureInvalidModelStateResponse()
                      .BindCorrelationIdHeaderOptions();
    }

    /// <summary>Adds the services required for the http context logging middleware.</summary>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder AddHttpContextDtoMiddleware(this IWaystoneApiServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddScoped<HttpContextDto>();

        return serviceBuilder;
    }

    /// <summary>Configures the problem details response for an invalid model state.</summary>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public static IWaystoneApiServiceBuilder ConfigureInvalidModelStateResponse(
        this IWaystoneApiServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.Configure<ApiBehaviorOptions>(
            options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "https://httpstatuses.io/400",
                        Title = "One or more validation errors occurred.",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "See the errors property for more information.",
                        Instance = context.HttpContext.Request.GetDisplayUrl(),
                    };

                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                    return new BadRequestObjectResult(problemDetails);
                };
            });

        return serviceBuilder;
    }

    /// <summary>Adds the recommended routing conventions for the Waystone Common API.</summary>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder AddRoutingConventions(this IWaystoneApiServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddRouting(
            options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

        return serviceBuilder;
    }

    /// <summary>Adds the controller configuration for the Waystone Common API.</summary>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder AddControllers(this IWaystoneApiServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddControllers(ConfigureMvcOptions)
                      .AddFluentValidation(ConfigureFluentValidation)
                      .AddProblemDetailsConventions()
                      .AddNewtonsoftJson(ConfigureMvcNewtonsoftJson);

        serviceBuilder.Services.AddHttpContextAccessor();
        serviceBuilder.Services.AddHealthChecks();

        return serviceBuilder;
    }

    /// <summary>Adds the problem detail maps for your api.</summary>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <param name="options">The action that configures the <see cref="ProblemDetailsOptions" />.</param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder AddProblemDetailMaps(
        this IWaystoneApiServiceBuilder serviceBuilder,
        Action<ProblemDetailsOptions> options)
    {
        serviceBuilder.Services.AddProblemDetails(options);

        return serviceBuilder;
    }

    /// <summary>Adds swagger documentation to the api.</summary>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <param name="title">The title of your api.</param>
    /// <param name="version">The version of your api.</param>
    /// <param name="description">The description of your api.</param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder AddSwaggerDocumentation(
        this IWaystoneApiServiceBuilder serviceBuilder,
        string title,
        string version,
        string description = "")
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        AssemblyName? assemblyName = entryAssembly?.GetName();

        string? apiName = title;
        string? apiVersion = version;
        string apiDescription = description;

        if (assemblyName != null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                apiName = assemblyName.Name;
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                apiVersion = assemblyName.Version?.ToString();
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                apiDescription = $"{apiName} {apiVersion}";
            }
        }

        serviceBuilder.Services.AddEndpointsApiExplorer();
        serviceBuilder.Services.AddScoped(
            provider =>
            {
                var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
            });

        serviceBuilder.Services.AddSwaggerDocument(
            (options, serviceProvider) =>
            {
                IServiceProvider scopedServiceProvider = serviceProvider.CreateScope().ServiceProvider;

                options.Title = apiName;
                options.Version = apiVersion;
                options.Description = apiDescription;
                options.SchemaType = SchemaType.OpenApi3;

                options.UseXmlDocumentation = true;
                options.ResolveExternalXmlDocumentation = true;

                options.GenerateEnumMappingDescription = true;
                options.GenerateExamples = true;

                var headerOptions = serviceProvider.GetRequiredService<IOptions<CorrelationIdHeaderOptions>>();
                string headerName = headerOptions.Value.HeaderName;

                options.OperationProcessors.Add(new AddCorrelationIdHeaderParameter(headerName));

                var fluentValidationSchemaProcessor =
                    scopedServiceProvider.GetService<FluentValidationSchemaProcessor>();
                options.SchemaProcessors.Add(fluentValidationSchemaProcessor);
            });

        return serviceBuilder;
    }

    /// <summary>Binds the <see cref="CorrelationIdHeaderOptions" /> from the api appsettings file.</summary>
    /// <param name="serviceBuilder">The <see cref="IWaystoneApiServiceBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder BindCorrelationIdHeaderOptions(
        this IWaystoneApiServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddOptions<CorrelationIdHeaderOptions>()
                      .Bind(serviceBuilder.Configuration.GetSection(CorrelationIdHeaderOptions.SectionName));

        return serviceBuilder;
    }

    private static void ConfigureDefaultProblemDetailMaps(ProblemDetailsOptions options, IHostEnvironment environment)
    {
        options.IncludeExceptionDetails = (_, _) => !environment.IsProduction();
        options.OnBeforeWriteDetails = (context, details) => details.Instance = context.Request.GetDisplayUrl();
        options.GetTraceId = GetTraceId;

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

        string GetTraceId(HttpContext context)
        {
            return string.IsNullOrWhiteSpace(context.TraceIdentifier)
                ? Activity.Current?.Id ?? string.Empty
                : context.TraceIdentifier;
        }
    }

    private static void ConfigureFluentValidation(
        FluentValidationMvcConfiguration options)
    {
        options.AutomaticValidationEnabled = false;
    }

    private static void ConfigureMvcOptions(MvcOptions options)
    {
        options.ReturnHttpNotAcceptable = true;
        options.RespectBrowserAcceptHeader = true;
        options.SuppressAsyncSuffixInActionNames = true;
        options.Filters.Add<PopulateHttpContextDtoFilter>();
    }

    private static void ConfigureMvcNewtonsoftJson(MvcNewtonsoftJsonOptions options)
    {
        options.UseCamelCasing(true);
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    }
}
