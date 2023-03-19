namespace Waystone.Sample.Domain.Prices;

using System.Net;
using System.Text;
using Common.Domain.Results;

/// <summary>
/// Errors that occur when interacting with the <see cref="Price" /> entity.
/// </summary>
public static class PriceErrors
{
    /// <summary>
    /// A value that was used to update the <see cref="Price" /> was not in the allowable range.
    /// </summary>
    /// <param name="minimum">The minimum allowed value.</param>
    /// <param name="maximum">The maximum allowed value.</param>
    /// <param name="inclusive">Whether the minimum and maximum values are inclusive.</param>
    /// <param name="exception">An optional exception that describes the error in more detail.</param>
    /// <returns>The error instance.</returns>
    public static HttpError OutOfRange(
        decimal minimum,
        decimal? maximum = default,
        bool inclusive = false,
        Exception? exception = default)
    {
        const string code = "Price_OutOfRange";
        string message = ConstructMessage();

        return new HttpError(HttpStatusCode.UnprocessableEntity, code, message, exception);

        string ConstructMessage()
        {
            StringBuilder sb = new("The provided value ");

            if (maximum is not default(decimal))
            {
                sb.Append($"is outside the allowable range of {minimum} - {maximum}");
                sb.Append(inclusive ? " inclusive." : ".");

                return sb.ToString();
            }

            sb.Append("must be greater than ");

            if (inclusive)
            {
                sb.Append("or equal to ");
            }

            sb.Append($"{minimum}.");

            return sb.ToString();
        }
    }
}
