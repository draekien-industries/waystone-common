namespace Waystone.Common.Application.Extensions;

using System.Net.Mime;
using Newtonsoft.Json;

/// <summary>Extensions for <see cref="HttpContent" />.</summary>
public static class HttpContentExtensions
{
    /// <summary>Deserializes the JSON string contained in the <see cref="HttpContent" /> to an object of the specified type.</summary>
    /// <remarks>Uses Newtonsoft.Json.</remarks>
    /// <param name="content">The <see cref="HttpContent" /> containing the JSON string.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="T">The type to deserialize the JSON into.</typeparam>
    /// <exception cref="InvalidOperationException">Content Type must be application/json.</exception>
    /// <returns>The nullable object of type T.</returns>
    public static async Task<T?> DeserializeObjectAsync<T>(
        this HttpContent content,
        CancellationToken cancellationToken = default)
    {
        EnsureHttpContentIsJson();

        string json = await content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<T>(json);

        void EnsureHttpContentIsJson()
        {
            if (content.Headers.ContentType == null)
            {
                throw new InvalidOperationException("Content-Type header is not populated.");
            }

            if (content.Headers.ContentType.MediaType != MediaTypeNames.Application.Json)
            {
                throw new InvalidOperationException(
                    $"Content Media Type is not {MediaTypeNames.Application.Json}. Media Type: {content.Headers.ContentType.MediaType}.");
            }
        }
    }
}
