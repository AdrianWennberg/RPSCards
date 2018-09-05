using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.Cards;
using RPSCardsEngine.UnitTests.Cards.DummyObjects;
using System.Collections.Generic;
using System.Linq;

namespace RPSCardsEngine.UnitTests.Cards
{
    [TestClass]
    public class BoardTest : CardZoneTest<Graveyard>
    {
        [TestMethod]
        public override void TestCount()
        {
            Hand hand = TestHelpers.GetFilledHand(TestHelpers.BaseUnitDeck);
            var board = new Board();

            for (int i = 0; i < hand.Capacity; i++)
            {
                Assert.AreEqual(board.Count, i);
                hand.PlayCard(i, board, i);
            }
            Assert.AreEqual(board.Count, 3);

        }

        [TestMethod]
        public void TestHasCards()
        {
            Board board = TestHelpers.GetFilledBoard(new List<Card> {
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            });

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(board.HasCard(i));
                Assert.AreEqual(board[i].Zone, Zone.Board);
                Assert.AreEqual(board[i].Position, i);
            }

            Assert.AreEqual(board[0].Type, CardType.Rock);
            Assert.AreEqual(board[1].Type, CardType.Paper);
            Assert.AreEqual(board[2].Type, CardType.Scissor);
        }

        [TestMethod]
        public void TestFight()
        {
            Board myBoard = TestHelpers.GetFilledBoard(TestHelpers.BaseUnitDeck);
            Board enemyBoard = TestHelpers.GetFilledBoard(TestHelpers.BaseUnitDeck);

            myBoard.Fight(enemyBoard);

            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                Assert.IsFalse(myBoard[i].IsDestroyed);
                Assert.IsFalse(enemyBoard[i].IsDestroyed);
            }

            myBoard = TestHelpers.GetUpgradedBoard(new List<CardType>
            {
                CardType.Axe,
                CardType.Laser,
                CardType.WaterGun
            });

            myBoard.Fight(enemyBoard);

            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                Assert.IsFalse(myBoard[i].IsDestroyed);
                Assert.IsTrue(enemyBoard[i].IsDestroyed);
            }

            enemyBoard = TestHelpers.GetUpgradedBoard(new List<CardType>
            {
                CardType.Axe,
                CardType.Laser,
                CardType.WaterGun
            });

            myBoard = TestHelpers.GetFilledBoard(TestHelpers.BaseUnitDeck);

            myBoard.Fight(enemyBoard);

            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                Assert.IsFalse(enemyBoard[i].IsDestroyed);
                Assert.IsTrue(myBoard[i].IsDestroyed);
            }
        }

        [TestMethod]
        public void TestRemoveDestroyed()
        {
            Board myBoard = TestHelpers.GetFilledBoard(TestHelpers.BaseUnitDeck);
            Board enemyBoard = TestHelpers.GetUpgradedBoard(new List<CardType>
            {
                CardType.Axe,
                CardType.Paper,
                CardType.WaterGun
            });

            myBoard.Fight(enemyBoard);

            Assert.IsTrue(myBoard[0].IsDestroyed);
            Assert.IsFalse(myBoard[1].IsDestroyed);
            Assert.IsTrue(myBoard[2].IsDestroyed);

            myBoard.RemoveDestroyed();

            Assert.IsFalse(myBoard.HasCard(0));
            Assert.IsTrue(myBoard.HasCard(1));
            Assert.IsFalse(myBoard[1].IsDestroyed);
            Assert.IsFalse(myBoard.HasCard(2));
        }
    }
}
