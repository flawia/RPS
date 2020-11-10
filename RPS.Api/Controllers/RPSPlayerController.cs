using Microsoft.AspNetCore.Mvc;
using RPS.Api.Services;
using RPSCore;

namespace RPS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RPSPlayerController : ControllerBase, IRPSPlayer
    {
        private IMatchService match;

        public RPSPlayerController(IMatchService match)
        {
            this.match = match;
        }

        [HttpGet("GetReady/{numGames}/{numDynamite}")]
        public string GetReady([FromRoute] int numGames, [FromRoute] int numDynamite)
        {
            match.NewMatch(numGames, numDynamite);
            return $"New Match Started. {numGames} game(s) left to play. {numDynamite} dynamite(s) available.";
        }

        [HttpGet]
        [Route("MakeMove")]
        public Move MakeMove()
        {
            match.AddGame();
            return match.BotsMove();
        }

        [HttpGet("GameResult/{yourOutcome}/{opponentMove}")]
        public void GameResult([FromRoute] Outcome yourOutcome, [FromRoute] Move opponentMove)
        {
            match.GameResult(yourOutcome, opponentMove);
        }

        [HttpGet("Result/{yourOutcome}")]
        public string Result(Outcome yourOutcome)
        {
            var matchResult = match.MatchResult();
            return matchResult;
        }
    }
}