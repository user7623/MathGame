using MathGame.BAL.Interfaces;
using MathGame.Dtos;
using MathGame.MathExpressions;
using MathGame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineUsers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Serilog;

namespace MathGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRoundsInformation _gameRoundsInformation;
        MathExpressionsGenerator generator = new MathExpressionsGenerator();

        public GameController(IGameRoundsInformation gameRoundsInformation)
        {
            _gameRoundsInformation = gameRoundsInformation;
        }

        public IActionResult Index()
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
            //var roomName = HttpContext.Session.GetString("RoomName");
            var lastRoundNumber = _gameRoundsInformation.GetLastRoundNumber("test room");
            var newRound = generator.GenerateMathExpression(lastRoundNumber, "test room");
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
                var roomName = HttpContext.Session.GetString("RoomName");
                response = _gameRoundsInformation.ReadRoundsForRoom(roomName);
                foreach(var round in response)
                {
                    if (string.IsNullOrEmpty(round.Result))
                    {
                        round.Result = string.Empty;
                    }
                }
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
        public List<GameRound> GetQuestionsTableAfterRound(int roundNumber)
        {
            try
            {
                var response = new List<GameRound>();
                var roomName = HttpContext.Session.GetString("RoomName");
                response = _gameRoundsInformation.ReadRoundsFromRoomAfterRound(roomName, roundNumber);
                foreach (var round in response)
                {
                    if (string.IsNullOrEmpty(round.Result))
                    {
                        round.Result = string.Empty;
                    }
                }
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
            int isFirst = 0;
            if (!string.IsNullOrEmpty(answer.Answer) && !string.IsNullOrEmpty(answer.RoomName))
            {
                answer.Username = HttpContext.Session.GetString("PlayerName");
                try
                {
                    isFirst = _gameRoundsInformation.SaveAnswerForRound(answer);
                }
                catch
                {
                    return BadRequest(isFirst);
                }
                return Ok(isFirst);
            }

            return BadRequest(isFirst);
        }
    }
}
