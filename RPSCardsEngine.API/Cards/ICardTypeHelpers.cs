using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsEngine.API.Cards
{
    public interface ICardTypeHelper
    {
        bool IsElement(CardType type);
        bool IsUnit(CardType type);
        bool IsBaseUnit(CardType type);
        bool IsUpgradedUnit(CardType type);
        bool DoesElementDestroy(CardType element, CardType type);
        bool DoesElementUpgrade(CardType element, CardType type);
        bool DoesUnitDestroy(CardType attacker, CardType type);
        CardType GetBaseUnit(CardType type);
        CardType GetUnitElement(CardType type);
    }
}
