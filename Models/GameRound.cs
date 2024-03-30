using System.ComponentModel.DataAnnotations.Schema;

namespace MathGame.Models
{
    public class GameRound
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int RoundNumber { get; set; }
        public string Expression { get; set; }
        public bool IsCorrect { get; set; }
        public long FirstCorrectAnswerTimestamp { get; set; }
        public string Username { get; set; }
        [NotMapped]
        public string Result { get; set; }

        public GameRound(int Id, string RoomName, int RoundNumber, string Expression, int FirstCorrectAnswerTimestamp, string Username)
        {
            this.Id = Id;
            this.RoomName = RoomName;
            this.RoundNumber = RoundNumber;
            this.Expression = Expression;
            this.FirstCorrectAnswerTimestamp = FirstCorrectAnswerTimestamp;
            this.Username = Username;
            this.Result = string.Empty;
        }

        public GameRound() { }
    }
}
