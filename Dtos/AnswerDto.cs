using System;

namespace MathGame.Dtos
{
    public class AnswerDto
    {
        public string Answer { get; set; }//TODO bit might be more efficient
        public int RoundNumber { get; set; }
        public string RoomName { get; set; }
        public int Timestamp { get; set; }
    }
}
