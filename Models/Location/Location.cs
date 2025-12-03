using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AssetRegistry.Models.Location
{
    public class Location
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string? LocationId { get; set; }
        public string? Address { get; set; }
        public int CompanyId { get; set; }
        public int DivisionId { get; set; }
        public bool Status { get; set; }
    }
}
