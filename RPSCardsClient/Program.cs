using System;

namespace RPSCardsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientTCP.ConnectToServer();
            Console.WriteLine("Closing...");
            Console.ReadLine();
        }
    }
}
