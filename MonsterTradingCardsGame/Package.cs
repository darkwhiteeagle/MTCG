using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardsGame
{
    class Package
    {
        public Package() { }

        public static void BuyPackage()
        {
            char chr;
            Console.WriteLine("One package includes 4 random cards and costs 5 coins.\n" +
                "Are you sur you want to purchase it? (y/n)");
            chr = Console.ReadKey().KeyChar;
            if (chr == 'y')
            {
                Database.GetConn().GetPackage();
                Console.WriteLine("Your Deck after purchaising a package:");
                Stack.PrintStack();
            }

        }
    }
}
