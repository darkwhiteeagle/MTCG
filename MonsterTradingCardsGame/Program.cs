using Npgsql;
using System;
using System.Collections.Generic;

using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace MonsterTradingCardsGame
{
    class Program
    {

        static void Main(string[] args)
        {
            //Farbiger Game Titel
            /*AsciiText();
            StartMenu();*/

            Database.GetConn().GetPackage();
        }
        static void StartMenu()
        {
            User user = new User();
            int c;
            Console.WriteLine("Please choose by entering a number:");
            Console.WriteLine("1.Login\n2.Register\n3.Quit");

            c = CheckInput(1, 3);
            if (c == 1)
            {
                if (user.LoginUser())
                    GameMenu();
                else
                    StartMenu();
            }
            if (c == 2)
            {
                if (user.RegisterUser())
                    GameMenu();
            }
            if (c == 3)
            {
                return;
            }

        }

        static void GameMenu()
        {
            int c;
            Battle battle = new Battle();
            Database.GetConn().GetUserInfo(User.username);
            Console.WriteLine($"\n===================================================", Color.Blue);
            Console.WriteLine($"User: {User.username} | ELO: {User.elo} | Coins: {User.coins} | Played Games: {User.playedGames}", Color.Blue);
            Console.WriteLine($"===================================================", Color.Blue);
            Console.WriteLine($"\nGame Menu");
            Console.WriteLine("Please choose by entering a number:");
            Console.WriteLine("1.Play a Round\n2.Buy a Package\n3.Trade\n4.Logout\n5.Quit Game");
            c = CheckInput(1, 5);
            switch (c)
            {
                case 1:
                    battle.StartBattle();
                    GameMenu();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    StartMenu();
                    break;
                case 5:
                    return;
                default:
                    break;
            }

        }
        static int CheckInput(int min, int max)
        {
            string input = Console.ReadLine();
            int c;
            while (!Int32.TryParse(input, out c) || c < min || c > max)
            {
                Console.WriteLine("Please choose a valid number!", Color.Red);
                input = Console.ReadLine();
            }
            return c;
        }
        static void AsciiText()
        {
            Console.WriteAscii("Monster Trading", Color.FromArgb(67, 144, 198));
            Console.WriteAscii("  Cards Game", Color.FromArgb(131, 184, 214));
            Console.WriteLine();

        }
    }
}
