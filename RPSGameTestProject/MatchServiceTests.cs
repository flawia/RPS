using Moq;
using NUnit.Framework;
using RPS.Api.Exceptions;
using RPS.Api.Models;
using RPS.Api.Services;
using RPSCore;
using System;
using System.Linq;

namespace RPSGameTestProject
{
    public class MatchServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GivenNewMatch_WhenZeroGamesSpecified_ThenExceptionIsThrown()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);

            // Act + Assert
            Assert.Throws<InvalidNumberOfGamesException>(() => service.NewMatch(0, 0));
        }

        [Test]
        public void GivenNewMatch_WhenNegativeNumberOfDynamitesSpecified_ThenExceptionIsThrown()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);

            // Act + Assert
            Assert.Throws<InvalidNumberOfDynamitesException>(() => service.NewMatch(5, -1));
        }

        [Test]
        public void GivenNewMatch_WhenNegativeNumberOfGamesSpecified_ThenExceptionIsThrown()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);

            // Act + Assert
            Assert.Throws<InvalidNumberOfGamesException>(() => service.NewMatch(-34, 0));
        }

        [Test]
        public void GivenNewMatch_WhenMoreDynamitesThanGames_ThenExceptionIsThrown()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);

            // Act + Assert
            Assert.Throws<InvalidNumberOfDynamitesException>(() => service.NewMatch(4, 5));
        }

        [Test]
        public void GivenNewMatch_WhenFirstGameAndNoDynamitesAvailableInTheGame_ThenBotsMoveShouldBeMadeWithNoDynamiteAndNoWaterbomb()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);

            service.NewMatch(3, 0);

            //Act
            service.AddGame();
            var botsDecision = service.BotsMove();
            
            //Assert
            Assert.AreNotEqual(Move.Dynamite, botsDecision);
            Assert.AreNotEqual(Move.Waterbomb, botsDecision);
        }

        [Test]
        public void GivenNewMatch_WhenFirstGameAndDynamitesAvailableInTheGame_ThenBotsMoveShouldBeMadeAccordingly()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);
            service.NewMatch(3, 0);

            //Act
            service.AddGame();
            var botsDecision = service.BotsMove();

            //Assert
            Assert.That(Enum.IsDefined(typeof(Move),botsDecision));
            Assert.Contains(botsDecision, Enum.GetValues(typeof(Move)).Cast<Move>().ToList());
        }

        [Test]
        public void GivenExistingMatch_WhenPreviousGameWonAndNoDynamitesAvailableInTheGame_ThenBotsMoveShouldBeOfOneThatBeatsItsPreviousHandWithNoDynamitAndNoWaterbomb()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);
            service.NewMatch(3, 0);

            //Act

            //first game
            service.AddGame();
            var botsFirstMove = service.BotsMove();
            service.GameResult(Outcome.Win, Move.Paper);

            //second game
            service.AddGame();
            var botsSecondMove = service.BotsMove();
            MoveHelpers mh = new MoveHelpers();
            var beaters = mh.GetBeaters(botsFirstMove).ToList();

            //Assert
            Assert.AreNotEqual(Move.Dynamite, botsSecondMove);
            Assert.AreNotEqual(Move.Waterbomb, botsSecondMove);
            Assert.Contains(botsSecondMove, beaters);
        }

        [Test]
        public void GivenExistingMatch_WhenPreviousGameWonAndDynamitesAvailableInTheGame_ThenBotsMoveShouldBeOfOneThatBeatsItsPreviousHand()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);
            service.NewMatch(3, 0);

            //Act

            //first game
            service.AddGame();
            var botsFirstMove = service.BotsMove();
            service.GameResult(Outcome.Win, Move.Paper);

            //second game
            service.AddGame();
            var botsSecondMove = service.BotsMove();
            MoveHelpers mh = new MoveHelpers();
            var beaters = mh.GetBeaters(botsFirstMove).ToList();

            //Assert
            Assert.Contains(botsSecondMove, beaters);
        }

        [Test]
        public void GivenExistingMatch_WhenPreviousGameLostAndNoDynamitesAvailableInTheGame_ThenBotsMoveShouldBeOfOneThatBeatsPlayersPreviousHandWithNoDynamitAndNoWaterbomb()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);
            service.NewMatch(3, 0);

            Move playersMove = Move.Paper;

            //Act

            //first game
            service.AddGame();
            var botsFirstMove = service.BotsMove();
            service.GameResult(Outcome.Lose, playersMove);

            //second game
            service.AddGame();
            var botsSecondMove = service.BotsMove();
            MoveHelpers mh = new MoveHelpers();
            var beaters = mh.GetBeaters(playersMove).ToList();

            //Assert
            Assert.AreNotEqual(Move.Dynamite, botsSecondMove);
            Assert.AreNotEqual(Move.Waterbomb, botsSecondMove);
            Assert.Contains(botsSecondMove, beaters);
        }

        [Test]
        public void GivenExistingMatch_WhenPreviousGameLostAndDynamitesAvailableInTheGame_ThenBotsMoveShouldBeOfOneThatBeatsPlayersPreviousHandWithNoDynamite()
        {
            // Assemble
            var botStrategy = new BotStrategy();
            IMatchService service = new MatchService(botStrategy);
            service.NewMatch(3, 0);

            Move playersMove = Move.Paper;

            //Act

            //first game
            service.AddGame();
            var botsFirstMove = service.BotsMove();
            service.GameResult(Outcome.Lose, playersMove);

            //second game
            service.AddGame();
            var botsSecondMove = service.BotsMove();
            MoveHelpers mh = new MoveHelpers();
            var beaters = mh.GetBeaters(playersMove).ToList();

            //Assert
            Assert.AreNotEqual(Move.Dynamite, botsSecondMove);
            Assert.Contains(botsSecondMove, beaters);
        }

        [Test]
        public void GivenExistingMatch_WhenDynamiteAvailableInTheGameAndAlreadyUsedByBot_ThenBotsMoveShouldBeOfOneThatBeatsPlayersPreviousHand()
        {
            // Assemble
            var botStrategy = new BotStrategy();

            var strategyMock = new Mock<IBotStrategy>();

            var sequence = strategyMock.SetupSequence(m => m.GetBotsNextMove(It.IsAny<Game>(), It.IsAny<bool>(), It.IsAny<bool>()));
                        

            IMatchService service = new MatchService(strategyMock.Object);


            service.NewMatch(3, 1);

            Move playersMove = Move.Waterbomb;

            //Act

            //first game
            sequence.Returns(Move.Dynamite);
            service.AddGame();
            var botsFirstMove = service.BotsMove();
            var gameResult = service.GameResult(Outcome.Lose, playersMove);

            //second game
            sequence.Returns(botStrategy.GetBotsNextMove(gameResult, false, true));
            service.AddGame();
            var botsSecondMove = service.BotsMove();
            MoveHelpers mh = new MoveHelpers();
            var beaters = mh.GetBeaters(playersMove).ToList();

            //Assert
            Assert.Contains(botsSecondMove, beaters);
        }
    }

    public class MoveHelperTest
    {
        [Test]
        public void GetLosingMoves()
        {
            MoveHelpers mh = new MoveHelpers();
            var beaters = mh.GetBeaters(Move.Rock);

            CollectionAssert.AreEquivalent(new[] { Move.Paper, Move.Dynamite }, beaters);
        }
    }
}