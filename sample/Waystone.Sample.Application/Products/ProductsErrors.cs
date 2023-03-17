namespace Waystone.Sample.Application.Products;

using System.Net;
using Common.Domain.Results;

internal static class ProductsErrors
{
    public static readonly HttpError NotFound = new(
        HttpStatusCode.NotFound,
        "Products_NotFound",
        "The product associated with the provided ID was not found.");
}
