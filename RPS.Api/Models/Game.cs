using RPSCore;

namespace RPS.Api.Models
{
    public class Game
    {
        public Move botMove;
        public Outcome botResult;
        public Move playerMove;
        public Outcome playerResult;

        public Game()
        {
        }
    }
}
