using DesafioAnotaAi.Context;
using DesafioAnotaAi.EndPoints;
using DesafioAnotaAi.Extensions;
using DesafioAnotaAi.Models;
using DesafioAnotaAi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using static DesafioAnotaAi.EndPoints.CategoryEndPoint;
using static DesafioAnotaAi.EndPoints.ProductEndPoint;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.InitMongoDb();
builder.Services.AddMongoDB<ApiContext>(builder.Configuration.GetConnectionString("MongoDbUri"), "desafio_anotaai");

var app = builder.Build();

app.MapProductEndPoint();
app.MapCategoryEndPoint();
app.MapCatalogEndPoint();

app.Run();

[JsonSerializable(typeof(ProductDto[]))]
[JsonSerializable(typeof(ProductCreateRequestDto[]))]
[JsonSerializable(typeof(CategoryDto[]))]
[JsonSerializable(typeof(CategoryCreateRequestDto[]))]
[JsonSerializable(typeof(CatalogDto[]))]
[JsonSerializable(typeof(ResponseBaseDto<CategoryDto>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
