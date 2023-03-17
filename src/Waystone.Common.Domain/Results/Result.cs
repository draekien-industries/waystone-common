namespace Waystone.Common.Domain.Results;

/// <summary>
/// A result is a container that has either a success state or a failure state.
/// It should be used in place of throwing exceptions as control flow. Return a Result
/// instead and handle the failure states appropriately.
/// </summary>
[PublicAPI]
public class Result
{
    /// <summary>
    /// Initializes a new <see cref="Result" />.
    /// </summary>
    /// <param name="success">Is the result a success or failure.</param>
    /// <param name="error">An <see cref="Error" /> that must be provided when initializing a fail result.</param>
    /// <exception cref="InvalidResultException">Error cannot be provided in a success result.</exception>
    protected Result(bool success, Error? error = default) : this(
        success,
        error is null ? Array.Empty<Error>() : new[] { error })
    { }

    /// <summary>
    /// Initializes a new <see cref="Result" />.
    /// </summary>
    /// <param name="success">Is the result a success or failure.</param>
    /// <param name="errors">The collection of errors associated with the result.</param>
    /// <exception cref="InvalidResultException">Errors cannot be provided in a success result.</exception>
    protected Result(bool success, IEnumerable<Error> errors)
    {
        Error[] errorsArray = errors.ToArray();

        ValidateResult(success, errorsArray);

        Succeeded = success;
        Errors = errorsArray;
    }

    /// <summary>
    /// A flag indicating whether the result is in it's success state.
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// A shortcut for checking if the result is in it's failed state.
    /// </summary>
    public bool Failed => !Succeeded;

    /// <summary>
    /// The collection of errors associated with this result.
    /// </summary>
    public Error[] Errors { get; }

    /// <summary>
    /// A shortcut for converting the collection of errors into an error message.
    /// </summary>
    public string Error => string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Message}").ToList());

    /// <summary>
    /// Creates a result in it's success state with no internal value.
    /// </summary>
    /// <returns>A result in it's success state.</returns>
    public static Result Success()
    {
        return new Result(true);
    }

    /// <summary>
    /// Creates a result in it's success state with the specified value.
    /// </summary>
    /// <param name="value">The value to store in the result object.</param>
    /// <typeparam name="TValue">The type of the value object.</typeparam>
    /// <returns>A result in it's success state.</returns>
    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(true, value);
    }

    /// <inheritdoc cref="Success{TValue}" />
    public static Result<TValue> Create<TValue>(TValue value)
    {
        return Success(value);
    }

    /// <summary>
    /// Creates a result in it's failed state.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exception">An optional <see cref="Exception" /> that caused the error.</param>
    /// <returns>A result in it's failed state.</returns>
    public static Result Fail(string code, string message, Exception? exception = default)
    {
        return new Result(false, new Error(code, message, exception));
    }

    /// <summary>
    /// Creates a result in it's failed state which contains a single error.
    /// </summary>
    /// <param name="error">The <see cref="Error" />.</param>
    /// <returns>A result in it's failed state.</returns>
    public static Result Fail(Error error)
    {
        return new Result(false, error);
    }

    /// <summary>
    /// Creates a result in it's failed state which contains multiple errors.
    /// </summary>
    /// <param name="errors">The collection of <see cref="Error" />s.</param>
    /// <returns>A result in it's failed state.</returns>
    public static Result Fail(IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }

    /// <summary>
    /// Creates a result in it's failed state.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exception">An optional <see cref="Exception" />.</param>
    /// <typeparam name="TValue">The type of the value that would be stored in a success result.</typeparam>
    /// <returns>A result in it's failed state.</returns>
    public static Result<TValue> Fail<TValue>(string code, string message, Exception? exception = default)
    {
        return new Result<TValue>(false, default, new Error(code, message, exception));
    }

    /// <summary>
    /// Creates a result in it's failed state which contains a single error.
    /// </summary>
    /// <param name="error">The <see cref="Error" />.</param>
    /// <typeparam name="TValue">The type of the value that would be stored in a success result.</typeparam>
    /// <returns>A result in it's failed state.</returns>
    public static Result<TValue> Fail<TValue>(Error error)
    {
        return new Result<TValue>(false, default, error);
    }

    /// <summary>
    /// Creates a result in it's failed state which contains multiple errors.
    /// </summary>
    /// <param name="errors">The collection of <see cref="Error" />s.</param>
    /// <typeparam name="TValue">The type of the value that would be stored in a success result.</typeparam>
    /// <returns>A result in it's failed state.</returns>
    public static Result<TValue> Fail<TValue>(IEnumerable<Error> errors)
    {
        return new Result<TValue>(false, default, errors);
    }

    /// <summary>
    /// Implicitly converts an error into a <see cref="Result" /> in it's failed state.
    /// </summary>
    /// <param name="error">The error to store inside the created result.</param>
    /// <returns>A result containing the error.</returns>
    public static implicit operator Result(Error error)
    {
        return Fail(error);
    }

    /// <summary>
    /// Implicitly converts an array of errors into a <see cref="Result" /> in it's failed state.
    /// </summary>
    /// <param name="errors">The errors to store inside the created result.</param>
    /// <returns>A result containing the errors.</returns>
    public static implicit operator Result(Error[] errors)
    {
        return Fail(errors);
    }

    private static void ValidateResult(bool success, IEnumerable<Error> errors)
    {
        switch (success)
        {
            case true when errors.Any():
                throw new InvalidResultException("Cannot assign errors when creating a successful result.");
            case false when !errors.Any():
                throw new InvalidResultException("Cannot create a failed result when there are no errors.");
        }
    }
}

/// <summary>
/// A result which contains a value of type TValue.
/// </summary>
/// <typeparam name="TValue">The type of the value associated with a successful result.</typeparam>
[PublicAPI]
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <inheritdoc />
    protected internal Result(bool success, TValue? value = default, Error? error = default) : base(success, error)
    {
        _value = value;
    }

    /// <inheritdoc />
    protected internal Result(bool success, TValue? value, IEnumerable<Error> errors) : base(success, errors)
    {
        _value = value;
    }

    /// <summary>
    /// The value of the current successful result.
    /// </summary>
    /// <exception cref="InvalidResultException">The value cannot be accessed in the current result's state.</exception>
    public TValue Value => GetValue();

    /// <summary>
    /// Implicitly converts a value of type TValue into a <see cref="Result" /> in it's success state.
    /// </summary>
    /// <param name="value">The value to store inside the created result.</param>
    /// <returns>A result containing a value of type TValue.</returns>
    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    /// <summary>
    /// Implicitly converts an error into a <see cref="Result" /> in it's failed state.
    /// </summary>
    /// <param name="error">The error to store inside the created result.</param>
    /// <returns>A result containing the error.</returns>
    public static implicit operator Result<TValue>(Error error)
    {
        return Fail<TValue>(error);
    }

    /// <summary>
    /// Implicitly converts an array of errors into a <see cref="Result" /> in it's failed state.
    /// </summary>
    /// <param name="errors">The errors to store inside the created result.</param>
    /// <returns>A result containing the errors.</returns>
    public static implicit operator Result<TValue>(Error[] errors)
    {
        return Fail<TValue>(errors);
    }

    private TValue GetValue()
    {
        if (Failed)
        {
            throw new InvalidResultException("Cannot access the value of a failed result.");
        }

        if (_value == null)
        {
            throw new InvalidResultException("The value of the result was not initialized.");
        }

        return _value;
    }
}
