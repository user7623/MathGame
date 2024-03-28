using MathGame.BAL.Interfaces;
using MathGame.Dtos;
using MathGame.MathExpressions;
using MathGame.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineUsers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace MathGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRoundsInformation _gameRoundsInformation;
        private readonly IHubContext<OnlineUsersHub> _onlineUsersHubContext;
        private bool isGeneratorActive = false;
        MathExpressionsGenerator generator = new MathExpressionsGenerator();

        public GameController(IGameRoundsInformation gameRoundsInformation, IHubContext<OnlineUsersHub> onlineUsersHubContext)
        {
            _gameRoundsInformation = gameRoundsInformation;
            _onlineUsersHubContext = onlineUsersHubContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public GameRound GetNewestRound(string roomName)
        {
            try
            {
                if (!string.IsNullOrEmpty(roomName))
                {
                    return _gameRoundsInformation.GetNewestRound(roomName);
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameController -> GetNewestRound");
                Log.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateGameRound()
        {
            var roomName = "test room";
            var lastRoundNumber = _gameRoundsInformation.GetLastRoundNumber(roomName);
            var newRound = generator.GenerateMathExpression(lastRoundNumber, roomName);//read dynamic
            await _gameRoundsInformation.SaveGameRound(newRound);
            return Ok();
        }

        [HttpGet]
        public int GetActivePlayers()
        {
            return OnlineUsersHub.GetOnlineUsersCount();
        }

        [HttpPost]
        public List<GameRound> GetQuestionsTable()
        {
            try
            {
                //TODO : get only the last five or so
                var response = new List<GameRound>();
                var roomName = "test room";//TODO read from session
                response = _gameRoundsInformation.ReadRoundsForRoom(roomName);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameController -> GetQuestionsTable");
                Log.Error(ex.ToString());
                return null;
            }
        }

        [HttpPost]
        public IActionResult SubmitAnswer([FromBody] AnswerDto answer)
        {
            if (!string.IsNullOrEmpty(answer.Answer) && !string.IsNullOrEmpty(answer.RoomName))
            {
                answer.Username = HttpContext.Session.GetString("PlayerName");//TODO : is this needed
                try
                {
                    _gameRoundsInformation.SaveAnswerForRound(answer);
                }
                catch
                {
                    return BadRequest();
                }
                return Ok();
            }

            return BadRequest();
        }
    }
}
