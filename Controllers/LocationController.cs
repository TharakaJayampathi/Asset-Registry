//using AssetRegistry.Models.Location;
//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Driver;

//namespace AssetRegistry.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LocationController : ControllerBase
//    {
//        private readonly IMongoCollection<Location> _collection;

//        public LocationController(IMongoClient client)
//        {
//            var database = client.GetDatabase("Test_DB_Iot_FE");
//            _collection = database.GetCollection<Location>("LocationData");
//        }
//    }
//}
