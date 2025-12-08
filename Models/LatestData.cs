//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

//public class LatestDataM
//{
//    [BsonId]
//    [BsonRepresentation(BsonType.ObjectId)] // ✅ Tells Mongo to convert ObjectId to string
//    public string Id { get; set; }

//    public long Time { get; set; } // Epoch time in milliseconds
//    public List<DataItem> Data { get; set; }
//}
