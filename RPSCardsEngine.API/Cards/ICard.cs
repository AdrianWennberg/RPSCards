using RPSCardsEngine.API.CardZone;

namespace RPSCardsEngine.API.Cards
{
    public interface ICard
    {
        CardType Type { get; }
        Zone Zone { get;  }
        int Position { get; }
        bool IsDestroyed { get; }
        
        void AddElement(ICard element);
        void GetAttackedBy(ICard other);
        void ChangeZone(Zone zone, int position);
    }
}
