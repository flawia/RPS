# RPS

Rock Paper Scissors With a Twist

This server project provides an API that plays Rock Paper Scissors against the Player

##Rules of the Game

 * A match is played between Player and The Bot (API)
 * Player sets number of games in the match and number of dynamites each side can use
 * Rock beats Scissors
 * Scissors beats Paper
 * Paper beats Rock
 * A Dynamite beats Rock, Paper, and Scissors
 * A Waterbombn beats Dynamite.
 * Rock, Paper, and Scissors all beat Waterbomb.
 * All matching choices will be a draw.
 
##Bots strategy

 * First move is random.
 * If Bots wins the round, for the next round, it will select the thing that would beat its previous hand.
 * If Bots loses the round, for the next round, it will select the thing that would beat Players previous hand.
 * Bot keeps track of Players dynamite use and will not play Waterbomb if Player is out of Dynamites.
 

 
