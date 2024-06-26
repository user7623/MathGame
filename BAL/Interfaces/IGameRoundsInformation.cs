﻿using MathGame.Dtos;
using MathGame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathGame.BAL.Interfaces
{
    public interface IGameRoundsInformation
    {
        Task SaveGameRound(GameRound newRound);
        void UpdateGameRound(GameRound newRound);
        List<GameRound> ReadRoundsForRoom(string roomName);
        List<GameRound> ReadRoundsFromRoomAfterRound(string roomName, int roundNumber);
        GameRound GetGameRoundById(int roundNumber, string roomName);
        int SaveAnswerForRound(AnswerDto answer);
        int GetLastRoundNumber(string roomName);
        GameRound GetNewestRound(string roomName);
    }
}
