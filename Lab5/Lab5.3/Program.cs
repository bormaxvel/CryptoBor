using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace _4part
{
    public class SaltedHash
    {
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length,second.Length);
            return ret;
        }
        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed,salt));
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> logins = new List<string> ();
            List<string> passwords = new List<string> ();
            List<string> salts = new List<string> ();
            while (true)
            {

                Console.Write("Enter '1' to logIN and '0' to Register: ");
                var temp = Console.ReadLine();
                if (temp == "1")
                {
                    Console.Write("Please enter your login: ");
                    var login = Console.ReadLine();
                    if (logins.Contains(login)){
                        var ind = logins.IndexOf(login);
                        Console.Write("Please enter your password: ");
                        var pass = Console.ReadLine();
                        var salt = Convert.FromBase64String(salts[ind]);
                        var sha256Forpass = Convert.ToBase64String(SaltedHash.HashPasswordWithSalt(Encoding.Unicode.GetBytes(pass), salt));
                        if (passwords[ind] == sha256Forpass){
                            Console.WriteLine("You loggined");
                        } else
                        {
                            Console.WriteLine("Wrong password");
                        }
                        
                    } else
                    {
                        Console.WriteLine("Wrong login");
                    }
                }else if (temp == "0"){
                    Console.Write("Please enter your login: ");
                    var login = Console.ReadLine();
                    if (!logins.Contains(login)){
                        logins.Add(login);
                        Console.Write("Please enter your password: ");
                        var pass = Console.ReadLine();
                        var salt = SaltedHash.GenerateSalt();
                        var sha256Forpass = Convert.ToBase64String(SaltedHash.HashPasswordWithSalt(Encoding.Unicode.GetBytes(pass), salt));
                        passwords.Add(sha256Forpass);
                        salts.Add(Convert.ToBase64String(salt));
                        Console.WriteLine("you registered");
                        
                    } else
                    {
                        Console.WriteLine("login already in base");
                        continue;
                    }
                } else
                {
                   Console.WriteLine("Enter correct please"); 
                }
            }
        }
    }
}
