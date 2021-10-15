using System;
using System.Security.Cryptography;

namespace С_
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(2077);
            for (int i=0; i < 10; i++)
            {
                Console.WriteLine(random.Next(-100, 100));
            }
            Console.WriteLine("--------------------");
            Random random1 = new Random(2077);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(random1.Next(-100, 100));
            }
            Console.WriteLine("--------------------");
            Random random2 = new Random(2021);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(random2.Next(-100, 100));
            }
            Console.WriteLine("--------------------");
            var rngGen = new RNGCryptoServiceProvider();
            var rndNumber = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                rngGen.GetBytes(rndNumber);
                string textRNGs = Convert.ToBase64String(rndNumber);
                Console.WriteLine(textRNGs);
            }
        }
    }
}
