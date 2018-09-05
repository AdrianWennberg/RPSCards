using System;

namespace RPSCardsEngine.Cards
{
    public class Board : CardZone
    {
        private readonly Card[] _cards = new Card[Constants.BOARD_SIZE];
        private int _count = 0;
        private readonly Graveyard _graveyard;

        public Card this[int i] => _cards[i] ?? throw new ArgumentException($"No card {i} on board");
        public override int Count => _count;
        public override Zone Zone => Zone.Board;
        public int Capacity => _cards.Length;
        public bool HasCard(int i) => _cards[i] != null;

        public Board() : this(new Graveyard()) { }

        public Board(Graveyard graveyard) => _graveyard = graveyard;

        public void Fight(Board otherBoard)
        {
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                this[i].Fight(otherBoard[i]);
            }
        }

        public void RemoveDestroyed()
        {
            for (int i = 0; i < Capacity; i++)
            {
                if (HasCard(i) && this[i].IsDestroyed)
                    MoveCardTo(i, _graveyard, 0);
            }
        }

        internal override bool IsValidIndex(int index) => 0 <= index && index < _cards.Length;

        internal override bool CanPutCard(int pos) => TryIndex(pos) && _cards[pos] == null;
        internal override bool CanRemoveCard(int pos) => TryIndex(pos) && _cards[pos] != null;

        internal override void PutCard(int pos, Card card)
        {
            if (CanPutCard(pos) == false)
            {
                throw new ArgumentException($"Cannot put a card in position {pos} of {GetType().Name}");
            }
            _count++;
            _cards[pos] = card ?? throw new ArgumentNullException($"Card cannot be null");
        }

        internal override Card RemoveCard(int pos)
        {
            if (CanRemoveCard(pos) == false)
            {
                throw new ArgumentException($"Cannot remove a card from position {pos} of {GetType().Name}");
            }

            _count--;
            Card card = _cards[pos];
            _cards[pos] = null;
            return card;
        }

        internal void AddElement(int targetPos, Card element)
        {
            if (element == null)
                throw new ArgumentNullException("Element cannot be null");

            if (HasCard(targetPos) == false)
                throw new ArgumentException($"Board has no card in slot {targetPos}");

            _cards[targetPos].AddElement(element);
        }
    }
}
