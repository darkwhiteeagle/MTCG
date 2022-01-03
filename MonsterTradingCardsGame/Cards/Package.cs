using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardsGame {
    class Package {
        public Package() { }
        //Purchase 4 random cards for 5 coins 
        public static void BuyPackage() {
            char chr;
            Console.WriteLine("One package includes 4 random cards and costs 5 coins.\n" +
                "Are you sure you want to purchase it? (y/n)");
            chr = Console.ReadKey().KeyChar;
            if (chr == 'y')
            {
                Database.GetConn().GetPackage();
                Console.WriteLine("Four Cards was added to your deck.\nYour deck after purchaising a package:");
                Database.GetConn().GetUserCards();
                Stack.PrintStack();
            }

        }
    }
}
