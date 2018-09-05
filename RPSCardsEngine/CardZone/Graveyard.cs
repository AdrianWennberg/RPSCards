using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.API.CardZone;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsEngine.CardZone
{
    public class Graveyard : IGraveyard
    {
        readonly Stack<ICard> _cards = new Stack<ICard>();

        public int Count => _cards.Count;

        public Zone Zone => Zone.Graveyard;

        public void MoveAllIntoDeck(IDeck deck)
        {
            if (deck.Count != 0)
                throw new InvalidOperationException("Can only move into deck if deck is empty");

            deck.AddCards(_cards);
            _cards.Clear();
        }

        public void AddCard(ICard card) => throw new NotImplementedException();
    }
}
