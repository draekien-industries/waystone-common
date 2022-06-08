namespace Waystone.Common.Application.Common.Behaviours;

using FluentValidation;
using FluentValidation.Results;
using MediatR;

/// <summary>
/// MediatR pipeline behaviour that triggers registered validators.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestValidationBehaviour{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">The set of all registered validators for the request type.</param>
    public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any()) return await next();

        ValidationContext<TRequest> context = new(request);

        IEnumerable<Task<ValidationResult>> validationTasks =
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken));

        ValidationResult[] results = await Task.WhenAll(validationTasks);

        List<ValidationFailure> failures =
            results.SelectMany(result => result.Errors)
                   .Where(error => error != null)
                   .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
