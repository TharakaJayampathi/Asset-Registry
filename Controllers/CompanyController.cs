using AssetRegistry.DTOs.Company;
using AssetRegistry.Models;
using AssetRegistry.Models.Company;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AssetRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMongoCollection<Company> _collection;

        public CompanyController(IMongoClient client)
        {
            var database = client.GetDatabase("Test_DB_Iot_FE");
            _collection = database.GetCollection<Company>("CompanyData");
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            try
            {
                var projection = Builders<Company>.Projection
                    .Include(x => x.Id)
                    .Include(x => x.CompanyId)
                    .Include(x => x.Name)
                    .Include(x => x.Status);

                var result = await _collection.Find(_ => true)
                    .Project(projection)
                    .ToListAsync();

                var formattedResult = result.Select(doc =>
                {
                    string? id = null;
                    string? name = null;
                    string? companyId = null;
                    bool? status = true;

                    if (doc.TryGetValue("_id", out var idValue) && idValue.IsObjectId)
                        id = idValue.AsObjectId.ToString();

                    if (doc.TryGetValue("Name", out var nameValue) && nameValue.IsString)
                        name = nameValue.AsString;

                    if (doc.TryGetValue("CompanyId", out var companyIdValue) && companyIdValue.IsString)
                        companyId = companyIdValue.AsString;

                    if (doc.TryGetValue("Status", out var statusValue) && companyIdValue.IsBoolean)
                        status = statusValue.AsBoolean;

                    return new
                    {
                        id,
                        name,
                        companyId,
                        status
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
                    companyId = data.CompanyId,
                    name = data.Name,
                    status = data.Status
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
        public async Task<IActionResult> Post([FromBody] CompanyDTO dto)
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
                var _payload = new Company
                {
                    CompanyId = dto.CompanyId,
                    Name = dto.Name,
                    Status = dto.Status
                };

                await _collection.InsertOneAsync(_payload);

                return Ok(new
                {
                    success = true,
                    message = "Company created successfully.",
                    data = new { generatedId = _payload.Id }
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
        public async Task<IActionResult> Put(string id, Company updated)
        {
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
                    message = "Company deleted successfully."
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
    }
}
