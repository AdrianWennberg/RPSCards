using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsEngine.API.CardZone
{
    public interface ICardZone
    {
        int Count { get; }
        Zone Zone { get; }
    }
}
