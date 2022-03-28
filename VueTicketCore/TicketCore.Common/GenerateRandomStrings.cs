using System;
using System.Security.Cryptography;
using System.Text;

namespace TicketCore.Common
{
    public class GenerateRandomStrings
    {
        public static string RandomString(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            using RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rngCrypto.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }

            return res.ToString();
        }
    }
}