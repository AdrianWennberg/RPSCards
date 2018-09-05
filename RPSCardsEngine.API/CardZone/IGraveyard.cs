using RPSCardsEngine.API.Cards;

namespace RPSCardsEngine.API.CardZone
{
    public interface IGraveyard : ICardZone
    {
        void AddCard(ICard card);
    }
}
