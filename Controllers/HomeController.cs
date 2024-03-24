using MathGame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OnlineUsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MathGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<OnlineUsersHub> _onlineUsersHubContext;

        public HomeController(ILogger<HomeController> logger, IHubContext<OnlineUsersHub> onlineUsersHubContext)
        {
            _logger = logger;
            _onlineUsersHubContext = onlineUsersHubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult JoinGame(Player player)
        {
            // Process the model data received from the form
            // Example: Save data to a database, perform business logic, etc.

            HttpContext.Session.SetString("PlayerName", player.PlayerName);
            HttpContext.Session.SetString("RoomName", "FirstRoomName");

            return RedirectToAction("Index", "Game");
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
            //TODO : remove comment
            // Alternatively, you can use the hub context to call methods on the hub
            //await _onlineUsersHubContext.Clients.All.SendAsync("UpdateOnlineUsers", onlineUsersCount);

            return Ok(onlineUsersCount);
        }
    }
}
