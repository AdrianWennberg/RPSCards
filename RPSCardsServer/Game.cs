using System;
using System.Collections.Generic;
using NetworkDataTypes;
using RPSCardsEngine.Cards;

namespace RPSCardsServer
{
    class Game : IGame
    {
        readonly Random _rnd=new Random();
        readonly Player[] _players = new Player[2];



        public Game(string playerOneName, string playerTwoName) =>
            // Set up decks and player structure.
            Setup(playerOneName, playerTwoName);

        void Setup(string playerOneName, string playerTwoName){
        }
    }
}

// TODO: Need some version of card and player in player itself. Need to be able to read data from card especially.