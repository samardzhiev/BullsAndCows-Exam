namespace BullsAndCows.GameLogic
{
    using System;
    using System.Linq;

    using BullsAndCows.Models;

    public class GuessVerifier
    {
        public static Guess GetResult(string answerNumber, string guessNumber)
        {
            Guess guess = new Guess();

            bool[] bulls = new bool[4];
            bool[] cows = new bool[4];

            for (int i = guessNumber.Length - 1; i >= 0; i--)
            {
                if (guessNumber[i] == answerNumber[i])
                {
                    guess.BullsCount++;
                    guessNumber.Remove(i);
                    answerNumber.Remove(i);
                }
            }

            for (int i = guessNumber.Length - 1; i >= 0; i--)
            {
                for (int j = answerNumber.Length - 1; j >= 0; j--)
                {
                    if (guessNumber[i] == answerNumber[j])
                    {
                        guess.CowsCount++;
                        guessNumber.Remove(i);
                        answerNumber.Remove(j);
                    }
                }
            }

            return guess;
        }
    }
}
