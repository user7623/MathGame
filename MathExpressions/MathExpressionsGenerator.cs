using MathGame.BAL.Interfaces;
using MathGame.Models;
using System;

namespace MathGame.MathExpressions
{
    public class MathExpressionsGenerator
    {
        /// <summary>
        /// Receives the name of a game room and the number of the current round
        /// Makes an expression and saves it with default timestamp and username
        /// </summary>
        /// <param name="roundNumber"></param>
        /// <param name="roomName"></param>
        public GameRound GenerateMathExpression(int roundNumber, string roomName)
        {
            var randomInteger = new Random();
            char[] operations = { '*', '/', '+', '-' };
            bool isCorrect = randomInteger.Next(1, 10) % 2 == 0;
            var operationSymbol = operations[randomInteger.Next(0, 3)];

            var firstOperand = randomInteger.Next(1, 10);
            var secondOperand = randomInteger.Next(1, 10);
            var resultFromOperation = CalculateResult(firstOperand, secondOperand, operationSymbol);

            //instead of writting two seperate functions and breaking the DRY priciple its better to make just one function for
            //generating expressions, also if the operation type is allways chosen at random and the added error is allways random
            //then the probability for prediction on user's end is far smaller, plus its more fun to make it like this :)
            if (!isCorrect)
            {
                resultFromOperation = AddError(resultFromOperation, randomInteger.Next(1, 10), operations[randomInteger.Next(0, 3)]);
            }

            string expression = string.Concat(firstOperand, operationSymbol, secondOperand, "=", resultFromOperation);

            //we decide who answered first based on the timestamp of the answer
            //instead of saving null as default value before answer is given by user
            //we save a timestamp that is in the future so when the user's answer 
            //arrives it will be "first" and get saved along with a real value for username        
            var futureTimestamp = DateTimeOffset.Now.AddDays(1).ToUnixTimeMilliseconds();

            GameRound newGameRound = new GameRound
            {
                IsCorrect = isCorrect,
                RoundNumber = roundNumber + 1,
                Expression = expression,
                Username = string.Empty,
                FirstCorrectAnswerTimestamp = futureTimestamp,
                RoomName = roomName
            };

            return newGameRound;
        }

        private int CalculateResult(int firstOperand, int secondOperand, char operationSymbol)
        {
            switch (operationSymbol)
            {
                case '*':
                    return firstOperand * secondOperand; 
                case '/':
                    return firstOperand / secondOperand; 
                case '+':
                    return firstOperand + secondOperand;
                case '-':
                    return firstOperand - secondOperand;
                default:
                    break;
            }
            return 0;
        }

        private int AddError(int correctResult, int errorNumber,char operationSymbol)
        {
            switch (operationSymbol)
            {
                case '*':
                    return correctResult * errorNumber;
                case '/':
                    return correctResult / errorNumber;
                case '+':
                    return correctResult + errorNumber;
                case '-':
                    return correctResult - errorNumber;
                default:
                    break;
            }
            return 0;
        }
    }
}
