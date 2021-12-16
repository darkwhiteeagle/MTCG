using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardsGame
{
    class Card
    {
        public string name;
        public int damage { get; set; }
        public int id;
        public enum ELEMENT_TYPE { Fire, Water, Normal }
        public enum CARD_TYPE { Monster, Spell }

        public ELEMENT_TYPE etype { get; set; }
        public CARD_TYPE ctype { get; set; }

        public Card()
        {
        }
        public Card(int id, string name, int damage, ELEMENT_TYPE etype, CARD_TYPE ctype)
        {
            this.id = id;
            this.name = name;
            this.damage = damage;
            this.etype = etype;
            this.ctype = ctype;
        }

        public void PrintCard()
        {
            string printthis = String.Format("|{0,-3}|{1,-12}|{2,-3}|{3,-7}|{4,-7}|", id, name, damage, etype, ctype);
            Console.WriteLine(printthis);
        }

    }
}
