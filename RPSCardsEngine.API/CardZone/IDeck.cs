using System;
using System.Collections.Generic;
using System.Text;
using RPSCardsEngine.API.Cards;

namespace RPSCardsEngine.API.CardZone
{
    public interface IDeck : ICardZone
    {
        void AddCards(IEnumerable<ICard> cards);
    }
}
