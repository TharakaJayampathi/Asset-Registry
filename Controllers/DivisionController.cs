//using AssetRegistry.Models.Division;
//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Driver;

//namespace AssetRegistry.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DivisionController : ControllerBase
//    {
//        private readonly IMongoCollection<Division> _collection;

//        public DivisionController(IMongoClient client)
//        {
//            var database = client.GetDatabase("Test_DB_Iot_FE");
//            _collection = database.GetCollection<Division>("DivisionData");
//        }
//    }
//}
