using MongoDB.Bson;

namespace DesafioAnotaAi.Models;

public class Category
{
    public ObjectId Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public ObjectId IdOwner { get; set; }
}
