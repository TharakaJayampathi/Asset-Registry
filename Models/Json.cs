using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Json
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("data")] // Explicitly map to lowercase field name
    public string Data { get; set; }

    public string? Name { get; set; }
}
