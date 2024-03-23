using MathGame.DataAccess;
using MathGame.Models;
using MathGame.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathGame.Repositories
{
    public class GameRoundsRepository : IGameRoundsRepository
    {
        public IContext _context;

        public GameRoundsRepository(IContext context)
        {
            _context = context;
        }

        public bool SaveGameRound(GameRound newRound)
        {
            try
            {
                _context.Instance.GameRounds.Add(newRound);
                _context.Instance.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                //TODO : implement logger
                return false;
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
                return null;
            }
        }
    }
}
