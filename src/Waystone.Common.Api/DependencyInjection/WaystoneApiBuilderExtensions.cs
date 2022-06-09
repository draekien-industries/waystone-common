﻿namespace Waystone.Common.Api.DependencyInjection;

using System.Diagnostics;
using System.Reflection;
using Application.Contracts.Exceptions;
using ConfigurationOptions;
using ExceptionProblemDetails;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using SwaggerDocument;

/// <summary>Extensions for configuring the Waystone Common API dependency injection.</summary>
public static class WaystoneApiBuilderExtensions
{
    /// <summary>
    /// Accept the default configuration for the Waystone Common API.
    /// </summary>
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
               .AddProblemDetailMaps(options => ConfigureDefaultProblemDetailMaps(options, builder.Environment))
               .AddSwaggerDocumentation(apiName, apiVersion, apiDescription)
               .BindCorrelationIdHeaderOptions();
    }

    /// <summary>
    /// Adds the controller configuration for the Waystone Common API.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder AddControllers(this IWaystoneApiBuilder builder)
    {
        builder.Services.AddControllers(ConfigureMvcOptions)
               .AddProblemDetailsConventions()
               .AddNewtonsoftJson(ConfigureMvcNewtonsoftJson);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks();

        return builder;
    }

    /// <summary>Adds the problem detail maps for your api.</summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder"/>.</param>
    /// <param name="options">The action that configures the <see cref="ProblemDetailsOptions"/>.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder"/>.</returns>
    public static IWaystoneApiBuilder AddProblemDetailMaps(
        this IWaystoneApiBuilder builder,
        Action<ProblemDetailsOptions> options)
    {
        builder.Services.AddProblemDetails(options);

        return builder;
    }

    /// <summary>
    /// Adds swagger documentation to the api.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder"/>.</param>
    /// <param name="title">The title of your api.</param>
    /// <param name="version">The version of your api.</param>
    /// <param name="description">The description of your api.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder"/>.</returns>
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
        builder.Services.AddSwaggerDocument(
            options =>
            {
                options.Title = apiName;
                options.Version = apiVersion;
                options.Description = apiDescription;
                options.UseXmlDocumentation = true;

                IConfigurationSection? correlationIdHeaderConfigs = builder.Configuration.GetSection(CorrelationIdHeaderOptions.SectionName);
                var headerName = correlationIdHeaderConfigs?.GetValue<string>(nameof(CorrelationIdHeaderOptions.HeaderName));

                options.OperationProcessors.Add(new AddCorrelationIdHeaderParameter(headerName ?? CorrelationIdHeaderOptions.DefaultHeaderName));
            });

        return builder;
    }

    /// <summary>
    /// Binds the <see cref="CorrelationIdHeaderOptions"/> from the api appsettings file.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApiBuilder"/>.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder"/>.</returns>
    public static IWaystoneApiBuilder BindCorrelationIdHeaderOptions(this IWaystoneApiBuilder builder)
    {
        builder.Services.AddOptions<CorrelationIdHeaderOptions>()
               .Bind(builder.Configuration.GetSection(CorrelationIdHeaderOptions.SectionName));

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
