using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.API.CardZone;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPSCardsEngine.CardZone
{
    public class Deck : IDeck
    {
        readonly Queue<ICard> _cards;

        public int Count => _cards.Count;
        public Zone Zone => Zone.Deck;

        public Deck() => _cards = new Queue<ICard>();

        public Deck(List<ICard> cards)
        {
            _cards = new Queue<ICard>(cards);
            int i = 0;
            cards.ToList().ForEach(card => card.ChangeZone(Zone, i++));
        }

        public void Draw(int amount, IHand hand)
        {
            if (_cards.Count() < amount)
                throw new InvalidOperationException($"Only {_cards.Count()} cards left, trying to draw {amount}");
            
            if (hand.Count != 0)
                throw new InvalidOperationException($"Hand is not empty when drawing");

            if (hand.Capacity < amount)
                throw new InvalidOperationException($"Hand can only have up to {hand.Capacity} cards, trying to draw {amount}");

            for (int i = 0; i < amount; i++)
            {
                hand.AddCard(_cards.Dequeue(), i);
            }
        }

        public void Shuffle(Random rnd) => _cards.OrderBy(card => rnd.Next());
        public void AddCards(IEnumerable<ICard> cards) => throw new NotImplementedException();
    }
}
