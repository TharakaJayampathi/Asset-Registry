using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AssetRegistry.Models.Division
{
    public class Division
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DivisionId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public bool Status { get; set; }
    }
}
