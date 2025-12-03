using AssetRegistry.Models.User;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AssetRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _collection;

        public UserController(IMongoClient client)
        {
            var database = client.GetDatabase("Test_DB_Iot_FE");
            _collection = database.GetCollection<User>("UserData");
        }
    }
}
