using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardsGame
{
    class Stack
    {
        public static List<Card> cardList = new List<Card>();
        public static List<Card> userCards = new List<Card>();

        public static int stackSize = 27;

        /*private Card.CARD_TYPE monster = Card.CARD_TYPE.Monster;
        private Card.CARD_TYPE spell = Card.CARD_TYPE.Spell;
        private Card.ELEMENT_TYPE normal = Card.ELEMENT_TYPE.Normal;
        private Card.ELEMENT_TYPE water = Card.ELEMENT_TYPE.Water;
        private Card.ELEMENT_TYPE fire = Card.ELEMENT_TYPE.Fire;*/

        public Stack()
        {
            /* cardList.Add(new Card(0, "RegularSpell", 20, normal, spell));
             cardList.Add(new Card(1, "WaterSpell", 20, water, spell));
             cardList.Add(new Card(2, "FireSpell", 20, fire, spell));

             cardList.Add(new Card(3, "Knight", 15, normal, monster));
             cardList.Add(new Card(4, "Goblin", 15, normal, monster));
             cardList.Add(new Card(5, "Ork", 15, normal, monster));
             cardList.Add(new Card(6, "Troll", 15, normal, monster));
             cardList.Add(new Card(7, "Dragon", 15, normal, monster));
             cardList.Add(new Card(8, "Wizzard", 15, normal, monster));
             cardList.Add(new Card(9, "Elve", 15, normal, monster));
             cardList.Add(new Card(10, "Kraken", 15, normal, monster));

             cardList.Add(new Card(11, "WaterKnight", 20, water, monster));
             cardList.Add(new Card(12, "WaterGoblin", 20, water, monster));
             cardList.Add(new Card(13, "WaterOrk", 20, water, monster));
             cardList.Add(new Card(14, "WaterTroll", 20, water, monster));
             cardList.Add(new Card(15, "WaterDragon", 20, water, monster));
             cardList.Add(new Card(16, "WaterWizzard", 20, water, monster));
             cardList.Add(new Card(17, "WaterElve", 20, water, monster));
             cardList.Add(new Card(18, "WaterKraken", 20, water, monster));

             cardList.Add(new Card(19, "FireKnight", 25, fire, monster));
             cardList.Add(new Card(20, "FireGoblin", 25, fire, monster));
             cardList.Add(new Card(21, "FireOrk", 25, fire, monster));
             cardList.Add(new Card(22, "FireTroll", 25, fire, monster));
             cardList.Add(new Card(23, "FireDragon", 25, fire, monster));
             cardList.Add(new Card(24, "FireWizzard", 25, fire, monster));
             cardList.Add(new Card(25, "FireElve", 25, fire, monster));
             cardList.Add(new Card(26, "FireKraken", 25, fire, monster));*/

        }

        public static void PrintStack()
        {
            string s = String.Format("|{0,-3}|{1,-12}|{2,-6}|{3,-7}|{4,-7}|", "ID", "Name", "Damage", "Element", "Type");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(s);
            Console.ResetColor();
            foreach (Card card in userCards)
            {
                card.PrintCard();
            }
        }

    }
}
