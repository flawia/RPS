using RPS.Api.Models;
using RPSCore;

namespace RPS.Api.Services
{
    public interface IMatchService
    {
        void NewMatch(int numGames, int numDynamites);
        void AddGame();
        Move BotsMove();
        Game GameResult(Outcome yourOutcome, Move opponentMove);
        string MatchResult(Outcome yourOutcome);
    }
}