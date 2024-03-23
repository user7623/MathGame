using MathGame.Models;
using System.Collections.Generic;

namespace MathGame.Repositories.Interfaces
{
    public interface IGameRoundsRepository
    {
        bool SaveGameRound(GameRound newRound);
        List<GameRound> ReadRoundsForRoom(string roomName);
    }
}
