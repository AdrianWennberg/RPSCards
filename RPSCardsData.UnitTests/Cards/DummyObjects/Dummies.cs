using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPSCardsEngine.UnitTests.Cards.DummyObjects
{
    static class TestHelpers
    {
        public static List<RPSCardsEngine.Cards.Card> BaseUnitDeck => new List<Card>{
            new RPSCardsEngine.Cards.Card(CardType.Rock),
            new RPSCardsEngine.Cards.Card(CardType.Paper),
            new RPSCardsEngine.Cards.Card(CardType.Scissor),
            };

        public static List<RPSCardsEngine.Cards.Card> BaseElementDeck => new List<Card>{
            new RPSCardsEngine.Cards.Card(CardType.Fire),
            new RPSCardsEngine.Cards.Card(CardType.Water),
            new RPSCardsEngine.Cards.Card(CardType.Wood),
            };

        public static List<RPSCardsEngine.Cards.Card> UpgradedUnitDeck => GetUpgradedUnitDeck();

        public static Card CardOfType(CardType type) => CardOfType(type, Zone.None);

        public static Card CardOfType(CardType type, Zone zone)
        {
            if (CardTypeHelpers.IsElement(type))
                return new RPSCardsEngine.Cards.Card(type, zone, 0);

            var card = new RPSCardsEngine.Cards.Card(CardTypeHelpers.GetBaseUnit(type), zone, 0);
            if (CardTypeHelpers.IsBaseUnit(type) == false)
                card.AddElement(new RPSCardsEngine.Cards.Card(CardTypeHelpers.GetUnitElement(type)));
            return card;
        }

        private static List<RPSCardsEngine.Cards.Card> GetUpgradedUnitDeck()
        {
            var cards = BaseUnitDeck.Concat(BaseUnitDeck).ToList();

            cards[0].AddElement(new RPSCardsEngine.Cards.Card(CardType.Wood));
            cards[1].AddElement(new RPSCardsEngine.Cards.Card(CardType.Wood));
            cards[2].AddElement(new RPSCardsEngine.Cards.Card(CardType.Water));
            cards[3].AddElement(new RPSCardsEngine.Cards.Card(CardType.Water));
            cards[4].AddElement(new RPSCardsEngine.Cards.Card(CardType.Fire));
            cards[5].AddElement(new RPSCardsEngine.Cards.Card(CardType.Fire));

            return cards;
        }

        public static Hand GetFilledHand(List<RPSCardsEngine.Cards.Card> cards, Graveyard graveyard = null)
        {
            var hand = new Hand(cards.Count, graveyard);

            new Deck(cards).Draw(cards.Count, hand);

            return hand;
        }

        public static Board GetBasicBoard()
        {
            Hand hand = GetFilledHand(BaseUnitDeck);
            var board = new Board();

            for(int i = 0; i < hand.Capacity && i < board.Capacity; i++)
            {
                if (hand.HasCard(i))
                    hand.PlayCard(i, board, i);
            }

            return board;
        }

        public static Board GetUpgradedBoard(List<CardType> cardTypes)
        {
            Hand hand = GetFilledHand(cardTypes.Select(type => CardOfType(type, Zone.Board)).ToList());
            var board = new Board();

            for (int i = 0; i < hand.Capacity && i < board.Capacity; i++)
            {
                if (hand.HasCard(i))
                    hand.PlayCard(i, board, i);
            }

            return board;
        }

        public static Board GetFilledBoard(List<RPSCardsEngine.Cards.Card> cards)
        {
            if (cards.Count > Constants.BOARD_SIZE)
                throw new ArgumentException("an only get a board with exactly 3 cards");
             
            Hand hand = GetFilledHand(cards);

            var board = new Board();

            for (int i = 0; i < board.Capacity; i++)
            {
                if (hand.HasCard(i))
                    hand.PlayCard(i, board, i);
            }

            return board;
        }
    }
}
