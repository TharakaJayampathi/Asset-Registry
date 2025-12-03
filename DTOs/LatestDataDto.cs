using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class LatestDataMDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public long Time { get; set; } // Epoch time in milliseconds
    public List<DataItem> Data { get; set; }
}
