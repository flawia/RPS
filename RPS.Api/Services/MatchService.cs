using RPS.Api.Exceptions;
using RPS.Api.Models;
using RPSCore;
using System;
using System.Linq;

namespace RPS.Api.Services
{

    public class MatchService : IMatchService
    {
        private Game[] games;
        private int numOfGames;
        private int numOfDynamites;
        private int currentGameIndex;
        private int botNumOfDynamitesLeft;
        private int playerNumOfDynamitesLeft;
        private IBotStrategy strategy;

        public MatchService(IBotStrategy botStrategy)
        {
            strategy = botStrategy;
        }

        void IMatchService.NewMatch(int numGames, int numDynamites)
        {
            if (numGames <= 0)
            {
                throw new InvalidNumberOfGamesException("Number of games must be 1 or greater.");
            }

            if (numDynamites < 0)
            {
                throw new InvalidNumberOfDynamitesException("Number of dynamites must be 0 or greater.");
            }

            if (numDynamites > numGames)
            {
                throw new InvalidNumberOfDynamitesException("Too Many Dynamites");
            }

            numOfDynamites = numDynamites;
            numOfGames = numGames;
            currentGameIndex = 0;
            botNumOfDynamitesLeft = numOfDynamites;
            playerNumOfDynamitesLeft = numOfDynamites;

            games = new Game[numOfGames];
        }

        void IMatchService.AddGame()
        {
            if (games == null)
            {
                games = new Game[numOfGames];
            }

            games[currentGameIndex] = new Game();
        }

        private void UpdateGameWithBotMove(Move botMove, Game game)
        {
            game.botMove = botMove;
        }

        private void UpdateGameWithResultsAndOpponentMove(Outcome myResult, Move opponentMove, Game game)
        {
            game.botResult = myResult;
            game.playerMove = opponentMove;
            game.playerResult = GetPlayerResult(myResult);
        }

        private Outcome GetPlayerResult(Outcome myResult)
        {
            switch (myResult)
            {
                case Outcome.Win:
                    return Outcome.Lose;
                case Outcome.Lose:
                    return Outcome.Win;
                default:
                    return Outcome.Draw;
            }
        }

        private void LogBotsDynamiteUsage(Move nextMove)
        {
            if (nextMove == Move.Dynamite)
                botNumOfDynamitesLeft--;
        }

        private void LogPlayersDynamiteUse(Move opponentMove)
        {
            if (opponentMove == Move.Dynamite)
                playerNumOfDynamitesLeft--;
        }


        Move IMatchService.BotsMove()
        {
            Move move;
            if (currentGameIndex == 0)
                move = strategy.GetBotsNextMove(null, botNumOfDynamitesLeft != 0, playerNumOfDynamitesLeft != 0);
            else
                move = strategy.GetBotsNextMove(games[currentGameIndex - 1], botNumOfDynamitesLeft != 0, playerNumOfDynamitesLeft != 0);
            LogBotsDynamiteUsage(move);
            UpdateGameWithBotMove(move, games[currentGameIndex]);
            return move;
        }

        Game IMatchService.GameResult(Outcome yourOutcome, Move opponentMove)
        {
            if (playerNumOfDynamitesLeft == 0 && opponentMove == Move.Dynamite)
            {
                throw new InvalidNumberOfDynamitesException("You have used all your dynamites!");
            }
            UpdateGameWithResultsAndOpponentMove(yourOutcome, opponentMove, games[currentGameIndex]);
            LogPlayersDynamiteUse(opponentMove);
            currentGameIndex++;

            return games[currentGameIndex - 1];
        }

        public string MatchResult(Outcome yourOutcome)
        {
            var botWins = games.Where(x => x.botResult == Outcome.Win).Count();
            var playerWins = games.Where(x => x.playerResult == Outcome.Win).Count();
            var draws = games.Where(x => x.botResult == Outcome.Draw).Count();

            return $"Game result: {yourOutcome.ToString()}{Environment.NewLine}Match statistics:{Environment.NewLine} Number of games played: {games.Count()}{Environment.NewLine} Bot: {botWins} wins{Environment.NewLine} Human: {playerWins} wins{Environment.NewLine} Draw(s): {draws}";
        }
    }
}
