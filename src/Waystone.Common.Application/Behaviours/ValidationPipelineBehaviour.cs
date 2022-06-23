namespace Waystone.Common.Application.Behaviours;

using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

internal class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        string requestType = request.GetType().Name;

        if (!_validators.Any())
        {
            _logger.LogInformation("No validators found for request of type {RequestType}", requestType);

            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        IEnumerable<Task<ValidationResult>> validationTasks = _validators.Select(
            validator => validator.ValidateAsync(context, cancellationToken));

        ValidationResult[] validationResults = await Task.WhenAll(validationTasks);

        List<ValidationFailure> failures = validationResults
                                          .SelectMany(validationResult => validationResult.Errors)
                                          .Where(validationFailure => validationFailure != null)
                                          .ToList();

        if (failures.Any())
        {
            _logger.LogInformation("Validation Failures found for request of type {RequestType}", requestType);

            throw new ValidationException(failures);
        }

        _logger.LogInformation("No validation failures found for request of type {RequestType}", requestType);

        return await next();
    }
}
