namespace Waystone.Common.Domain.Results;

using Contracts.Results;

/// <summary>
/// Extensions for using the result objects
/// </summary>
public static class ResultExtensions
{
    public static async Task<Result> BindFunctionAsync<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
    {
        if (result.Failed)
        {
            return result;
        }

        return await func(result.Value);
    }

    public static async Task<Result<TOut>> BindFunctionAsync<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func)
    {
        if (result.Failed)
        {
            return Result.Fail<TOut>(result.Errors);
        }

        return await func(result.Value);
    }

    public static async Task<TOut> MatchResultAsync<TOut>(
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

    public static async Task<TOut> MatchResultAsync<TIn, TOut>(
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
