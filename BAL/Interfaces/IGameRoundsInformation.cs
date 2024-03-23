using MathGame.Models;
using System.Collections.Generic;

namespace MathGame.BAL.Interfaces
{
    public interface IGameRoundsInformation
    {
        bool SaveGameRound(GameRound newRound);
        List<GameRound> ReadRoundsForRoom(string roomName);
    }
}
