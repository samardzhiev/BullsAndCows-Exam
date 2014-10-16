namespace BullsAndCows.Models
{
    using System;
    using System.Linq;

    public enum GameState
    {
        WaitingForOpponent,
        BlueInTurn,
        RedInTurn,
        WonByBluePlayer,
        WonByRedPlayer
    }
}
