using RPSCardsEngine.API.Card;
using RPSCardsEngine.API.Cards;
using System;
using System.Collections.Generic;

namespace RPSCardsEngine.Helpers
{
    public class CardTypeHelper : ICardTypeHelper
    {
        private static HashSet<CardType> Elements =>
            new HashSet<CardType>(new CardType[]
            {
                CardType.Fire,
                CardType.Water,
                CardType.Wood,
            });
        private static HashSet<CardType> Units => new HashSet<CardType>(new CardType[]
        {
            CardType.Rock,
            CardType.Paper,
            CardType.Scissor,
            CardType.Magma,
            CardType.Laser,
            CardType.Glass,
            CardType.WaterGun,
            CardType.Axe,
            CardType.Tree,
        });
        private static HashSet<CardType> BaseUnits => new HashSet<CardType>(new CardType[]
        {
            CardType.Rock,
            CardType.Paper,
            CardType.Scissor,
        });
        private static Dictionary<CardType, HashSet<CardType>> ElementUpgrades => new Dictionary<CardType, HashSet<CardType>>{
            { CardType.Fire, new HashSet<CardType>(new CardType[]{CardType.Rock, CardType.Scissor }) },
            { CardType.Water, new HashSet<CardType>(new CardType[]{CardType.Paper, CardType.Scissor }) },
            { CardType.Wood, new HashSet<CardType>(new CardType[]{CardType.Rock, CardType.Paper }) },
        };
        private static Dictionary<CardType, HashSet<CardType>> ElementDestroys => new Dictionary<CardType, HashSet<CardType>>{
            { CardType.Fire, new HashSet<CardType>(new CardType[]{CardType.Paper, CardType.Axe, CardType.Tree }) },
            { CardType.Water, new HashSet<CardType>(new CardType[]{CardType.Rock, CardType.Magma, CardType.Laser }) },
            { CardType.Wood, new HashSet<CardType>(new CardType[]{CardType.Scissor, CardType.Glass, CardType.WaterGun }) },
        };
        private static Dictionary<CardType, CardType> UnitToBaseUnit => new Dictionary<CardType, CardType>{
            { CardType.Rock, CardType.Rock },
            { CardType.Paper, CardType.Paper },
            { CardType.Scissor, CardType.Scissor },
            { CardType.Magma, CardType.Rock },
            { CardType.Laser, CardType.Scissor },
            { CardType.Glass, CardType.Paper },
            { CardType.WaterGun, CardType.Scissor },
            { CardType.Axe, CardType.Rock },
            { CardType.Tree, CardType.Paper },
        };
        private static Dictionary<CardType, CardType> UnitElement => new Dictionary<CardType, CardType>{
            { CardType.Magma, CardType.Fire },
            { CardType.Laser, CardType.Fire },
            { CardType.Glass, CardType.Water },
            { CardType.WaterGun, CardType.Water },
            { CardType.Axe, CardType.Wood },
            { CardType.Tree, CardType.Wood },
        };
        private static Dictionary<CardType, HashSet<CardType>> BaseUnitDestroys => new Dictionary<CardType, HashSet<CardType>>
        {
            { CardType.Rock, new HashSet<CardType>{CardType.Scissor, CardType.WaterGun, CardType.Laser } },
            { CardType.Paper, new HashSet<CardType>{CardType.Rock, CardType.Magma, CardType.Axe } },
            { CardType.Scissor, new HashSet<CardType>{CardType.Paper, CardType.Glass, CardType.Tree } },
        };


        public bool IsElement(CardType type) => Elements.Contains(type);
        public bool IsUnit(CardType type) => Units.Contains(type);
        public bool IsBaseUnit(CardType type) => BaseUnits.Contains(type);
        public bool IsUpgradedUnit(CardType type) => IsUnit(type) && !IsBaseUnit(type);
        public bool DoesElementDestroy(CardType element, CardType type)
        {
            if (IsElement(element) == false)
                throw new ArgumentException($"{element} is not an element");

            if (IsElement(type))
                throw new ArgumentException($"{type} is an element, so it cannot be destroyed");

            return ElementDestroys[element].Contains(type);
        }
        public bool DoesUnitDestroy(CardType attacker, CardType type)
        {
            if (IsElement(attacker))
                throw new ArgumentException($"{attacker} is not a unit");

            if (IsElement(type))
                throw new ArgumentException($"{type} is an element, so it cannot be destroyed");

            return BaseUnitDestroys[attacker].Contains(type) ||
                (IsUpgradedUnit(attacker) &&
                    (DoesElementDestroy(attacker, type) ||
                    (IsBaseUnit(type) &&
                        UnitToBaseUnit[attacker] == type)));
        }
        public bool DoesElementUpgrade(CardType element, CardType type)
        {
            if (IsElement(element) == false)
                throw new ArgumentException($"{element} is not an element");

            if (IsElement(type))
                throw new ArgumentException($"{type} is an element, so it cannot be upgraded");

            return ElementUpgrades[element].Contains(type);
        }
        public CardType GetBaseUnit(CardType type)
        {
            if(IsUnit(type) == false)
                throw new ArgumentException($"{type} is not a unit");

            return UnitToBaseUnit[type];
        }
        public CardType GetUnitElement(CardType type)
        {
            if (IsUnit(type) == false)
                throw new ArgumentException($"{type} is not a unit");
            if (IsUpgradedUnit(type) == false)
                throw new ArgumentException($"{type} does not have an element");

            return UnitElement[type];
        }
    }
}
