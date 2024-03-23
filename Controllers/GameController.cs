using MathGame.BAL.Interfaces;
using MathGame.Dtos;
using MathGame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace MathGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRoundsInformation _gameRoundsInformation;

        public GameController(IGameRoundsInformation gameRoundsInformation)
        {
            _gameRoundsInformation = gameRoundsInformation;
        }

        public IActionResult Index()
        {
            int activeSessionsCount = HttpContext.Session.Keys.Count();

            var playerName = HttpContext.Session.GetString("PlayerName");
            var roomName = HttpContext.Session.GetString("RoomName");

            return View();
        }
        [HttpPost]
        public List<GameRound> GetQuestionsTable()
        {

            var response = new List<GameRound>();

            GameRound roundOne = new GameRound(1,"BlueRoom", 1, "2 + 2", (int)DateTime.Now.Ticks, "Filip");
            GameRound roundTwo = new GameRound(2, "BlueRoom", 2, "2 + 4", (int)DateTime.Now.Ticks, "Filip");
            response.Add(roundOne);
            response.Add(roundTwo);

            return response;
        }

        [HttpPost]
        public IActionResult SubmitAnswer([FromBody] AnswerDto answer)
        {
            var temp = answer;

            var a = answer.Answer;

            try
            {
            }
            catch
            {
                return BadRequest();
            }

            //TODO : save answer

            return Ok();
        }
    }
}
