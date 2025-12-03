using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text.Json;
using AssetRegistry.Models;

namespace AssetRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonDataController : ControllerBase
    {
        private readonly IMongoCollection<Json> _collection;

        public JsonDataController(IMongoClient client)
        {
            var database = client.GetDatabase("Test_DB_Iot_FE");
            _collection = database.GetCollection<Json>("JsonData");
        }




        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            try
            {
                var projection = Builders<Json>.Projection
                    .Include(x => x.Id)
                    .Include(x => x.Name);

                var result = await _collection.Find(_ => true)
                    .Project(projection)
                    .ToListAsync();

                var formattedResult = result.Select(doc =>
                {
                    string? id = null;
                    string? name = null;

                    if (doc.TryGetValue("_id", out var idValue) && idValue.IsObjectId)
                        id = idValue.AsObjectId.ToString();

                    if (doc.TryGetValue("Name", out var nameValue) && nameValue.IsString)
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

        [HttpGet("{id}")]
        public async Task<ActionResult<HttpResponse<object>>> GetById(string id)
        {
            try
            {
                var data = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

                if (data == null)
                {
                    return NotFound(new HttpResponse<object>
                    {
                        Success = false,
                        Message = $"No data found with id: {id}"
                    });
                }

                var responsePayload = new
                {
                    id = data.Id,
                    name = data.Name,
                    data = SafeDeserialize(data.Data)
                };

                return Ok(new HttpResponse<object>
                {
                    Success = true,
                    Data = responsePayload
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new HttpResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the data. " + ex.Message
                });
            }
        }


        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] JsonCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid data",
                    errors = ModelState
                });
            }

            try
            {
                var jsonData = new Json
                {
                    Data = dto.JsonData,
                    Name = dto.Name
                };

                await _collection.InsertOneAsync(jsonData);

                return Ok(new
                {
                    success = true,
                    message = "Dashboard created successfully.",
                    data = new { generatedId = jsonData.Id }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while inserting data.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Json updated)
        {
            if (string.IsNullOrWhiteSpace(updated.Data))
            {
                return BadRequest(new { message = "Data field cannot be empty." });
            }

            try
            {
                updated.Id = id;
                var result = await _collection.ReplaceOneAsync(x => x.Id == id, updated);

                if (result.MatchedCount == 0)
                {
                    return NotFound(new { message = $"No data found with id: {id}" });
                }

                return Ok(new { message = "Data updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating data.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _collection.DeleteOneAsync(x => x.Id == id);

                if (result.DeletedCount == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No data found with id: {id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Dashboard deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while deleting data.",
                    error = ex.Message
                });
            }
        }


        private object SafeDeserialize(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<object>(json);
            }
            catch
            {
                return "Invalid JSON";
            }
        }
    }
}
