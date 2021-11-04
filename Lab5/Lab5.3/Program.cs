using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace _4part
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
            List<string> logins = new List<string>();
            List<string> passwords = new List<string>();
            List<string> salts = new List<string>();
            while (true)
            {

                Console.Write("Enter '1' to logIN and '0' to Register: ");
                var temp = Console.ReadLine();
                if (temp == "1")
                {
                    Console.Write("Please enter your login: ");
                    var login = Console.ReadLine();
                    if (logins.Contains(login))
                    {
                        var ind = logins.IndexOf(login);
                        Console.Write("Please enter your password: ");
                        var pass = Console.ReadLine();
                        var salt = Convert.FromBase64String(salts[ind]);
                        var sha256Forpass = Convert.ToBase64String(PBKDF2.HashPassword(Encoding.Unicode.GetBytes(pass), salt, 30000, HashAlgorithmName.SHA256, 32));
                        if (passwords[ind] == sha256Forpass)
                        {
                            Console.WriteLine("You loggined");
                        }
                        else
                        {
                            Console.WriteLine("Wrong password");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Wrong login");
                    }
                }
                else if (temp == "0")
                {
                    Console.Write("Please enter your login: ");
                    var login = Console.ReadLine();
                    if (!logins.Contains(login))
                    {
                        logins.Add(login);
                        Console.Write("Please enter your password: ");
                        var pass = Console.ReadLine();
                        var salt = PBKDF2.GenerateSalt();
                        var sha256Forpass = Convert.ToBase64String(PBKDF2.HashPassword(Encoding.Unicode.GetBytes(pass), salt, 30000, HashAlgorithmName.SHA256, 32));
                        passwords.Add(sha256Forpass);
                        salts.Add(Convert.ToBase64String(salt));
                        Console.WriteLine("you registered");

                    }
                    else
                    {
                        Console.WriteLine("login already in base");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Enter correct please");
                }
            }
        }
    }
}
