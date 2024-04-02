namespace MathGame.Dtos
{
    public class AnswerDto
    {
        public string Answer { get; set; }//TODO bit might be more efficient
        public int RoundNumber { get; set; }
        public string RoomName { get; set; }
        public long Timestamp { get; set; }
        public string? Username { get; set; }
    }
}
