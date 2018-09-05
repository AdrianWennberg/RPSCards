using RPSCardsEngine.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPSCardsServer
{
    class Player
    {
        private Client _client;
        public string Name => _client.username;
        public byte[] StartDeck { get; private set; }

        public Deck Deck { get; }
        
        public Hand Hand { get; }

        public Player(Client client, byte[] startDeck)
        {
            _client = client;
            StartDeck = startDeck;
        }
        
    }
}
