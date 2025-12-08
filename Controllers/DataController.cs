//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Driver;
//using AssetRegistry.Models;

//namespace AssetRegistry.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DataController : ControllerBase
//    {


//        private readonly IMongoCollection<LatestDataM> _collection;

//        public DataController(IMongoClient client)
//        {
//            var database = client.GetDatabase("Test_DB_Iot_FE");
//            _collection = database.GetCollection<LatestDataM>("RawData");
//        }




//        [HttpPost("post")]
//        public async Task<IActionResult> Post([FromBody] LatestDataM dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                // Convert current time to epoch (milliseconds)
//                long epochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

//                var document = new LatestDataM
//                {
//                    Time = epochTime,
//                    Data = dto.Data
//                };

//                await _collection.InsertOneAsync(document);

//                return Ok(new
//                {
//                    message = "Data inserted successfully.",
                   
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    message = "An error occurred while inserting data.",
//                    error = ex.Message
//                });
//            }
//        }




//        [HttpPost("LatestDataGetByName")]
//        public async Task<IActionResult> GetByName([FromBody] GetByNameRequestDto request)
//        {
//            if (string.IsNullOrWhiteSpace(request.Name))
//            {
//                return BadRequest(new { message = "Name parameter is required." });
//            }

//            try
//            {
//                // Filter: Match documents with the given name in Data and within the time range
//                var filter = Builders<LatestDataM>.Filter.And(
//                    Builders<LatestDataM>.Filter.Gte(x => x.Time, request.StartTime),
//                    Builders<LatestDataM>.Filter.Lte(x => x.Time, request.EndTime),
//                    Builders<LatestDataM>.Filter.ElemMatch(
//                        x => x.Data,
//                        dataItem => dataItem.Name == request.Name
//                    )
//                );

//                // Sort by Time descending to get the latest first
//                var result = await _collection
//                    .Find(filter)
//                    .SortByDescending(x => x.Time)
//                    .FirstOrDefaultAsync();

//                if (result == null)
//                {
//                    return NotFound(new { message = "No matching data found for the given criteria." });
//                }

//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    message = "An error occurred while retrieving data.",
//                    error = ex.Message
//                });
//            }
//        }











//    }
//}
