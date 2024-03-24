using MathGame.BAL.Interfaces;
using MathGame.Dtos;
using MathGame.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Transports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var roomName = "test room";//TODO
            response = _gameRoundsInformation.ReadRoundsForRoom(roomName);

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

        #region forTestingDeleteWhenDone

        public int GetOnlineCount()
        {
            return 1;
        }

        public void TestForSavingAndReadingGameRounds()
        {
            try
            {
                GameRound testGameRound = new GameRound
                {
                    Expression = "2+1=3",
                    FirstCorrectAnswerTimestamp = 11111111,
                    Username = "FILIPUIS",
                    RoomName = "test room",
                    RoundNumber = 11
                };

                _gameRoundsInformation.SaveGameRound(testGameRound);
            } catch(Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        public void TestReadGameRound()
        {
            var testRound = new List<GameRound>();

            testRound = _gameRoundsInformation.ReadRoundsForRoom("blueRoom");
            testRound.AddRange(_gameRoundsInformation.ReadRoundsForRoom("test room"));

            testRound = testRound;
        }

        public void TestUpdateGameRound()
        {
            var testRound = _gameRoundsInformation.ReadRoundsForRoom("test room");
            testRound.FirstOrDefault().FirstCorrectAnswerTimestamp = DateTime.Now.Ticks;
            testRound.FirstOrDefault().Username = "F ILI P";
            _gameRoundsInformation.UpdateGameRound(testRound.FirstOrDefault());
        }

        #endregion
    }
}
