using MathGame.DataAccess;
using MathGame.Dtos;
using MathGame.Models;
using MathGame.Repositories.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathGame.Repositories
{
    public class GameRoundsRepository : IGameRoundsRepository
    {
        public IContext _context;
        //TODO : maybe make calls async?
        public GameRoundsRepository(IContext context)
        {
            _context = context;
        }

        public async Task SaveGameRound(GameRound newRound)
        {
            try
            {
                 await _context.Instance.GameRounds.AddAsync(newRound);
                 await _context.Instance.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
            }
        }
     
        public void UpdateGameRound(GameRound newRound)
        {
            try
            {
                _context.Instance.GameRounds.Update(newRound);
                _context.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
            }
        }

        public List<GameRound> ReadRoundsForRoom(string roomName)
        {
            try
            {
                var rounds = _context.Instance.GameRounds.Where(r => r.RoomName.Equals(roomName))
                                                         .OrderBy(r => r.RoundNumber)
                                                         .ToList();

                return rounds;
            }
            catch(Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
                return null;
            }
        }

        public GameRound GetGameRoundById(int roundNumber, string roomName)
        {
            try
            {
                return _context.Instance.GameRounds.Where(r => r.RoomName.Equals(roomName)
                                                            && r.RoundNumber == roundNumber)
                                                   .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
                return null;
            }
        }
        public void SaveAnswerForRound(AnswerDto answer)
        {
            try
            {
                //

            }
            catch(Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> SaveAnswerForRound");
                Log.Error(ex.ToString());
            }
        }

        public int GetLastRoundNumber(string roomName)
        {
            try
            {
                var rounds = _context.Instance.GameRounds.Where(r => r.RoomName.Equals(roomName)).ToList();

                if (rounds.Count == 0) return 0;//First round

                return rounds.OrderByDescending(r => r.RoundNumber)
                             .Select(r => r.RoundNumber)
                             .FirstOrDefault();
            }
            catch(Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> GetLastRoundNumber");
                Log.Error(ex.ToString());
                return 1;
            }
        }

        public GameRound GetNewestRound(string roomName)
        {
            try
            {
                return _context.Instance.GameRounds.OrderByDescending(r => r.RoundNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error("Exception in GameRoundsInformation -> GetNewestRound");
                Log.Error(ex.ToString());
                return null;
            }
        }
    }
}
