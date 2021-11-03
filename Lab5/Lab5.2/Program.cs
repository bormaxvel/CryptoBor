using System;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Lab5._2
{
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, Int32 NumberOfBytes)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, hashAlgorithm))
            {
                return rfc2898.GetBytes(NumberOfBytes);
            }
        }
}
    class Program
    {
        static void Main(string[] args)
        {
            const string passwordToHash = "VeryComplexPassword";
            Console.WriteLine("Algoritm SHA1");
            HashPassword(passwordToHash, 30000);
            HashPassword(passwordToHash, 80000);
            HashPassword(passwordToHash, 130000);
            HashPassword(passwordToHash, 180000);
            HashPassword(passwordToHash, 230000);
            HashPassword(passwordToHash, 280000);
            HashPassword(passwordToHash, 330000);
            HashPassword(passwordToHash, 380000);
            HashPassword(passwordToHash, 430000);
            HashPassword(passwordToHash, 480000);
            // Console.WriteLine(Rfc2898DeriveBytes.HashAlgorithm);
        }
        public static void HashPassword(string passwordToHash, int numberOfRounds)
        {
            var sw = new Stopwatch();
            sw.Start();
            var hashedPassword = PBKDF2.HashPassword(Encoding.UTF8.GetBytes(passwordToHash), PBKDF2.GenerateSalt(), numberOfRounds, HashAlgorithmName.SHA1, 20);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Password to hash : " + passwordToHash);
            Console.WriteLine("Hashed Password : " + Convert.ToBase64String(hashedPassword));
            Console.WriteLine("Iterations <" + numberOfRounds + "> Elapsed Time: " + sw.ElapsedMilliseconds + "ms");
        }
    }

}

