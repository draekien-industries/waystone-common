namespace Waystone.Sample.Application.Products;

using System.Net;

internal static class ProductsErrors
{
    public static readonly HttpError NotFound = new(
        HttpStatusCode.NotFound,
        "Products_NotFound",
        "The product associated with the provided ID was not found.");
}
