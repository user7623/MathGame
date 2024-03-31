using System;
using System.Threading;

namespace MathGame.MathExpressions
{
    public class RoomExpressionGeneratorScheduler : IRoomExpressionGeneratorScheduler
    {
        private Timer timer;
        private bool isRunning;
        private readonly int roundNumber;
        private readonly string roomName;
        private readonly MathExpressionsGenerator _generator;

        /// <summary>
        /// Each instance is responsible for a single room
        /// if there are active players then it generates new expressions
        /// </summary>
        /// <param name="roomName"></param>
        public RoomExpressionGeneratorScheduler(string roomName, MathExpressionsGenerator generator)
        {
            isRunning = false;
            roundNumber = 0;
            this.roomName = roomName;
            _generator = generator;
        }

        public void Start()
        { 
            if (!isRunning)
            {
                timer = new Timer(GenerateExpression, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
                isRunning = true;
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                timer.Dispose();
                isRunning = false;
            }
        }

        private void GenerateExpression(object state)
        {

            _generator.GenerateMathExpression(roundNumber, roomName);
        }
    }
}
