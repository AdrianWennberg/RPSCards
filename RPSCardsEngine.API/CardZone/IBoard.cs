using RPSCardsEngine.API.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsEngine.API.CardZone
{
    public interface IBoard : ICardZone
    {
        ICard this[int i] { get; }
    }
}
