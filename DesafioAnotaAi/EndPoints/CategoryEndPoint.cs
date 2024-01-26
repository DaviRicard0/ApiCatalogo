using DesafioAnotaAi.Context;
using DesafioAnotaAi.Models;
using DesafioAnotaAi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace DesafioAnotaAi.EndPoints;

public static class CategoryEndPoint
{
    public static void MapCategoryEndPoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/categories");

        group.MapGet(string.Empty, (ApiContext context) =>
        {
            return Results.Ok(
                context
                .Categories
                .ToList()
                .Select(
                    cur => new CategoryDto(cur.Id.ToString(), cur.Title, cur.Description, cur.IdOwner.ToString())
                )
                .ToArray()
            );
        });

        group.MapGet("{id}", ([FromRoute] string id,ApiContext context) =>
        {
            var category = context
                .Categories
                .SingleOrDefault(c => c.Id == ObjectId.Parse(id));

            if (category is null)
                return Results.NotFound("Notfound IdOwner");

            return Results.Ok(
                new ResponseBaseDto<CategoryDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new CategoryDto(
                        category.Id.ToString(),
                        category.Title,
                        category.Description, 
                        category.IdOwner.ToString()
                    )
                }
            );
        });

        group.MapPost(string.Empty,async ([FromBody] CategoryCreateRequestDto req, ApiContext context) =>
        {
            Category category = new()
            {
                Id = ObjectId.GenerateNewId(),
                Title = req.Title,
                Description = req.Description,
                IdOwner = ObjectId.Parse(req.IdOwner)
            };

            if (context.Owners.SingleOrDefault(o => o.Id == category.IdOwner) is null)
                return Results.NotFound("Notfound IdOwner");

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Results.Ok(
                new CategoryDto(
                    category.Id.ToString(),
                    category.Title,
                    category.Description,
                    category.IdOwner.ToString()
                )
            );
        });

        group.MapPut(string.Empty, async ([FromBody] CategoryDto req, ApiContext context) =>
        {
            if (context.Owners.SingleOrDefault(o => o.Id == ObjectId.Parse(req.IdOwner)) is null)
                return Results.NotFound("Notfound IdOwner");

            var categoryFiltered = context.Categories.SingleOrDefault(p => p.Id == ObjectId.Parse(req.Id));

            if (categoryFiltered is null)
                return Results.NotFound("Notfound Category");

            categoryFiltered.Title = req.Title;
            categoryFiltered.Description = req.Description;
            categoryFiltered.IdOwner = ObjectId.Parse(req.IdOwner);

            await context.SaveChangesAsync();

            //DbContext.Categories = [categoryFiltered, .. DbContext.Categories.Where(p => p.Id != categoryFiltered.Id)];

            return Results.Ok(
                new CategoryDto(
                    categoryFiltered.Id.ToString(),
                    categoryFiltered.Title,
                    categoryFiltered.Description,
                    categoryFiltered.IdOwner.ToString()
                )
            );
        });

        group.MapDelete("{idCategory}", async ([FromRoute] string idCategory, ApiContext context) =>
        {

            var categoryFiltered = context.Categories.SingleOrDefault(p => p.Id == ObjectId.Parse(idCategory));

            if (categoryFiltered is null)
                return Results.NotFound("Notfound Product");

            if (context.Products.FirstOrDefault(p => p.IdCategory == categoryFiltered.Id) is not null)
                return Results.NotFound("The category is linked");

            context.Remove(categoryFiltered);
            await context.SaveChangesAsync();
            //DbContext.Products = DbContext.Products.Where(p => p.Id != categoryFiltered.Id).ToArray();

            return Results.Ok(
                new CategoryDto(
                    categoryFiltered.Id.ToString(),
                    categoryFiltered.Title,
                    categoryFiltered.Description,
                    categoryFiltered.IdOwner.ToString()
                )
            );
        });
    }
}
