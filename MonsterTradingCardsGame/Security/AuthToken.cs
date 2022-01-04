using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;              //Concat()

namespace MonsterTradingCardsGame {
    public class AuthToken {
        private static string token;
        public AuthToken() {
            createToken();
        }
        public static void createToken() {
            //GUID: Globally Unique Identifier is a number with 128 Bit
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            token = Convert.ToBase64String(time.Concat(key).ToArray());
        }
        public static bool checkToken() {
            //The token is expired after 12 hours
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-12)) return false; else return true;
        }
        public static void deleteToken() {
            //The token will be set to null
            token = null;
        }

    }
}
