using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.API.CardZone;
using System;

namespace RPSCardsEngine.CardZone
{
    public class Board : IBoard
    {
        private readonly ICard[] _cards = new ICard[Constants.BOARD_SIZE];
        private readonly IGraveyard _graveyard;

        public ICard this[int i] => _cards[i] ?? throw new ArgumentException($"No card {i} on board");
        public int Count { get; private set; } = 0;
        public Zone Zone => Zone.Board;
        public int Capacity => _cards.Length;
        public bool HasCard(int i) => _cards[i] != null;
        

        public Board(IGraveyard graveyard) => _graveyard = graveyard;

        public void Fight(IBoard otherBoard)
        {
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                this[i].GetAttackedBy(otherBoard[i]);
                otherBoard[i].GetAttackedBy(this[i]);
            }
        }

        public void RemoveDestroyed()
        {
            for (int i = 0; i < Capacity; i++)
            {
                if (HasCard(i) && _cards[i].IsDestroyed)
                {
                    _graveyard.AddCard(_cards[i]);
                    _cards[i] = null;
                }
            }
        }
        internal void AddElement(int targetPos, ICard element)
        {
            if (element == null)
                throw new ArgumentNullException("Element cannot be null");

            if (HasCard(targetPos) == false)
                throw new ArgumentException($"Board has no card in slot {targetPos}");

            _cards[targetPos].AddElement(element);
        }
    }
}
