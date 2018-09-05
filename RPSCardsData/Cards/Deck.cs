using System;
using System.Collections.Generic;
using System.Linq;

namespace RPSCardsEngine.Cards
{
    public class Deck : CardZone
    {
        readonly Queue<Card> _cards;

        public static Deck BasicDeck => new Deck(new List<Card> {
            new Card(CardType.Rock),
            new Card(CardType.Scissor),
            new Card(CardType.Paper),
            new Card(CardType.Rock),
            new Card(CardType.Scissor),
            new Card(CardType.Paper),
            new Card(CardType.Fire),
            new Card(CardType.Water),
            new Card(CardType.Wood), });

        public override int Count => _cards.Count;
        public override Zone Zone => Zone.Deck;

        public Deck() => _cards = new Queue<Card>();

        public Deck(List<Card> cards)
        {
            _cards = new Queue<Card>(cards);
            int i = 0;
            cards.ToList().ForEach(card => card.ChangeZone(Zone, i++));
        }

        public void Draw(int amount, Hand hand)
        {
            if (_cards.Count() < amount)
                throw new InvalidOperationException($"Only {_cards.Count()} cards left, trying to draw {amount}");
            
            if (hand.Count != 0)
                throw new InvalidOperationException($"Hand is not empty when drawing");

            if (hand.Capacity < amount)
                throw new InvalidOperationException($"Hand can only have up to {hand.Capacity} cards, trying to draw {amount}");

            for (int i = 0; i < amount; i++)
            {
                MoveCardTo(0, hand, i);
            }
        }

        public void Shuffle(Random rnd) => _cards.OrderBy(card => rnd.Next());
        
        internal override bool CanPutCard(int pos) => pos == 0;

        internal override bool CanRemoveCard(int pos) => pos == 0;
        
        internal override void PutCard(int pos, Card card)
        {
            if (CanPutCard(pos) == false)
            {
                throw new ArgumentException("Can only put a card at the top of a graveyard");
            }

            _cards.Enqueue(card);
        }

        internal override Card RemoveCard(int pos) => 
            pos == 0 ? _cards.Dequeue() :
            throw new ArgumentException("Can only remove first card of deck");

        internal override bool IsValidIndex(int index) => index == 0;
    }
}
