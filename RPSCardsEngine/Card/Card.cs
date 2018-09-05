using System;
using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.API.CardZone;
using RPSCardsEngine.Helpers;

namespace RPSCardsEngine.Cards
{
    public class Card : ICard
    {
        private readonly CardType _baseType;
        private CardType _element;

        public CardType Type { get; private set; }
        public Zone Zone { get; private set; }
        public int Position { get; private set; }
        public bool IsDestroyed { get; private set; }

        private static ICardTypeHelper cardTypeHelper;
        public static void SetCardTypeHelper(ICardTypeHelper helper) => 
            cardTypeHelper = helper;

        public Card(CardType type)
        {
            if (CardTypeHelper.IsBaseUnit(type))
            {
                _baseType = type;
                _element = CardType.None;
            }
            else if (CardTypeHelper.IsElement(type))
            {
                _element = type;
                _baseType = CardType.None;
            }
            else
                throw new ArgumentException($"Cannot create a unit of type {type}");

            Type = type;
            IsDestroyed = false;
        }

        public Card(CardType type, Zone zone, int position) : this(type)
        {
            Zone = zone;
            Position = position;
        }


        public void AddElement(ICard element)
        {
            CheckIfDestroyed();

            if (Zone != Zone.Board)
                throw new InvalidOperationException($"Can only add elements to cards on the board");

            if (CardTypeHelper.IsElement(element.Type) == false)
                throw new ArgumentException($"{element} is not an element");

            if (CardTypeHelper.IsElement(Type))
                throw new InvalidOperationException($"Cannot add an element to a {Type} card");

            if (CardTypeHelper.DoesElementDestroy(element.Type, Type)) IsDestroyed = true;
            else if (CardTypeHelper.DoesElementUpgrade(element.Type,Type))
            {
                Type += (int)element.Type;
                _element = element.Type;
            }
        }

        public void GetAttackedBy(ICard other)
        {
            if (Zone != Zone.Board || other.Zone != Zone.Board ||
                CardTypeHelper.IsElement(other.Type) ||
                CardTypeHelper.IsElement(Type))
                throw new InvalidOperationException($"Only cards on the board can fight");
            
            else if (CardTypeHelper.DoesUnitDestroy(other.Type, Type))
                IsDestroyed = true;
        }

        public void ChangeZone(Zone zone, int position)
        {
            if (Zone == Zone.Board && zone == Zone.Graveyard && IsDestroyed)
                IsDestroyed = false;

            CheckIfDestroyed();
            Zone = zone;
            Position = position;
        }

        private void CheckIfDestroyed()
        {
            if (IsDestroyed)
                throw new InvalidOperationException($"{Type} is destroyed");
        }
    }
}
