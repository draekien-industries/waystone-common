namespace Waystone.Sample.Application.Products;

using System.Net;

public static class ProductsErrors
{
    public static HttpError NotFound = new(
        HttpStatusCode.NotFound,
        "Products_NotFound",
        "The product associated with the provided ID was not found.");
}
