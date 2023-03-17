namespace Waystone.Common.Domain.Results;

/// <summary>
/// Extensions for using the result objects
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Executes a function which returns a result containing no value against the current instance of
    /// <see cref="Result{TValue}" />
    /// </summary>
    /// <param name="result">The current instance of <see cref="Result{TValue}" /></param>
    /// <param name="func">The function that will be executed against the current result instance's value</param>
    /// <typeparam name="TIn">The current result value type</typeparam>
    /// <returns>The <see cref="Result" /> of the function</returns>
    public static Result Bind<TIn>(this Result<TIn> result, Func<TIn, Result> func)
    {
        if (result.Failed)
        {
            return result;
        }

        return func(result.Value);
    }

    /// <summary>
    /// Executes a function which returns a result containing no value against the current instance of
    /// <see cref="Result{TValue}" />
    /// </summary>
    /// <param name="result">The current instance of <see cref="Result{TValue}" /></param>
    /// <param name="func">The function that will be executed against the current result instance's value</param>
    /// <typeparam name="TIn">The current result value type</typeparam>
    /// <typeparam name="TOut">The function result value type</typeparam>
    /// <returns>The <see cref="Result" /> of the function</returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        if (result.Failed)
        {
            return Result.Fail<TOut>(result.Errors);
        }

        return func(result.Value);
    }

    /// <summary>
    /// Executes an asynchronous function which returns a result containing a value against the current instance of
    /// <see cref="Result{TValue}" />
    /// </summary>
    /// <param name="result">The current instance of <see cref="Result{TValue}" /></param>
    /// <param name="func">The function that will be executed against the current result instance's value</param>
    /// <typeparam name="TIn">The current result value type</typeparam>
    /// <returns>The <see cref="Result{TValue}" /> of the function</returns>
    public static async Task<Result> BindAsync<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
    {
        if (result.Failed)
        {
            return result;
        }

        return await func(result.Value);
    }

    /// <summary>
    /// Executes an asynchronous function which returns a result containing a value against the current instance of
    /// <see cref="Result{TValue}" />
    /// </summary>
    /// <param name="result">The current instance of <see cref="Result{TValue}" /></param>
    /// <param name="func">The function that will be executed against the current result instance's value</param>
    /// <typeparam name="TIn">The current result value type</typeparam>
    /// <typeparam name="TOut">The function result value type</typeparam>
    /// <returns>The <see cref="Result{TValue}" /> of the function</returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func)
    {
        if (result.Failed)
        {
            return Result.Fail<TOut>(result.Errors);
        }

        return await func(result.Value);
    }

    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<IReadOnlyCollection<Error>, TOut> onFailure)
    {
        if (result.Succeeded)
        {
            return onSuccess();
        }

        return onFailure(result.Errors);
    }

    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<IReadOnlyCollection<Error>, TOut> onFailure)
    {
        if (result.Succeeded)
        {
            return onSuccess(result.Value);
        }

        return onFailure(result.Errors);
    }

    /// <summary>
    /// Executes a function depending on the outcome of the current <see cref="Result" /> task
    /// </summary>
    /// <param name="resultTask">A task, which when completed will return a <see cref="Result" /></param>
    /// <param name="onSuccess">The delegate which will be invoked upon a successful result</param>
    /// <param name="onFailure">The delegate which will be invoked upon a failed result</param>
    /// <typeparam name="TOut">The success function return type</typeparam>
    /// <returns>The success function return value</returns>
    public static async Task<TOut> MatchAsync<TOut>(
        this Task<Result> resultTask,
        Func<TOut> onSuccess,
        Func<IReadOnlyCollection<Error>, TOut> onFailure)
    {
        Result result = await resultTask;

        if (result.Succeeded)
        {
            return onSuccess();
        }

        return onFailure(result.Errors);
    }

    /// <summary>
    /// Executes a function depending on the outcome of the current <see cref="Result{TValue}" /> task
    /// </summary>
    /// <param name="resultTask">A task, which when completed will return a <see cref="Result{TValue}" /></param>
    /// <param name="onSuccess">The delegate which will be invoked upon a successful result</param>
    /// <param name="onFailure">The delegate which will be invoked upon a failed result</param>
    /// <typeparam name="TIn">The result value type</typeparam>
    /// <typeparam name="TOut">The success function return type</typeparam>
    /// <returns>THe success function return value</returns>
    public static async Task<TOut> MatchAsync<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> onSuccess,
        Func<IReadOnlyCollection<Error>, TOut> onFailure)
    {
        Result<TIn> result = await resultTask;

        if (result.Succeeded)
        {
            return onSuccess(result.Value);
        }

        return onFailure(result.Errors);
    }
}
