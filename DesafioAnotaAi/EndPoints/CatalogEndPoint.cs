using DesafioAnotaAi.Context;
using DesafioAnotaAi.Models.DTOs;
using MongoDB.Bson;
using System.Linq;

namespace DesafioAnotaAi.EndPoints;

public static class CatalogEndPoint
{
    public static void MapCatalogEndPoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalogs");

        group.MapGet(string.Empty, (ApiContext context) =>
        {
            return Results.Ok(
               context.Categories.ToList().Select(c => new CatalogDto(
                    new CategoryDto(c.Id.ToString(),c.Title,c.Description,c.IdOwner.ToString()),
                    [
                        ..context.Products
                        .Where(p => p.IdCategory == c.Id)
                        .Select(p => new ProductDto(p.Id.ToString(), p.Title, p.Description, p.Price, p.IdCategory.ToString(), p.IdOwner.ToString()))
                        .ToArray()
                    ]
                   )).ToArray()
            );
        });
    }
}
