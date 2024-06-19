//using FirstApiTry.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace FirstApiTry.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GuidesController : ControllerBase
//    {
//        [HttpGet]
//        public IEnumerable<Guide> get()
//        {
//            Guide[] sts = GuidesDBMock.Guides.ToArray();
//            return sts;
//        }

//        [HttpGet("{id}")]
//        public Guide get(int id)
//        {
//            return GuidesDBMock.Guides.SingleOrDefault(gui => gui.ID == id);
//        }


//        [HttpPost]

//        public int Post(Guide guide)
//        {
//            GuidesDBMock.Guides.Add(guide);
//            return guide.ID;
//        }
//        [HttpPut("{id}")]

//        public string Put(int id, Guide guide)
//        {
//            Guide gui = GuidesDBMock.Guides.SingleOrDefault(gui => gui.ID == id);
//            gui.Name = guide.Name;
//            gui.Number = guide.Number;
//            gui.Languages = guide.Languages;
//            gui.HasCar = guide.HasCar;
//            gui.Nationality = guide.Nationality;
//            return "done:)";
//        }


//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            Guide gui = GuidesDBMock.Guides.SingleOrDefault(gui => gui.ID == id);
//            GuidesDBMock.Guides.Remove(gui);
//            var v = new { Result = "Deleted Successfully" };
//            var j = new JsonResult(v);
//            return j;
//        }




//    }
//}
    