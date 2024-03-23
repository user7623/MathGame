using MathGame.BAL.Interfaces;
using MathGame.Models;
using MathGame.Repositories.Interfaces;
using System.Collections.Generic;

namespace MathGame.BAL
{
    public class GameRoundsInformation : IGameRoundsInformation
    {
        public readonly IGameRoundsRepository _repository;

        public GameRoundsInformation(IGameRoundsRepository repository)
        {
            _repository = repository;
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

        public bool SaveGameRound(GameRound newRound)
        {
            return _repository.SaveGameRound(newRound);
        }
    }
}
