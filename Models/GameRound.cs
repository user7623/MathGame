using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathGame.Models
{
    public class GameRound
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int RoundNumber { get; set; }
        public string Expression { get; set; }
        DateTime FirstCorrectAnswerTimestamp { get; set; }
        public string Username { get; set; }

        public GameRound(int Id, string RoomName, int RoundNumber, string Expression, DateTime FirstCorrectAnswerTimestamp, string Username)
        {
            this.Id = Id;
            this.RoomName = RoomName;
            this.RoundNumber = RoundNumber;
            this.Expression = Expression;
            this.FirstCorrectAnswerTimestamp = FirstCorrectAnswerTimestamp;
            this.Username = Username;
        }
    }
}
