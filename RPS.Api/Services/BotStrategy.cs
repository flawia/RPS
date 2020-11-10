using RPS.Api.Models;
using RPSCore;
using System;
using System.Linq;

namespace RPS.Api.Services
{
    public class BotStrategy : IBotStrategy
    {
        private static Random random = new Random();

        public Move GetBotsNextMove(Game lastGame, bool botHasDynamitesAvailable, bool playerHasDynamitesAvailable)
        {
            Move nextMove;

            //if previous games exist, use the strategy
            if (lastGame !=null)
            {
                //if bot lost previous game, it will select the move that would beat players previous move
                if (lastGame.botResult == Outcome.Lose)
                {
                    nextMove = ReturnMoveBeater(lastGame.playerMove, botHasDynamitesAvailable, playerHasDynamitesAvailable);
                }
                //if bot won previous game, it will select the move that would beat its previous move 
                else if (lastGame.botResult == Outcome.Win)
                {
                    nextMove = ReturnMoveBeater(lastGame.botMove, botHasDynamitesAvailable, playerHasDynamitesAvailable);
                }
                //if draw, select move at random
                else
                {
                    nextMove = GetRandomMoveFromSet(botHasDynamitesAvailable, playerHasDynamitesAvailable);
                }
            }
            // if first game, select move at random
            else
            {
                nextMove = GetRandomMoveFromSet(botHasDynamitesAvailable, playerHasDynamitesAvailable);
            }

            return nextMove;
        }

        private Move GetRandomMoveFromSet(bool botHasDynamitesAvailable, bool playerHasDynamitesAvailable)
        {
            //if no dynamites are allowed in the game, no point using dynamite or waterbomb
            if (botHasDynamitesAvailable == false && playerHasDynamitesAvailable==false)
            {
                return (Move)random.Next(1, 4);
            }
            //if bot and player have still dynamite hand available all moves are allowed 
            else if (botHasDynamitesAvailable && playerHasDynamitesAvailable)
            {
                return (Move)random.Next(1, 6);
            }
            //if player has no dynamites left, no point playing waterbomb
            else if (botHasDynamitesAvailable && playerHasDynamitesAvailable == false)
            {
                return (Move)random.Next(1, 5);
            }
            //if bot has no dynamites left, make sure it never gets selected
            else
            {

                Move move = Move.Dynamite;
                while (move == Move.Dynamite)
                {
                    move = (Move)random.Next(1, 6);
                }
                return move;
            }
        }

        private Move ReturnMoveBeater(Move moveToBeat, bool botHasDynamitesAvailable, bool playerHasDynamitesAvailable)
        {
            var moveHelpers = new MoveHelpers();
            //this selects all the moves that beat the hand
            var availableMoves = moveHelpers.GetBeaters(moveToBeat);

            //if bot knows that player run out of dynamites, no point playing waterbomb
            if (availableMoves.Contains(Move.Waterbomb) && playerHasDynamitesAvailable == false)
            {
                return GetRandomMoveFromSet(botHasDynamitesAvailable, playerHasDynamitesAvailable);
            }
            //if bot does not have any dynamites left, cant play it
            else if (availableMoves.Contains(Move.Dynamite) && botHasDynamitesAvailable == false)
            {
                return availableMoves.Where(x => x != Move.Dynamite).FirstOrDefault();
            }
            //if more than one moves available, select one at random
            else
            {
                int index = new Random().Next(availableMoves.Count());
                return availableMoves.ElementAt(index);
            }
        }
    }
}
