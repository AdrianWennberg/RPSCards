using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.Cards;
using System;
using System.Collections.Generic;

namespace RPSCardsEngine.UnitTests.Cards
{
    [TestClass]
    public class DeckTest : CardZoneTest<Deck>
    {

        [TestMethod]
        public override void TestCount()
        {
            var deck = new Deck(new List<Card> {
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Fire),
            new RPSCardsEngine.Cards.Card(CardType.Water),
            new RPSCardsEngine.Cards.Card(CardType.Wood), });

            Assert.AreEqual(deck.Count, 9);
            deck.Draw(3, new Hand(3, new Graveyard()));
            Assert.AreEqual(deck.Count, 6);
            deck.Draw(4, new Hand(4, new Graveyard()));
            Assert.AreEqual(deck.Count, 2);
            deck.Draw(2, new Hand(2, new Graveyard()));
            Assert.AreEqual(deck.Count, 0);
        }

        [TestMethod]
        public void TestDraw()
        {
            var deck = new Deck(new List<Card> {
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Fire),
            new RPSCardsEngine.Cards.Card(CardType.Water),
            new RPSCardsEngine.Cards.Card(CardType.Wood), });


            var hand1 = new Hand(3, new Graveyard());
            Assert.AreEqual(deck.Count, 9);
            deck.Draw(3, hand1);
            Assert.AreEqual(deck.Count, 6);
            Assert.AreEqual(hand1.Count, 3);


            var hand2 = new Hand(5, new Graveyard());
            var hand3 = new Hand(1, new Graveyard());
            Assert.ThrowsException<InvalidOperationException>(() => deck.Draw(3, hand1));
            Assert.ThrowsException<InvalidOperationException>(() => deck.Draw(5, hand3));

            deck.Draw(5, hand2);
            Assert.AreEqual(hand2.Count, 5);
            Assert.AreEqual(deck.Count, 1);

            deck.Draw(1, hand3);
            Assert.AreEqual(hand3.Count, 1);
            Assert.AreEqual(deck.Count, 0);

            Assert.ThrowsException<InvalidOperationException>(() => deck.Draw(1, new Hand()));
            Assert.AreEqual(deck.Count, 0);
        }

        [TestMethod]
        public void TestShuffleDeterministic()
        {
            var cards = new List<Card> {
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Fire),
            new RPSCardsEngine.Cards.Card(CardType.Water),
            new RPSCardsEngine.Cards.Card(CardType.Wood), };

            int seed = 4051996;
            var random1 = new Random(seed);
            var random2 = new Random(seed);
            var deck1 = new Deck(cards);
            var deck2 = new Deck(cards);
            var hand1 = new Hand(9, new Graveyard());
            var hand2 = new Hand(9, new Graveyard());


            deck1.Shuffle(random1);
            deck1.Draw(9, hand1);
            deck2.Shuffle(random2);
            deck2.Draw(9, hand2);

            for (int i = 0; i < 9; i++)
                Assert.AreEqual(hand1[i], hand2[i]);
        }
    }
}
