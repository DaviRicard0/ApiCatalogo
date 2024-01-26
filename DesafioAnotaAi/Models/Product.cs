using MongoDB.Bson;

namespace DesafioAnotaAi.Models;

public class Product
{
    public ObjectId Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public ObjectId IdCategory { get; set; }
    public ObjectId IdOwner { get; set; }
}
