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
            //GUID: Globally Unique Identifier ist eine Zahl mit 128 Bit
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            token = Convert.ToBase64String(time.Concat(key).ToArray());
        }
        public static bool checkToken() {
            //Der Tokken ist nach 24 ungültig
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24)) return false; else return true;
        }

    }
}
