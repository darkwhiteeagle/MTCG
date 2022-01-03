using NUnit.Framework;

namespace MonsterTradingCardsGame.NUnitTests {
    [TestFixture]
    public class AuthenticationTest {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void RegisterUserShouldPass() {
            //Arrange 
            var user = new User();
            //Act
            var result = user.RegisterUser("TestUser", "Password123");
            Database.GetConn().DeleteUser("TestUser");
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void RegisterUserShouldFail() {
            //Arrange 
            var user = new User();
            //Act
            var result = user.RegisterUser("Terry", "Password123");
            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void LoginUserShouldPass() {
            //Arrange 
            var user = new User();
            //Act
            var result = user.AttemptLogin("Dummy", "Dummy11");
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void LoginUserShouldFail() {
            //Arrange 
            var user = new User();
            //Act
            var result = user.AttemptLogin("Dummy", "wrongPassword");
            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void AuthTokenTest() {
            //Arrange 
            AuthToken.createToken();
            //Act
            var result = AuthToken.checkToken();
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void LogoutUserShouldPass() {
            //Arrange 
            var user = new User();
            //Act
            user.AttemptLogin("Dummy", "Dummy11");
            User.LogoutUser();
            //Assert
            Assert.IsNull(User.username);
            Assert.AreEqual(User.elo, 0);
            Assert.AreEqual(User.coins, 0);
            Assert.AreEqual(User.playedGames, 0);
        }

    }
}