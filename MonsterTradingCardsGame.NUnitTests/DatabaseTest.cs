using System;
using System.IO;
using Npgsql;
using NUnit.Framework;


namespace MonsterTradingCardsGame.NUnitTests {
    [TestFixture]
    class DatabaseTest {
        static string username = "TestDummy", password = "Password123";
        [SetUp]
        public void Setup() {
        }
        [Test, Order(1)]
        public void DatabaseConnection() {
            //Arrange 
            var db = Database.GetConn();
            var result = false;
            //Act
            try
            {
                db.Open();
                result = true;
                db.Close();
            }
            catch (NpgsqlException)
            {
                result = false;
            }
            //Assert
            Assert.IsTrue(result);
        }
        [Test, Order(2)]
        public void RegisterUserShoudPass() {
            //Arrange 
            int elo = 100, coins = 20, games = 0;
            //Act
            var result = Database.GetConn().RegisterUser(username, password, elo, coins, games);
            //Assert
            Assert.IsTrue(result);
        }
        [Test, Order(3)]
        public void LoginUserShoudPass() {
            //Act
            var result = Database.GetConn().LoginUser(username, password);
            //Assert
            Assert.IsTrue(result);
        }

        [Test, Order(4)]
        public void GetUserInfoFromDB() {
            Database.GetConn().LoginUser(username, password);
            //Act
            Database.GetConn().GetUserInfo();
            //Assert
            Assert.AreEqual(User.username, username);
            Assert.IsNotNull(User.elo);
            Assert.IsNotNull(User.coins);
            Assert.IsNotNull(User.playedGames);
        }
        [Test, Order(5)]
        public void GetAllCardsFromDB() {
            //Act
            Database.GetConn().GetAllCards();
            //Assert
            Assert.AreEqual(Stack.cardList.Count, Stack.stackSize);
            Assert.AreEqual(Stack.cardList[0].name, "RegularSpell"); //First Card
            Assert.AreEqual(Stack.cardList[26].name, "FireKraken"); //Last Card
        }
        [Test, Order(6)]
        public void UpdateUserStats() {
            //Arrange 
            Database.GetConn().LoginUser(username, password);
            //Act
            Database.GetConn().UpdatePlayedGames(); //adding 1
            Database.GetConn().UpdateElo(3); //adding 3 
            Database.GetConn().UpdateCoins(2); //adding 2
            Database.GetConn().GetUserInfo(); //refresh stored data
            //Assert
            Assert.AreEqual(User.playedGames, 1);
            Assert.AreEqual(User.elo, 103);
            Assert.AreEqual(User.coins, 22);
        }
        [Test, Order(7)]
        public void InsertCardIntoDB() {
            //Arrange 
            int cardId = 1;
            Database.GetConn().LoginUser(username, password);
            //Act
            Database.GetConn().InsertCard(cardId);
            Database.GetConn().GetUserCards(); //refresh stored data
            //Assert
            Assert.AreEqual(Stack.userCards.Count, 1);
        }
        //[Test, Order(8)]
        public void DeleteCardFromDB() {
            //Arrange 
            int cardId = 1;
            Database.GetConn().LoginUser(username, password);
            //Act
            Database.GetConn().DeleteCard(cardId, username);
            Database.GetConn().GetUserCards(); //refresh stored data
            //Assert
            Assert.AreEqual(Stack.userCards.Count, 0);
        }
        [Test, Order(8)]
        public void DeleteUserFromDB() {
            //Act
            var result = Database.GetConn().DeleteUser(username);
            //Assert
            Assert.IsTrue(result);
        }
    }
}
