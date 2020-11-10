using RPS.Api.Models;
using RPSCore;

namespace RPS.Api.Services
{
    public interface IBotStrategy
    {
        Move GetBotsNextMove(Game lastGame, bool botHasDynamitesAvailable, bool playerHasDynamitesAvailable);
    }
}