using MathGame.BAL.Interfaces;
using MathGame.Dtos;
using MathGame.Models;
using MathGame.Repositories.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathGame.BAL
{
    public class GameRoundsInformation : IGameRoundsInformation
    {
        public readonly IGameRoundsRepository _repository;

        public GameRoundsInformation(IGameRoundsRepository repository)
        {
            _repository = repository;
        }

        public GameRound GetGameRoundById(int roundNumber, string roomName)
        {
            return _repository.GetGameRoundById(roundNumber, roomName);
        }

        public int GetLastRoundNumber(string roomName)
        {
            return _repository.GetLastRoundNumber(roomName);
        }

        public GameRound GetNewestRound(string roomName)
        {
            return _repository.GetNewestRound(roomName);
        }

        public List<GameRound> ReadRoundsForRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return null;
            }
            //TODO : maybe an aditional check for validity / special characters and similar
            return _repository.ReadRoundsForRoom(roomName);
        }

        public void SaveAnswerForRound(AnswerDto answer)
        {
            try
            {
                var gameRound = _repository.GetGameRoundById(answer.RoundNumber, answer.RoomName);

                if (gameRound.FirstCorrectAnswerTimestamp > answer.Timestamp
                    || string.IsNullOrEmpty(gameRound.FirstCorrectAnswerTimestamp.ToString()))
                {
                    gameRound.FirstCorrectAnswerTimestamp = answer.Timestamp;
                    gameRound.Username = answer.Username;
                    _repository.UpdateGameRound(gameRound);
                }

                _repository.SaveAnswerForRound(answer);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
            }
        }

        public async Task SaveGameRound(GameRound newRound)
        {
            try
            {
                await _repository.SaveGameRound(newRound);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveGameRound");
                Log.Error(ex.ToString());
            }
        }

        public void UpdateGameRound(GameRound newRound)
        {
            try
            {
                _repository.UpdateGameRound(newRound);
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> UpdateGameRound");
                Log.Error(ex.ToString());
            }
        }
    }
}
