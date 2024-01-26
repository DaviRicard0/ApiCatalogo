using DesafioAnotaAi.Context;
using DesafioAnotaAi.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DesafioAnotaAi.Extensions;

public static class MongoDbContext
{
    public static void InitMongoDb(this WebApplicationBuilder app) {
        var connectionString = app.Configuration.GetConnectionString("MongoDbUri");
        /*if (connectionString == null)
        {
            Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
            Environment.Exit(0);
        }*/

        var mongoClient = new MongoClient(connectionString);

        var dbContextOptions =
            new DbContextOptionsBuilder<ApiContext>().UseMongoDB(mongoClient, "desafio_anotaai");
        var db = new ApiContext(dbContextOptions.Options);

        var owners = new Owner[]
        {
            new() {
                Id = ObjectId.Parse("65ad6510f0532182f805fa12"),
                Name = "Letícia"
            },
            new()
            {
                Id = ObjectId.Parse("65ad6525f0532182f805fa13"),
                Name = "Olívia"
            }
        };
        db.Owners.RemoveRange(owners);
        db.SaveChanges();
        db.Owners.AddRange(owners);
        db.SaveChanges();
        /*db.Owners.Add(new Owner() { Id = ObjectId.GenerateNewId(), Name = "Irineu" });
        db.SaveChanges();*/
    }
}
