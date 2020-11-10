using System;
using System.Collections.Generic;
using System.Linq;

namespace RPSCore
{
    public class MoveHelpers
    {
        public IEnumerable<Move> GetBeaters(Move move)
        {
            var value = move.GetType().GetMember(move.ToString());
            var attributes = value.SingleOrDefault().GetCustomAttributes(typeof(LosesToAttribute), false);
            return attributes.Cast<LosesToAttribute>().Select(a => a.Move);
        }
    }

    public enum Move
    {
        [LosesTo(Paper)]
        [LosesTo(Dynamite)]
        Rock = 1, //beats Scissors

        [LosesTo(Scissors)]
        [LosesTo(Dynamite)]
        Paper = 2, //beats Rock

        [LosesTo(Rock)]
        [LosesTo(Dynamite)]
        Scissors = 3, //beats Paper

        [LosesTo(Waterbomb)]
        Dynamite = 4, //beats everything but Waterbomb

        [LosesTo(Rock)]
        [LosesTo(Paper)]
        [LosesTo(Scissors)]
        Waterbomb = 5 //loses to everything but Dynamite
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    class LosesToAttribute : Attribute
    {
        public LosesToAttribute(Move move)
        {
            Move = move;
        }

        public Move Move { get; }
    }
}
