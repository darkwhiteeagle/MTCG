using System;
using System.Collections.Generic;
using System.Text;

using Colorful;
using System.Drawing;
using Console = Colorful.Console;
namespace MonsterTradingCardsGame {
    public class Stack {
        public static List<Card> cardList = new List<Card>();
        public static List<Card> userCards = new List<Card>();

        public static int stackSize = 27;

        public Stack() { }
        //Prints all user Cards in formatted table
        public static void PrintStack() {
            if (userCards.Count > 0)
            {
                string s = String.Format("|{0,-3}|{1,-12}|{2,-6}|{3,-7}|{4,-7}|", "ID", "Name", "Damage", "Element", "Type");
                Console.WriteLine(s, Color.DarkGray);
                foreach (Card card in userCards)
                    card.PrintCard();
            }
            else
            {
                Console.WriteLine("No Cards to see here!");
            }
        }

    }
}
