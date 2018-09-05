using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.Cards;
using RPSCardsEngine.UnitTests.Cards.DummyObjects;
using System.Collections.Generic;

namespace RPSCardsEngine.UnitTests.Cards
{
    [TestClass]
    public class HandTest : CardZoneTest<Hand>
    {
        [TestMethod]
        public override void TestCount()
        {
            Hand hand = TestHelpers.GetFilledHand(TestHelpers.BaseUnitDeck);

            Assert.AreEqual(3, hand.Count);

            hand.PlayCard(2, new Board(), 2);
            Assert.AreEqual(2, hand.Count);

            hand.PlayCard(1, new Board(), 1);
            Assert.AreEqual(1, hand.Count);

            hand.PlayCard(0, new Board(), 0);
            Assert.AreEqual(0, hand.Count);
        }

        [TestMethod]
        public void TestHasCards()
        {
            Hand hand = TestHelpers.GetFilledHand(new List<Card> {
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            });

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(hand.HasCard(i));
                Assert.AreEqual(hand[i].Zone, Zone.Hand);
                Assert.AreEqual(hand[i].Position, i);
            }

            Assert.AreEqual(hand[0].Type, CardType.Rock);
            Assert.AreEqual(hand[1].Type, CardType.Paper);
            Assert.AreEqual(hand[2].Type, CardType.Scissor);
        }

        [TestMethod]
        public void TestPlayUnit()
        {
            var testCard = new RPSCardsEngine.Cards.Card(CardType.Rock);
            Hand hand = TestHelpers.GetFilledHand(new List<Card> { testCard });
            var board = new Board();

            hand.PlayCard(0, board, 2);

            Assert.IsFalse(hand.HasCard(0));
            Assert.IsTrue(board.HasCard(2));
            Assert.AreEqual(board[2], testCard);
        }

        [TestMethod]
        public void TestUpgradeUnit()
        {
            var testCard = new RPSCardsEngine.Cards.Card(CardType.Rock);
            Hand hand = TestHelpers.GetFilledHand(new List<Card> {
            testCard,
            new RPSCardsEngine.Cards.Card(CardType.Fire),
            }, new Graveyard());
            var board = new Board(new Graveyard());

            hand.PlayCard(0, board, 2);

            Assert.AreEqual(board[2], testCard);

            hand.PlayCard(1, board, 2);

            Assert.AreEqual(board[2], testCard);
            Assert.AreEqual(testCard.Type, CardType.Magma);

        }

        [TestMethod]
        public void TestDestroyUnit()
        {
            var testCard = new RPSCardsEngine.Cards.Card(CardType.Rock);
            var ownGraveyard = new Graveyard();
            Hand hand = TestHelpers.GetFilledHand(
                new List<Card> {
                    testCard,
                    new RPSCardsEngine.Cards.Card(CardType.Water),}, 
                ownGraveyard);

            var enemyGraveyard = new Graveyard();
            var enemyBoard = new Board(enemyGraveyard);
            

            hand.PlayCard(0, enemyBoard, 2);

            Assert.AreEqual(enemyBoard[2], testCard);
            Assert.AreEqual(enemyBoard.Count, 1);

            hand.PlayCard(1, enemyBoard, 2);

            Assert.AreEqual(enemyBoard[2], testCard);
            Assert.IsTrue(testCard.IsDestroyed);
            Assert.AreEqual(ownGraveyard.Count, 1);

            Assert.AreEqual(enemyBoard.Count, 1);
            enemyBoard.RemoveDestroyed();
            Assert.AreEqual(enemyBoard.Count, 0);

            Assert.AreEqual(enemyGraveyard.Count, 1);
        }

    }
}
