using MathGame.Dtos;
using MathGame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathGame.Repositories.Interfaces
{
    public interface IGameRoundsRepository
    {
        Task SaveGameRound(GameRound newRound);
        void UpdateGameRound(GameRound newRound);
        List<GameRound> ReadRoundsForRoom(string roomName);
        List<GameRound> ReadRoundsForRoomAfterRound(string roomName, int roundNumber);
        GameRound GetGameRoundById(int roundNumber, string roomName);
        void SaveAnswerForRound(AnswerDto answer);
        int GetLastRoundNumber(string roomName);
        GameRound GetNewestRound(string roomName);
    }
}
