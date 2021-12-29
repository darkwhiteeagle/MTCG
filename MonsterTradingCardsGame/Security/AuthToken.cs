using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;              //Concat()

namespace MonsterTradingCardsGame
{
    class AuthToken
    {
        private static string token;
        public AuthToken()
        {
            createToken();
        }
        private void createToken()
        {
            //GUID: Globally Unique Identifier ist eine Zahl mit 128 Bit
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            token = Convert.ToBase64String(time.Concat(key).ToArray());
        }
        public static bool checkToken()
        {
            //Der Tokken ist nach 24 ungültig
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                //Console.WriteLine("Token is too old!");
                return false;
            }
            else
            {
                //Console.WriteLine("User authenticated!");
                return true;
            }
        }

    }
}
