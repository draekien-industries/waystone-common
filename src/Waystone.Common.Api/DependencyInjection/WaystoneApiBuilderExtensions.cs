namespace Waystone.Common.Api.DependencyInjection;

using System.Diagnostics;
using System.Reflection;
using ConfigurationOptions;
using Domain.Contracts.Exceptions;
using ExceptionProblemDetails;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using SwaggerDocument;
using ZymLabs.NSwag.FluentValidation;

/// <summary>Extensions for configuring the Waystone Common API dependency injection.</summary>
public static class WaystoneApiBuilderExtensions
{
    /// <summary>Accept the default configuration for the Waystone Common API.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <param name="apiName">The name of the api.</param>
    /// <param name="apiVersion">The version of the api.</param>
    /// <param name="apiDescription">The description of the api.</param>
    public static void AcceptDefaults(
        this IWaystoneApiBuilder builder,
        string apiName,
        string apiVersion,
        string apiDescription = "")
    {
        builder.AddControllers()
               .AddRoutingConventions()
               .AddProblemDetailMaps(options => ConfigureDefaultProblemDetailMaps(options, builder.Environment))
               .AddSwaggerDocumentation(apiName, apiVersion, apiDescription)
               .ConfigureInvalidModelStateResponse()
               .BindCorrelationIdHeaderOptions();
    }

    /// <summary>Configures the problem details response for an invalid model state.</summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IWaystoneApiBuilder ConfigureInvalidModelStateResponse(this IWaystoneApiBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(
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

        return builder;
    }

    /// <summary>Adds the recommended routing conventions for the Waystone Common API.</summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder AddRoutingConventions(this IWaystoneApiBuilder builder)
    {
        builder.Services.AddRouting(
            options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

        return builder;
    }

    /// <summary>Adds the controller configuration for the Waystone Common API.</summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder AddControllers(this IWaystoneApiBuilder builder)
    {
        builder.Services.AddControllers(ConfigureMvcOptions)
               .AddFluentValidation(ConfigureFluentValidation)
               .AddProblemDetailsConventions()
               .AddNewtonsoftJson(ConfigureMvcNewtonsoftJson);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks();

        return builder;
    }

    /// <summary>Adds the problem detail maps for your api.</summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <param name="options">The action that configures the <see cref="ProblemDetailsOptions" />.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder AddProblemDetailMaps(
        this IWaystoneApiBuilder builder,
        Action<ProblemDetailsOptions> options)
    {
        builder.Services.AddProblemDetails(options);

        return builder;
    }

    /// <summary>Adds swagger documentation to the api.</summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <param name="title">The title of your api.</param>
    /// <param name="version">The version of your api.</param>
    /// <param name="description">The description of your api.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder AddSwaggerDocumentation(
        this IWaystoneApiBuilder builder,
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<FluentValidationSchemaProcessor>();
        builder.Services.AddSwaggerDocument(
            (options, serviceProvider) =>
            {
                IServiceProvider scopedServiceProvider = serviceProvider.CreateScope().ServiceProvider;

                options.Title = apiName;
                options.Version = apiVersion;
                options.Description = apiDescription;
                options.GenerateEnumMappingDescription = true;
                options.ResolveExternalXmlDocumentation = true;
                options.UseXmlDocumentation = true;
                options.GenerateExamples = true;

                var headerOptions = serviceProvider.GetRequiredService<IOptions<CorrelationIdHeaderOptions>>();
                string headerName = headerOptions.Value.HeaderName;

                options.OperationProcessors.Add(new AddCorrelationIdHeaderParameter(headerName));

                var fluentValidationSchemaProcessor =
                    scopedServiceProvider.GetService<FluentValidationSchemaProcessor>();
                options.SchemaProcessors.Add(fluentValidationSchemaProcessor);
            });

        return builder;
    }

    /// <summary>Binds the <see cref="CorrelationIdHeaderOptions" /> from the api appsettings file.</summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder BindCorrelationIdHeaderOptions(this IWaystoneApiBuilder builder)
    {
        builder.Services.AddOptions<CorrelationIdHeaderOptions>()
               .Bind(builder.Configuration.GetSection(CorrelationIdHeaderOptions.SectionName));

        return builder;
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
        options.AutomaticValidationEnabled = true;
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
