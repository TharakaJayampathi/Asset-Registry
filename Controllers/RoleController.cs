//using AssetRegistry.Models.Role;
//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Driver;

//namespace AssetRegistry.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RoleController : ControllerBase
//    {
//        private readonly IMongoCollection<Role> _collection;

//        public RoleController(IMongoClient client)
//        {
//            var database = client.GetDatabase("Test_DB_Iot_FE");
//            _collection = database.GetCollection<Role>("RoleData");
//        }
//    }
//}
