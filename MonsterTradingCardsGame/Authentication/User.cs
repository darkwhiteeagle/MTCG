using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MonsterTradingCardsGame
{
    class User
    {
        public static string username;
        public static int coins { get; set; }
        public static int elo { get; set; }
        public static int playedGames { get; set; }
        private string password { get; set; }

        public bool RegisterUser()
        {
            Database db = Database.GetConn();
            bool containsLetter = false;

            Console.WriteLine("Enter username:");
            while (containsLetter == false || username.Length < 5)
            {
                username = Console.ReadLine();
                containsLetter = Regex.IsMatch(username, "[a-zA-Z]");
                if (!containsLetter || username.Length < 5)
                    Console.WriteLine("Username must contain letters and be longer than 5 Letters, try again");
            }
            containsLetter = false;
            Console.WriteLine("Enter Password:");
            while (containsLetter == false || password.Length < 7)
            {
                password = ReadPassword();
                containsLetter = Regex.IsMatch(password, "[a-zA-Z0-9]");
                if (!containsLetter || password.Length < 7)
                    Console.WriteLine("Password must contain at leat\n a capital letter\n a small letter\n a number and be at least 7 Letters, try again");
            }
            elo = 100;
            coins = 20;
            playedGames = 0;
            if (db.RegisterUser(username, password, elo, coins, playedGames))
            {
                Console.WriteLine("Successfully registered!");
                AuthToken auth = new AuthToken();       //constuctor creates new AUTH Token
                return true;
            }
            else
            {
                Console.WriteLine("Something went wrong, try again!");
                return false;
            }
        }
        public bool LoginUser()
        {
            Database db = Database.GetConn();
            Console.WriteLine("Enter username:");
            username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            password = ReadPassword();

            if (db.LoginUser(username, password))
            {
                Console.WriteLine("Successfully logged in!");
                AuthToken auth = new AuthToken();       //constuctor creates new AUTH Token
                return true;
            }
            else
            {
                Console.WriteLine("Something went wrong, try again!");
                return false;
            }
        }
        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }
        public static void LogoutUser()
        {
            //code
        }
    }
}
