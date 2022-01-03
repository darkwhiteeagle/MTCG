using System;
using System.Collections.Generic;
using System.Text;

using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace MonsterTradingCardsGame {
    class Trade {
        public static List<Card> tradeList = new List<Card>();
        public static List<Tuple<int, string>> tradeInfo = new List<Tuple<int, string>>();

        public Trade() { }
        ~Trade() {
            tradeList.Clear();
            tradeInfo.Clear();
        }
        //User can buy, sell or go back
        public static void TradeMenu() {
            int input;
            Console.WriteLine("Do you want to buy or sell a card (1 or 2)\n1.Buy\n2.Sell\n3.Return");
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:
                    TradeBuy();
                    break;
                case 2:
                    TradeSell();
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
        //The cards on the market will be listed and user can choose one
        private static void TradeBuy() {
            string input;
            int num;
            Database.GetConn().GetTradeCards();
            if (tradeList.Count > 0)
            {
                Console.WriteLine("The following cards are for sale");
                for (int i = 0; i < tradeList.Count; i++)
                {
                    Console.WriteLine("=======================================");
                    Console.WriteLine($"#Card No. {i}");
                    tradeList[i].PrintCard();
                    Console.WriteLine($"costs {tradeInfo[i].Item1} coins from {tradeInfo[i].Item2}");
                }
                Console.WriteLine("\nType in the Card No. you want to buy, or just press enter to go back");
                input = Console.ReadLine();

                if (int.TryParse(input, out num) && num <= tradeList.Count && num >= 0)
                {
                    if (tradeInfo[num].Item1 <= User.coins)
                    {
                        Database.GetConn().BuyCard(tradeList[num].id, tradeInfo[num].Item1, tradeInfo[num].Item2);
                        Console.WriteLine("Card bought successfully!", Color.DarkGreen);
                    }
                    else
                    {
                        Console.WriteLine("You dont have enough coins!", Color.DarkRed);
                    }
                }
                else
                {
                    Console.WriteLine("Nothing bought!");
                }
            }
            else
            {
                Console.WriteLine("Nothing to buy!", Color.DarkRed);
            }
            tradeList.Clear();
            tradeInfo.Clear();
        }
        //User can choose a card from his deck and offer it for some coins
        private static void TradeSell() {
            string input;
            int card, coins;
            if (Stack.userCards.Count > 0)
            {
                Stack.PrintStack();
                Console.WriteLine("\nType in the Card ID you want to sell, or just press enter to go back");
                input = Console.ReadLine();

                if (int.TryParse(input, out card) && Stack.userCards.Exists(x => x.id == card))
                {
                    Console.WriteLine("How many coins you want for it?");
                    coins = Convert.ToInt32(Console.ReadLine());
                    if (coins < 100 && coins > 0)
                    {
                        Database.GetConn().SellCard(card, coins);
                        Console.WriteLine("Your card has been put up for sale", Color.DarkGreen);
                    }
                    else
                    {
                        Console.WriteLine("Please keep it between 0 and 100 coins");
                    }
                }
            }
            else
            {
                Console.WriteLine("You don't have any cards to sell!", Color.DarkRed);
            }
        }
    }
}
