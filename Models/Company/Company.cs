using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AssetRegistry.Models.Company
{
    public class Company
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string? CompanyId { get; set; }
        public string? Name { get; set; }
        public bool Status { get; set; }
    }
}
