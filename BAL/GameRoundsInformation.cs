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

        public List<GameRound> ReadRoundsFromRoomAfterRound(string roomName, int roundNumber)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return null;
            }
            //TODO : maybe an aditional check for validity / special characters and similar
            return _repository.ReadRoundsForRoomAfterRound(roomName, roundNumber);
        }

        /// <summary>
        /// Checks answer objecs validity, then checks if answer is correct
        /// If first correct answer returns 1
        /// If correct but not first retruns 0
        /// If answer is wrong returns -1
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public int SaveAnswerForRound(AnswerDto answer)
        {
            try
            {
                bool isValid = ValidateAnswer(answer);
                bool isCorrect = false;
                if (isValid && answer.RoundNumber > 0)
                {
                    var gameRound = _repository.GetGameRoundById(answer.RoundNumber, answer.RoomName);

                    if(gameRound.IsCorrect && answer.Answer.ToLower().Equals("yes")
                        || !gameRound.IsCorrect && answer.Answer.ToLower().Equals("no"))
                    {
                        isCorrect = true;
                    }

                    if ((gameRound.FirstCorrectAnswerTimestamp > answer.Timestamp
                        || string.IsNullOrEmpty(gameRound.FirstCorrectAnswerTimestamp.ToString()))
                        && isCorrect)
                    {
                        gameRound.FirstCorrectAnswerTimestamp = answer.Timestamp;

                        gameRound.Username = answer.Username;
                        _repository.UpdateGameRound(gameRound);
                        return 1;
                    }
                    if (isCorrect) { return 0; }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
            }
            return -1;
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

        private bool ValidateAnswer(AnswerDto answer)
        {
            bool isValid = false;
            if (answer.Timestamp.ToString() != null 
                && !string.IsNullOrEmpty(answer.Answer) 
                && !string.IsNullOrEmpty(answer.RoomName) 
                && !string.IsNullOrEmpty(answer.Username))
            {
                isValid = true;
            }
            return isValid;
        }
    }
}
