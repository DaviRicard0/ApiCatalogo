using DesafioAnotaAi.Context;
using DesafioAnotaAi.Models;
using DesafioAnotaAi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace DesafioAnotaAi.EndPoints;

public static class ProductEndPoint
{
    public static void MapProductEndPoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapGet(string.Empty, (ApiContext context) =>
        {
            return Results.Ok(
                context.Products.ToList().Select(p => new ProductDto(p.Id.ToString(), p.Title, p.Description, p.Price, p.IdCategory.ToString(), p.IdOwner.ToString())).ToArray()
            );
        });
        group.MapPost(string.Empty, async ([FromBody] ProductCreateRequestDto req, ApiContext context) =>
        {
            Product product = new() {
                Title = req.Title,
                Price = req.Price,
                Description = req.Description ,
                IdCategory= ObjectId.Parse( req.IdCategory),
                IdOwner = ObjectId.Parse(req.IdOwner)
            };
            if (context.Categories.SingleOrDefault(c => c.Id == product.IdCategory) is null)
                return Results.NotFound("Notfound IdCategory");
            if (context.Owners.SingleOrDefault(o => o.Id == product.IdOwner) is null)
                return Results.NotFound("Notfound IdOwner");

            //DbContext.Products = [product, .. DbContext.Products];// (product);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return Results.Ok(
                new ProductDto(
                    product.Id.ToString(),
                    product.Title,
                    product.Description,
                    product.Price,
                    product.IdCategory.ToString(),
                    product.IdOwner.ToString()
                )
            );
        });

        group.MapPut(string.Empty, async ([FromBody] ProductDto req,ApiContext context) =>
        {
            if (context.Categories.SingleOrDefault(c => c.Id == ObjectId.Parse(req.IdCategory)) is null)
                return Results.NotFound("Notfound IdCategory");

            if (context.Owners.SingleOrDefault(o => o.Id == ObjectId.Parse(req.IdOwner)) is null)
                return Results.NotFound("Notfound IdOwner");

            var productFiltered = context.Products.SingleOrDefault(p => p.Id == ObjectId.Parse(req.Id));

            if (productFiltered is null)
                return Results.NotFound("Notfound Product");

            productFiltered.Price = req.Price;
            productFiltered.Title = req.Title;
            productFiltered.Description = req.Description;
            productFiltered.IdOwner = ObjectId.Parse(req.IdOwner);
            productFiltered.IdCategory = ObjectId.Parse(req.IdCategory);

            //DbContext.Products = [productFiltered, .. DbContext.Products.Where(p => p.Id != productFiltered.Id)];

            await context.SaveChangesAsync();

            return Results.Ok(
                new ProductDto(
                    productFiltered.Id.ToString(),
                    productFiltered.Title,
                    productFiltered.Description,
                    productFiltered.Price,
                    productFiltered.IdCategory.ToString(),
                    productFiltered.IdOwner.ToString()
                )
            );
        });

        group.MapDelete("{idProduct}", async ([FromRoute] string idProduct, ApiContext context) =>
        {

            var productFiltered = context.Products.SingleOrDefault(p => p.Id == ObjectId.Parse(idProduct));

            if (productFiltered is null)
                return Results.NotFound("Notfound Product");

            //context.Products = context.Products.Where(p => p.Id != productFiltered.Id).ToArray();
            context.Products.Remove(productFiltered);
            await context.SaveChangesAsync();

            return Results.Ok(
                new ProductDto(
                    productFiltered.Id.ToString(),
                    productFiltered.Title,
                    productFiltered.Description,
                    productFiltered.Price,
                    productFiltered.IdCategory.ToString(),
                    productFiltered.IdOwner.ToString()
                )
            );
        });
    }
}
