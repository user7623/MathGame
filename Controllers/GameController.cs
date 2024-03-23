using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json.Nodes;

namespace MathGame.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            int activeSessionsCount = HttpContext.Session.Keys.Count();

            var playerName = HttpContext.Session.GetString("PlayerName");
            var roomName = HttpContext.Session.GetString("RoomName");

            return View();
        }
        [HttpPost]
        public string GetQuestionsTable()
        {
            string jsonData = @"[
                                    { ""#"": 1, ""Expression"": ""2 + 2"", ""YourAnswer"": 4, ""Result"": ""Correct"" },
                                    { ""#"": 2, ""Expression"": ""5 * 3"", ""YourAnswer"": 15, ""Result"": ""Correct"" }
                                ]";

            var res = new JsonResult(jsonData);

            var response = JsonConvert.SerializeObject(jsonData);

            //JsonResult result = new JsonResult
            //{
            //    Data = JsonConvert.DeserializeObject(jsonData)
            //};

            return response;
        }
    }
}
