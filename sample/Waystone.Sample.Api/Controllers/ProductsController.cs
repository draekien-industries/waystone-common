namespace Waystone.Sample.Api.Controllers;

using Application.Products;
using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetProduct;
using Application.Products.ListProducts;
using Common.Api.Controllers;
using Common.Api.ExceptionProblemDetails;
using Common.Application.Contracts.Pagination;
using Common.Domain.Results;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoints for interacting with the products resource.
/// </summary>
public class ProductsController : WaystoneApiController
{
    /// <summary>
    /// List a page of products.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A paginated response of products.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [FromQuery] ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await Result.Create(request)
                           .BindAsync(query => Mediator.Send(query, cancellationToken))
                           .MatchAsync(
                                products =>
                                {
                                    products.Links = CreatePaginationLinks("List", request, products);

                                    return Ok(products);
                                },
                                CreateProblem);
    }

    /// <summary>
    /// Gets a product using it's ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The product corresponding to the provided ID.</returns>
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        GetProductByIdQuery request = new(id);

        return await Result.Create(request)
                           .BindAsync(query => Mediator.Send(query, cancellationToken))
                           .MatchAsync(Ok, CreateProblem);
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created product.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddAsync(
        [FromBody] CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        return await Result.Create(request)
                           .BindAsync(command => Mediator.Send(command, cancellationToken))
                           .MatchAsync(
                                product => CreatedAtAction("GetById", new { id = product.Id }, product),
                                CreateProblem);
    }

    /// <summary>
    /// Deletes an existing product.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>No content.</returns>
    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        DeleteProductCommand command = new(id);

        return await Result.Create(command)
                           .BindAsync(request => Mediator.Send(request, cancellationToken))
                           .MatchAsync(NoContent, CreateProblem);
    }
}
