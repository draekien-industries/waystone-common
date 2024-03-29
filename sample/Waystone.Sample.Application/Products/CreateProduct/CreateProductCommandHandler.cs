﻿namespace Waystone.Sample.Application.Products.CreateProduct;

using System.Net;
using Common.Application.Contracts.Mediator;
using Common.Domain.Results;
using Domain.Prices;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
{
    private readonly IRepository _repository;

    public CreateProductCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Result<Price> createPriceResult = Price.Create(
            request.AmountExcludingTax,
            request.TaxPercentage,
            request.DiscountPercentage.GetValueOrDefault(0));

        if (createPriceResult.Failed)
        {
            return createPriceResult.Errors;
        }

        Result<Product> createProductResult = Product.CreateTransient(
            request.Name,
            createPriceResult.Value,
            request.Description ?? string.Empty);

        if (createProductResult.Failed)
        {
            return createProductResult.Errors;
        }

        Product product = createProductResult.Value;

        bool conflict = await _repository.Products.AnyAsync(existing => product.Equals(existing), cancellationToken);

        if (conflict)
        {
            return new HttpError(
                HttpStatusCode.Conflict,
                "Products_Add_Conflict",
                "A product with the same properties already exists.");
        }

        try
        {
            await _repository.Products.AddAsync(product, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return new Error("Products_AddFailed", ex.Message, ex);
        }

        ProductDto result = ProductDto.FromProduct(product);

        return result;
    }
}
