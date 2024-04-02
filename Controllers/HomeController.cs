using MathGame.Models;
using MathGame.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OnlineUsers;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MathGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly IActivePlayersService _activePlayersService;

        public HomeController(IActivePlayersService activePlayersService)
        {
            _activePlayersService = activePlayersService;
        }

        public IActionResult Index()
        {
            var userToken = HttpContext.Session.GetString("token");
            if (TempData["RoomIsFull"] != null)
            {
                var isFull = TempData["RoomIsFull"];
                ViewData["RoomIsFull"] = isFull;
            }
            if (string.IsNullOrEmpty(userToken))
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JoinGame(string roomName)
        {
            var activeInRoom = await _activePlayersService.GetActivePlayersInRoom();
            HttpContext.Session.SetString("RoomName", roomName);
            if (activeInRoom >= 4)
            {
                TempData["RoomIsFull"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Game");
            }
        } 

        public IActionResult ActiveSessionsCount()
        {
            int activeSessionsCount = HttpContext.Session.Keys.Count();
            return Content($"Active sessions: {activeSessionsCount}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetActive()
        {
            int onlineUsersCount = OnlineUsersHub.GetOnlineUsersCount();
            return Ok(onlineUsersCount);
        }
    }
}
