using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text.Json;
using Colorful;
using System.Drawing;
using Console = Colorful.Console;


namespace MonsterTradingCardsGame {
    public class Database {
        private static Database _DB = new Database();
        private static NpgsqlConnection _conn = new NpgsqlConnection("Host=localhost;Username=terry;Password=Brooklyn99;Database=mtcg");

        private Database() { }

        public static Database GetConn() {
            return _DB;
        }
        public bool Open() {
            _conn.Open();
            return true;
        }
        public bool Close() {
            _conn.Close();
            return true;
        }
        //Administration Database entries
        public bool RegisterUser(string username, string password, int elo, int coins, int played_games) {
            Open();

            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256 - bit subkey(use HMACSHA256 with 100, 000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT username FROM player WHERE username = @username", _conn);
            cmd.Parameters.AddWithValue("username", username);
            Object res = cmd.ExecuteScalar();

            if (res == null)
            {
                NpgsqlCommand cmdInsert = new NpgsqlCommand("INSERT INTO player (username, password, elo, coins, played_games, salt) VALUES (@username, @password, @elo, @coins, @played_games, @salt); ", _conn);
                cmdInsert.Parameters.AddWithValue("username", username);
                cmdInsert.Parameters.AddWithValue("password", hashed);
                cmdInsert.Parameters.AddWithValue("elo", elo);
                cmdInsert.Parameters.AddWithValue("coins", coins);
                cmdInsert.Parameters.AddWithValue("played_games", played_games);
                cmdInsert.Parameters.AddWithValue("salt", salt);
                cmdInsert.ExecuteReader();
                Close();
                User.username = username;
                return true;
            }
            Close();
            return false;
        }
        public bool LoginUser(string username, string password) {
            Open();
            NpgsqlCommand checkSalt = new NpgsqlCommand("SELECT salt FROM player WHERE username = @username;", _conn);
            checkSalt.Parameters.AddWithValue("username", username);
            Object salt = checkSalt.ExecuteScalar();

            if (salt != null)
            {
                NpgsqlCommand checkPassword = new NpgsqlCommand("SELECT password FROM player WHERE username = @username;", _conn);
                checkPassword.Parameters.AddWithValue("username", username);
                Object dbPassowrd = checkPassword.ExecuteScalar();

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: (byte[])salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                if (hashed == (string)dbPassowrd)
                {
                    Close();
                    User.username = username;
                    GetUserInfo();
                    return true;
                }
            }
            Close();
            return false;
        }
        //Getting Data from Database with Npgsql
        public void GetUserInfo() {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT username, elo, coins, played_games FROM player WHERE username = @username;", _conn);
            cmd.Parameters.AddWithValue("username", User.username);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User.username = reader[0].ToString();
                User.elo = (int)reader[1];
                User.coins = (int)reader[2];
                User.playedGames = (int)reader[3];
            }
            Close();
        }
        public void GetAllCards() {
            Card.CARD_TYPE type;
            Card.ELEMENT_TYPE element;

            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM card;", _conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader[3].ToString() == "normal")
                    element = Card.ELEMENT_TYPE.Normal;
                else if (reader[3].ToString() == "fire")
                    element = Card.ELEMENT_TYPE.Fire;
                else
                    element = Card.ELEMENT_TYPE.Water;

                if (reader[4].ToString() == "monster")
                    type = Card.CARD_TYPE.Monster;
                else
                    type = Card.CARD_TYPE.Spell;

                Stack.cardList.Add(new Card((int)reader[0], reader[1].ToString(), (int)reader[2], element, type));
            }
            Close();
        }
        public void GetUserCards() {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cardstack WHERE username = @username;", _conn);
            cmd.Parameters.AddWithValue("username", User.username);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                Stack.userCards.Add(Stack.cardList[(int)reader[0]]);
            Close();
        }
        public void GetTradeCards() {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM trade;", _conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Trade.tradeList.Add(Stack.cardList[(int)reader[0]]);
                Trade.tradeInfo.Add(new Tuple<int, string>((int)reader[1], reader[2].ToString()));
            }
            Close();
        }
        //Update Database entries
        public void UpdatePlayedGames() {

            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("UPDATE player SET played_games = played_games + 1 WHERE username = @username;", _conn);
            cmd.Parameters.AddWithValue("username", User.username);
            Object res = cmd.ExecuteScalar();
            Close();

        }
        public void UpdateElo(int elo) {

            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("UPDATE player SET elo = elo + @elo WHERE username = @username;", _conn);
            cmd.Parameters.AddWithValue("username", User.username);
            cmd.Parameters.AddWithValue("elo", elo);
            Object res = cmd.ExecuteScalar();
            Close();

        }
        public void UpdateCoins(int coins) {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("UPDATE player SET coins = coins + @coins WHERE username = @username;", _conn);
            cmd.Parameters.AddWithValue("username", User.username);
            cmd.Parameters.AddWithValue("coins", coins);
            Object res = cmd.ExecuteScalar();
            Close();

        }
        //Database entry manipulation
        public void InsertCard(int id) {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO cardstack (id, username) VALUES (@id,@username);", _conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("username", User.username);
            Object res = cmd.ExecuteScalar();
            Close();
        }
        public void DeleteCard(int id, string username) {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM cardstack WHERE id = @id AND username = @username;", _conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("username", username);
            cmd.ExecuteScalar();
            Close();
        }
        public void GetPackage() {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                Open();
                int num = random.Next(0, Stack.stackSize - 1);
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM card WHERE id = @id;", _conn);
                cmd.Parameters.AddWithValue("id", num);
                int cardId = (int)cmd.ExecuteScalar();
                Close();
                InsertCard(cardId);
            }
            UpdateCoins(-5);
        }
        public void BuyCard(int id, int coins, string username) {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM trade WHERE id = @id AND coins = @coins AND username = @username;", _conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("coins", coins);
            cmd.Parameters.AddWithValue("username", username);
            Object res = cmd.ExecuteScalar();
            Close();
            InsertCard(id);
            if (username == User.username)
                coins = 0;
            UpdateCoins(-coins);
        }
        public void SellCard(int id, int coins) {
            DeleteCard(id, User.username);
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO trade VALUES(@id,@coins,@username);", _conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("coins", coins);
            cmd.Parameters.AddWithValue("username", User.username);
            Object res = cmd.ExecuteScalar();
            Close();
        }
        public bool DeleteUser(string username) {
            int rows;
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM trade WHERE username = @username;" +
                "DELETE FROM cardstack WHERE username = @username; " +
                "DELETE FROM player WHERE username = @username; ", _conn);
            cmd.Parameters.AddWithValue("username", username);
            //cmd.ExecuteScalar();
            rows = cmd.ExecuteNonQuery();
            Close();
            if (rows > 0)
                return true;

            return false;
        }
    }

}
