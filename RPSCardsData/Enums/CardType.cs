using System.Collections.Generic;

namespace RPSCardsEngine
{
    public class CardTypes
    {
        public static HashSet<CardType> Elements { get; } = 
            new HashSet<CardType>(new CardType[] 
            {
                CardType.Fire,
                CardType.Water,
                CardType.Wood,
            });

        public static HashSet<CardType> Units { get; } = new HashSet<CardType>(new CardType[] 
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

        public static HashSet<CardType> BaseUnits { get; } = new HashSet<CardType>(new CardType[] 
        {
            CardType.Rock,
            CardType.Paper,
            CardType.Scissor,
        });

        public static Dictionary<CardType,HashSet<CardType>> ElementUpgrades { get; } = new Dictionary<CardType, HashSet<CardType>>{
            { CardType.Fire, new HashSet<CardType>(new CardType[]{CardType.Rock, CardType.Scissor }) },
            { CardType.Water, new HashSet<CardType>(new CardType[]{CardType.Paper, CardType.Scissor }) },
            { CardType.Wood, new HashSet<CardType>(new CardType[]{CardType.Rock, CardType.Paper }) },
        };

        public static Dictionary<CardType, HashSet<CardType>> ElementDestroys { get; } = new Dictionary<CardType, HashSet<CardType>>{
            { CardType.Fire, new HashSet<CardType>(new CardType[]{CardType.Paper, CardType.Axe, CardType.Tree }) },
            { CardType.Water, new HashSet<CardType>(new CardType[]{CardType.Rock, CardType.Magma, CardType.Laser }) },
            { CardType.Wood, new HashSet<CardType>(new CardType[]{CardType.Scissor, CardType.Glass, CardType.WaterGun }) },
        };

        public static Dictionary<CardType, CardType> UnitToBaseUnit { get; } = new Dictionary<CardType, CardType>{
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

        public static Dictionary<CardType, CardType> UnitElement { get; } = new Dictionary<CardType, CardType>{
            { CardType.Magma, CardType.Fire },
            { CardType.Laser, CardType.Fire },
            { CardType.Glass, CardType.Water },
            { CardType.WaterGun, CardType.Water },
            { CardType.Axe, CardType.Wood },
            { CardType.Tree, CardType.Wood },
        };

        public static Dictionary<CardType, HashSet<CardType>> BaseUnitDestroys = new Dictionary<CardType, HashSet<CardType>>
        {
            { CardType.Rock, new HashSet<CardType>{CardType.Scissor, CardType.WaterGun, CardType.Laser } },
            { CardType.Paper, new HashSet<CardType>{CardType.Rock, CardType.Magma, CardType.Axe } },
            { CardType.Scissor, new HashSet<CardType>{CardType.Paper, CardType.Glass, CardType.Tree } },
        };
    }
}
