using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardsGame
{
    class Trade
    {
        public static List<Card> tradeList = new List<Card>();
        public static List<Tuple<int, string>> tradeInfo = new List<Tuple<int, string>>();

        public Trade() { }
        ~Trade()
        {
            tradeList.Clear();
            tradeInfo.Clear();
        }

        public static void TradeMenu()
        {
            string input;
            Console.WriteLine("Do you want to buy or sell a card (1 or 2)\n1.Buy\n2.Sell");
            input = Console.ReadLine();
            if (input == "1")
                TradeBuy();
            if (input == "2")
                TradeSell();
        }
        private static void TradeBuy()
        {
            int input;
            Database.GetConn().GetTradeCards();
            Console.WriteLine("The following cards are for sale");
            for (int i = 0; i < tradeList.Count; i++)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine($"#Card No. {i}");
                tradeList[i].PrintCard();
                Console.WriteLine($"costs {tradeInfo[i].Item1} coins from {tradeInfo[i].Item2}\n");
            }
            Console.WriteLine("\nType in the Card No. you want to buy:");
            input = Convert.ToInt32(Console.ReadLine());
            if (input <= tradeList.Count && input >= 0)
            {
                if (tradeInfo[input].Item1 <= User.coins)
                    Database.GetConn().BuyCard(tradeList[input].id, tradeInfo[input].Item1, tradeInfo[input].Item2);
                else
                    Console.WriteLine("You dont have enough coins!");
            }
            tradeList.Clear();
            tradeInfo.Clear();
        }
        private static void TradeSell()
        {
            int card, coins;
            Stack.PrintStack();
            Console.WriteLine("\nType in the Card ID you want to sell:");
            card = Convert.ToInt32(Console.ReadLine());

            if (Stack.userCards.Exists(x => x.id == card))
            {
                Console.WriteLine("How many coins you want for it?");
                coins = Convert.ToInt32(Console.ReadLine());
                if (coins < 100 && coins > 0)
                {
                    Database.GetConn().SellCard(card, coins);
                    Console.WriteLine("Your card has been put up for sale");
                }
                else
                {
                    Console.WriteLine("Please keep it between 0 and 100 coins");
                }
            }
        }
    }
}
