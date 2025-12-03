using AssetRegistry.Models;
using AssetRegistry.Models.Location;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AssetRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMongoCollection<Location> _collection;

        public LocationController(IMongoClient client)
        {
            var database = client.GetDatabase("Test_DB_Iot_FE");
            _collection = database.GetCollection<Location>("LocationData");
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            try
            {
                var projection = Builders<Location>.Projection
                    .Include(x => x.Id)
                    .Include(x => x.Address);

                var result = await _collection.Find(_ => true)
                    .Project(projection)
                    .ToListAsync();

                var formattedResult = result.Select(doc =>
                {
                    string? id = null;
                    string? name = null;

                    if (doc.TryGetValue("_id", out var idValue) && idValue.IsObjectId)
                        id = idValue.AsObjectId.ToString();

                    if (doc.TryGetValue("Address", out var nameValue) && nameValue.IsString)
                        name = nameValue.AsString;

                    return new
                    {
                        id,
                        name
                    };
                });

                return Ok(new HttpResponse<IEnumerable<object>>
                {
                    Success = true,
                    Data = formattedResult
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving data.", error = ex.Message });
            }
        }
    }
}
