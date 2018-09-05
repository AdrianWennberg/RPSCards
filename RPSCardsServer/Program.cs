using System;

namespace RPSCardsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerTCP.Init();
            Console.ReadLine();

            //new GameSlow("a", "b");
            //Console.ReadLine();
        }
    }
}
