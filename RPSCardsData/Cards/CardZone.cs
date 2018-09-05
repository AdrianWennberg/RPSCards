using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsEngine.Cards
{
    public abstract class CardZone
    {
        public abstract int Count { get; }
        public abstract Zone Zone { get; }

        internal abstract bool CanPutCard(int pos);
        internal abstract bool CanRemoveCard(int pos);

        internal abstract void PutCard(int pos, Card card);
        internal abstract Card RemoveCard(int pos);

        internal abstract bool IsValidIndex(int index);

        internal bool TryIndex(int index) =>
            IsValidIndex(index) ? true :
            throw new ArgumentOutOfRangeException($"{index} is not a valid position in this {GetType().Name}");

        internal void MoveCardTo(int currentPos, CardZone newZone, int newPos)
        {
            TryIndex(currentPos);
            newZone.TryIndex(newPos);

            if (newZone.CanPutCard(newPos) == false)
            {
                throw new InvalidOperationException($"Cannot move a card to {newZone.GetType().Name} poitnion {newPos}.");
            }

            Card card = RemoveCard(currentPos);
            card.ChangeZone(newZone.Zone, newPos);
            newZone.PutCard(newPos, card);
        }
    }
}
