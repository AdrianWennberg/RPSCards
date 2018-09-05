using System;

namespace RPSCardsEngine.Cards
{
    public class Hand : CardZone
    {
        readonly Card[] _cards;
        readonly Graveyard _graveyard;
        private int _cardsLeft = 0;

        public Card this[int i] => _cards[i] ?? throw new ArgumentException($"No card {i} in hand");
        public override int Count => _cardsLeft;
        public override Zone Zone => Zone.Hand;
        public int Capacity => _cards.Length;
        public bool HasCard(int i) => _cards[i] != null;

        public Hand() : this(Constants.HAND_SIZE, new Graveyard()) { }

        public Hand(int size, Graveyard graveyard)
        {
            _cards = new Card[size];
            _graveyard = graveyard;
        }

        public void PlayCard(int cardNum, Board board, int targetPos)
        {
            if (HasCard(cardNum) == false)
                throw new ArgumentException($"No card {cardNum} in hand");

            if (CardTypes.Units.Contains(_cards[cardNum].Type))
            {
                if (board.HasCard(targetPos))
                    throw new ArgumentException($"Allrready a unit in {targetPos}");

                MoveCardTo(cardNum, board, targetPos);
            }
            else
            {
                if (board.HasCard(targetPos))
                    board.AddElement(targetPos, _cards[cardNum]);
                MoveCardTo(cardNum, _graveyard, 0);
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
            _cardsLeft++;
            _cards[pos] = card ?? throw new ArgumentNullException($"Card cannot be null");
        }

        internal override Card RemoveCard(int pos)
        {
            if (CanRemoveCard(pos) == false)
            {
                throw new ArgumentException($"Cannot remove a card from position {pos} of {GetType().Name}");
            }

            _cardsLeft--;
            Card card = _cards[pos];
            _cards[pos] = null;
            return card;
        }
    }
}