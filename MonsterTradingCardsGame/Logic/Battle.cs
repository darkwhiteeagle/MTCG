using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace MonsterTradingCardsGame {
    class Battle {

        public List<Card> userDeck = new List<Card>(new Card[4]);
        public List<Card> botDeck = new List<Card>(new Card[4]);
        private int deckSize = 4;
        private int maxRounds = 100;

        private Card.CARD_TYPE monster = Card.CARD_TYPE.Monster;
        private Card.CARD_TYPE spell = Card.CARD_TYPE.Spell;
        private Card.ELEMENT_TYPE normal = Card.ELEMENT_TYPE.Normal;
        private Card.ELEMENT_TYPE water = Card.ELEMENT_TYPE.Water;
        private Card.ELEMENT_TYPE fire = Card.ELEMENT_TYPE.Fire;
        ~Battle() {
            userDeck.Clear();
            botDeck.Clear();
        }
        public Battle() {
            //Bot choosing random Cards for Component
            for (int i = 0; i < deckSize; i++)
            {
                int choiceBot = RandomCard(9);
                while (botDeck.Contains(Stack.cardList[choiceBot]))
                    choiceBot = RandomCard(9);

                botDeck[i] = Stack.cardList[choiceBot];
            }
        }
        private void ChooseCards() {
            int choice;
            string[] count = new string[] { "first", "second", "third", "fourth" };
            for (int i = 0; i < deckSize; i++)
            {
                Console.WriteLine($"\nPlease choose your {count[i]} Card");
                string input = Console.ReadLine(); //   || choice < 0 || choice >= Stack.stackSize
                while (!Int32.TryParse(input, out choice) || !Stack.userCards.Contains(Stack.cardList.Find(x => x.id == choice)))
                {
                    Console.WriteLine("Please choose a valid Cardnumber!", Color.Red);
                    input = Console.ReadLine();
                }
                userDeck[i] = Stack.userCards.Find(x => x.id == choice);
                Stack.userCards.Remove(userDeck[i]);
            }
        }
        public void PrintDecks() {
            Console.WriteLine("\nYour Deck:");
            foreach (Card card in userDeck)
            {
                card.PrintCard();
            }
            Console.WriteLine("\nComputer's Deck:");

            foreach (Card card in botDeck)
            {
                card.PrintCard();
            }
        }

        public void StartBattle() {
            if (Stack.userCards.Count > 3)
            {
                Stack.PrintStack();
                ChooseCards();
                PrintDecks();
                Console.WriteLine();
                for (int i = 0; i <= maxRounds; i++)             //maxRound
                {
                    int x = RandomCard(userDeck.Count);
                    int y = RandomCard(botDeck.Count);
                    Console.WriteLine($"Round {i}:", Color.DarkGray);
                    Console.WriteLine($"{userDeck[x].name}({userDeck[x].damage}) vs {botDeck[y].name}({botDeck[y].damage}) ");
                    Specialties(x, y);
                    if (CountCards(i)) break;
                }
                PrintDecks();
                Database.GetConn().UpdatePlayedGames();
            }
            else
            {
                Console.WriteLine("You have to have at least 4 cards to play", Color.DarkRed);
            }
        }
        private void Specialties(int x, int y) {
            if ((userDeck[x].name == "Goblin" && botDeck[y].name == "Dragon") ||
                (userDeck[x].name == "Dragon" && botDeck[y].name == "Goblin"))
            {
                Console.WriteLine("Goblins are too afraid of Dragons to attack", Color.Yellow);
            }
            else if ((userDeck[x].name == "Ork" && botDeck[y].name == "Wizzard") ||
                (userDeck[x].name == "Wizzard" && botDeck[y].name == "Ork"))
            {
                Console.WriteLine("Wizzard can control Orks so they are not able to damage them", Color.Yellow);
            }
            else if ((userDeck[x].name == "WaterSpell" && botDeck[y].name == "Knight") ||
                (userDeck[x].name == "Knight" && botDeck[y].name == "WaterSpell"))
            {
                userDeck.Add(botDeck[y]);
                botDeck.Remove(botDeck[y]);
                Console.WriteLine("The armor of Knights is so heavy that WaterSpells make them drown them instantly", Color.Yellow);
            }
            else if ((userDeck[x].ctype.Equals(spell) && botDeck[y].name == "Kraken") ||
                (botDeck[y].ctype.Equals(spell) && userDeck[x].name == "Kraken"))
            {
                Console.WriteLine("The Kraken is immune against spells", Color.Yellow);
            }
            else if ((userDeck[x].name == "Dragon" && botDeck[y].name == "FireElve") ||
                (userDeck[x].name == "FireElve" && botDeck[y].name == "Dragon"))
            {
                Console.WriteLine("The FireElves know Dragons since they were little and can evade their attacks", Color.Yellow);
            }
            else
            {
                if (userDeck[x].ctype.Equals(monster) && botDeck[y].ctype.Equals(monster))
                    CompareDamage(x, y, userDeck[x].damage, botDeck[y].damage);
                else
                    CompareElemntType(x, y);
            }
        }
        private void CompareDamage(int x, int y, int xDamage, int yDamage) {
            Console.WriteLine($"{xDamage} VS {yDamage}");
            if (xDamage > yDamage)
            {
                Database.GetConn().InsertCard(botDeck[y].id);
                userDeck.Add(botDeck[y]);
                botDeck.Remove(botDeck[y]);
                Console.WriteLine("You won this round : Card added to your Deck");
            }
            else if (xDamage < yDamage)
            {
                Database.GetConn().DeleteCard(userDeck[x].id, User.username);
                botDeck.Add(userDeck[x]);
                userDeck.Remove(userDeck[x]);
                Console.WriteLine("You lost this round : Card transfered to Opponent");
            }
            else
            {
                Console.WriteLine("Both Card Damage are equal : No changes");
            }
        }
        private void CompareElemntType(int x, int y) {
            int[] arr = new int[2];

            //water VS fire, fire VS normal, normal VS water
            if ((userDeck[x].etype.Equals(water) && botDeck[y].etype.Equals(fire)) ||
                (userDeck[x].etype.Equals(fire) && botDeck[y].etype.Equals(normal)) ||
                (userDeck[x].etype.Equals(normal) && botDeck[y].etype.Equals(water)))
            {
                arr = DoubleDamage(x, y);
            }
            //fire VS water, normal VS fire, water VS normal
            if ((userDeck[x].etype.Equals(fire) && botDeck[y].etype.Equals(water)) ||
                (userDeck[x].etype.Equals(normal) && botDeck[y].etype.Equals(fire)) ||
                (userDeck[x].etype.Equals(water) && botDeck[y].etype.Equals(normal)))
            {
                arr = HalfDamage(x, y);
            }
            CompareDamage(x, y, arr[0], arr[1]);
        }

        private int[] HalfDamage(int x, int y)         //water VS fire, fire VS normal, normal VS water
        {
            int[] xy = new int[2];
            Console.WriteLine("Damage halfed!");
            double var = userDeck[x].damage / 2;
            xy[0] = (int)Math.Round(var, MidpointRounding.AwayFromZero);
            var = botDeck[y].damage * 2;
            xy[1] = (int)Math.Round(var, MidpointRounding.AwayFromZero);
            return xy;
        }
        private int[] DoubleDamage(int x, int y)         //fire VS water, normal VS fire, water VS normal
        {
            int[] xy = new int[2];
            Console.WriteLine("Damaged doubled!");
            double var = userDeck[x].damage * 2;
            xy[0] = (int)Math.Round(var, MidpointRounding.AwayFromZero);
            var = botDeck[y].damage / 2;
            xy[1] = (int)Math.Round(var, MidpointRounding.AwayFromZero);
            return xy;
        }

        private int RandomCard(int curSize) {
            Random rnd = new Random();
            return rnd.Next(0, curSize);
        }

        private bool CountCards(int i) {
            if (((userDeck.Count < botDeck.Count) && i > 99) || userDeck.Count == 0)
            {
                Console.WriteLine("\nYou lose this game", Color.Red);
                Database.GetConn().UpdateElo(-5);
                return true;
            }
            if ((userDeck.Count == botDeck.Count) && i > 99)
            {
                Console.WriteLine("\nThis round is a tie", Color.DarkOrange);
                return true;
            }
            if (((userDeck.Count > botDeck.Count) && i > 99) || botDeck.Count == 0)
            {
                Console.WriteLine("\nYou won this game", Color.Green);
                Database.GetConn().UpdateElo(3);
                return true;
            }
            return false;
        }
    }
}
