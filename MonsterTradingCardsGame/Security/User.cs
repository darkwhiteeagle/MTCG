using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace MonsterTradingCardsGame {
    public class User {
        public static string username;
        public static int coins { get; set; }
        public static int elo { get; set; }
        public static int playedGames { get; set; }
        //Asks for Credentials and provide them further
        public bool NewUser() {
            string name = NewUsername();
            string pwd = NewPassword();
            if (RegisterUser(name, pwd)) return true; else return false;
        }
        //Makes calls a database function to create new user
        public bool RegisterUser(string name, string pwd) {
            Database db = Database.GetConn();
            elo = 100;
            coins = 20;
            playedGames = 0;
            if (db.RegisterUser(name, pwd, elo, coins, playedGames))
            {
                Console.WriteLine("Successfully registered!", Color.DarkGreen);
                AuthToken auth = new AuthToken();       //constuctor creates new AUTH Token
                return true;
            }
            else
            {
                Console.WriteLine("Something went wrong, try again!", Color.Red);
                return false;
            }
        }
        //Validate username input
        private string NewUsername() {
            string name = null;
            bool containsLetter = false;
            Console.WriteLine("Enter username:");
            while (containsLetter == false || name.Length < 5)
            {
                name = Console.ReadLine();
                containsLetter = Regex.IsMatch(name, "[a-zA-Z]");
                if (!containsLetter || name.Length < 5)
                    Console.WriteLine("Username must contain letters and be longer than 5 Letters, try again", Color.DarkRed);
            }
            return name;
        }
        //Validate passwort input
        private string NewPassword() {
            string pwd = null;
            bool containsLetter = false;
            Console.WriteLine("Enter Password:");
            while (containsLetter == false || pwd.Length < 7)
            {
                pwd = ReadPassword();
                containsLetter = Regex.IsMatch(pwd, "[a-zA-Z0-9]");
                if (!containsLetter || pwd.Length < 7)
                    Console.WriteLine("Password must contain at leat\n a capital letter\n a small letter\n a number and be at least 7 Letters, try again", Color.DarkRed);
            }
            return pwd;
        }
        //Ask for user credentials
        public bool LoginUser() {
            string pwd, name;
            Console.WriteLine("Enter username:");
            name = Console.ReadLine();
            Console.WriteLine("Enter password:");
            pwd = ReadPassword();
            if (AttemptLogin(name, pwd)) return true; else return false;
        }
        //Check if right username and password stored in DB
        public bool AttemptLogin(string name, string pwd) {
            Database db = Database.GetConn();
            if (db.LoginUser(name, pwd))
            {
                Console.WriteLine("Successfully logged in!", Color.Green);
                AuthToken auth = new AuthToken();       //constuctor creates new AUTH Token
                return true;
            }
            else
            {
                Console.WriteLine("Something went wrong, try again!", Color.Red);
                return false;
            }
        }
        //Hides Password while writing it
        private static string ReadPassword() {
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
        public static void LogoutUser() {
            username = null;
            elo = 0;
            coins = 0;
            playedGames = 0;
            //code
        }
    }
}
