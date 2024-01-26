using MongoDB.Bson;

namespace DesafioAnotaAi.Models;

public class Owner
{
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
}
