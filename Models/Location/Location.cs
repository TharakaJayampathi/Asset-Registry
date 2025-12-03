using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AssetRegistry.Models.Location
{
    public class Location
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string LocationId { get; set; }
        public string Address { get; set; }
        public string CompanyId { get; set; }
        public string DivisionId { get; set; }
        public bool Status { get; set; }
    }
}
