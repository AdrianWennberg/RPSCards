using RPSCardsEngine.API;
using RPSCardsEngine.API.Cards;
using RPSCardsEngine.API.CardZone;
using System;

namespace RPSCardsEngine.CardZone
{
    public class Hand : IHand
    {
        readonly ICard[] _cards;
        readonly IGraveyard _graveyard;

        public ICard this[int i] => _cards[i] ?? throw new ArgumentException($"No card {i} in hand");
        public int Count { get; private set; } = 0;
        public Zone Zone => Zone.Hand;
        public int Capacity => _cards.Length;
        public bool HasCard(int i) => _cards[i] != null;

        public Hand(int size, Graveyard graveyard)
        {
            _cards = new ICard[size];
            _graveyard = graveyard;
        }

        public void PlayCard(int cardNum, Board board, int targetPos)
        {
            throw new NotImplementedException();
            //if (HasCard(cardNum) == false)
            //    throw new ArgumentException($"No card {cardNum} in hand");

            //if (CardTypeHelpers.IsUnit(_cards[cardNum].Type))
            //{
            //    if (board.HasCard(targetPos))
            //        throw new ArgumentException($"Allrready a unit in {targetPos}");

            //    MoveCardTo(cardNum, board, targetPos);
            //}
            //else
            //{
            //    if (board.HasCard(targetPos))
            //        board.AddElement(targetPos, _cards[cardNum]);
            //    MoveCardTo(cardNum, _graveyard, 0);
            //}

        }

        public void AddCard(ICard card, int i) => throw new NotImplementedException();
    }
}