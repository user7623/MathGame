using MathGame.Dtos;
using MathGame.Models;
using System.Collections.Generic;

namespace MathGame.BAL.Interfaces
{
    public interface IGameRoundsInformation
    {
        void SaveGameRound(GameRound newRound);
        void UpdateGameRound(GameRound newRound);
        List<GameRound> ReadRoundsForRoom(string roomName);
        GameRound GetGameRoundById(int roundNumber, string roomName);
        void SaveAnswerForRound(AnswerDto answer);
    }
}
