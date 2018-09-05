using System;
using System.Collections.Generic;
using System.Text;
using RPSCardsEngine.API.Cards;

namespace RPSCardsEngine.API.CardZone
{
    public interface IHand : ICardZone
    {
        int Capacity { get; }
        void AddCard(ICard card, int i);
    }
}
