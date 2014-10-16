using BullsAndCows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.GameLogic
{
    public class DesideFirstPlayerGuess
    {
        public static GameState Deside
        {
            get
            {
                if (new Random().Next(0, 2) == 0)
                {
                    return GameState.RedInTurn;
                }
                else
                {
                    return GameState.BlueInTurn;
                }
            }
        }
    }
}
