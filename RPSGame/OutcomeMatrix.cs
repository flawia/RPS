using RPSCore;
using System;

namespace RPSGame
{
    public class OutcomeMatrix
    {
        private static Move[,] outcomeMatrix = new Move[,] {
            {Move.Paper, Move.Rock }, {Move.Rock, Move.Scissors}, {Move.Scissors, Move.Paper},
            {Move.Dynamite, Move.Paper }, {Move.Dynamite, Move.Rock}, {Move.Dynamite, Move.Scissors}, {Move.Warterbomb, Move.Dynamite}
        };
    }
}
