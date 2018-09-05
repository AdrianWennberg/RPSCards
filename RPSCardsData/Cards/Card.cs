using System;
using RPSCardsEngine.API;

namespace RPSCardsEngine.Cards
{
    public class Card
    {
        private readonly CardType _baseType;
        private CardType _element;

        public CardType Type { get; private set; }
        public Zone Zone { get; private set; }
        public int Position { get; private set; }
        public bool IsDestroyed { get; private set; }

        public Card(CardType type)
        {
            if (CardTypes.BaseUnits.Contains(type))
            {
                _baseType = type;
                _element = CardType.None;
            }
            else if (CardTypes.Elements.Contains(type))
            {
                _element = type;
                _baseType = CardType.None;
            }
            else
                throw new IllegalArgumentException($"Cannot create a unit of type {type}");

            Type = type;
            IsDestroyed = false;
        }

        public Card(CardType type, Zone zone, int position) : this(type)
        {
            Zone = zone;
            Position = position;
        }

        public void AddElement(Card element)
        {
            CheckIfDestroyed();

            if (Zone != Zone.Board)
                throw new InvalidOperationException($"Can only add elements to cards on the board");

            if (CardTypes.Elements.Contains(element.Type) == false)
                throw new ArgumentException($"{element} is not an element");

            if (CardTypes.Elements.Contains(Type))
                throw new InvalidOperationException($"Cannot add an element to a {Type} card");

            if (element.Destroys(this)) IsDestroyed = true;
            else if (CardTypes.ElementUpgrades[element.Type].Contains(Type))
            {
                Type += (int)element.Type;
                _element = element.Type;
            }
        }

        public void Fight(Card other)
        {
            if (Zone != Zone.Board || other.Zone != Zone.Board ||
                CardTypes.Elements.Contains(other.Type) ||
                CardTypes.Elements.Contains(Type))
                throw new InvalidOperationException($"Only cards on the board can fight");
            
            if (Destroys(other))
                other.IsDestroyed = true;
            else if (other.Destroys(this))
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

        private bool Destroys(Card other)
        {
            if (CardTypes.Elements.Contains(other.Type))
                throw new ArgumentException("Cannot destroy an element");

            return CardTypes.Elements.Contains(Type) ? CardTypes.ElementDestroys[_element].Contains(other.Type)
                : CardTypes.BaseUnitDestroys[_baseType].Contains(other.Type) ||
                (CardTypes.BaseUnits.Contains(Type) == false && 
                    (CardTypes.ElementDestroys[_element].Contains(other.Type) ||
                    (other._element == CardType.None &&
                        other._baseType == _baseType)));
        }

        private void CheckIfDestroyed()
        {
            if (IsDestroyed)
            {
                throw new InvalidOperationException($"{Type} is destroyed");
            }
        }
        
        public override string ToString()
            => $"{Type}, {Zone}, {Position}" + (IsDestroyed ? ", destroyed" : "");
    }
}
