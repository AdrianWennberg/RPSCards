using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsEngine.Cards
{
    public class Graveyard : CardZone
    {
        readonly Stack<Card> _cards = new Stack<Card>();

        public override int Count => _cards.Count;

        public override Zone Zone => Zone.Graveyard;

        public void MoveAllIntoDeck(Deck deck)
        {
            if (deck.Count != 0)
                throw new InvalidOperationException("Can only move into deck if deck is empty");

            while (CanRemoveCard(0))
            {
                MoveCardTo(0, deck, 0);
            }
        }

        internal override bool CanPutCard(int pos) => pos == 0;

        internal override bool CanRemoveCard(int pos) => pos == 0 && Count > 0;

        internal override void PutCard(int pos, Card card)
        {
            if (CanPutCard(pos) == false)
            {
                throw new ArgumentException("Can only put a card at the top of a graveyard");
            }

            _cards.Push(card);
        }

        internal override Card RemoveCard(int pos) => 
            CanRemoveCard(pos) ? _cards.Pop() : 
            throw new ArgumentException("Can only take a card from the top of a graveyard");


        internal override bool IsValidIndex(int index) => index == 0;
    }
}
