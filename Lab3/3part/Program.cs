using System;
using System.Text;
using System.Security.Cryptography;

namespace _3part
{
    class Program
    {
        public static byte[] ComputeHmacsha1(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }
        public static void CheckIfConfident(string GettedMessage, string key, string GettedHash)
        {
            Console.WriteLine("Computing and comparing hash");
            var tempComputedHash = ComputeHmacsha1(Encoding.Unicode.GetBytes(GettedMessage), Encoding.Unicode.GetBytes(key));
            if (GettedHash == Convert.ToBase64String(tempComputedHash))
            {
                Console.WriteLine("Computed Hash is the same to getted hash");
                Console.WriteLine("Your message is autorized");
            }
            else
            {
                Console.WriteLine("Computed Hash is NOT the same to getted hash");
                Console.WriteLine("Your message is FAKE");
            }
        }

        static void Main(string[] args)
        {
            const string strForHash = "Hello World!";
            const string IncorectStrForHash = "Buy World!";
            const string keyForHash = "secret";
            const string HashWeGet = "+wS72Kx9qyWwG1LhKL4rDbgbNqc=";

            var sha1ForStr = ComputeHmacsha1(Encoding.Unicode.GetBytes(strForHash), Encoding.Unicode.GetBytes(keyForHash));
            Console.WriteLine($"Hash sha1:{Convert.ToBase64String(sha1ForStr)}");
            Console.WriteLine("-------------------");
            CheckIfConfident(strForHash, keyForHash, HashWeGet);
            Console.WriteLine("-------------------");
            CheckIfConfident(IncorectStrForHash, keyForHash, HashWeGet);


        }
    }
}
