using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace _4part
{
    class Program
    {
        public static byte[] ComputeHashSha256(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(toBeHashed);
            }
        }
        static void Main(string[] args)
        {
            List<string> logins = new List<string> ();
            List<string> passwords = new List<string> ();

            while (true)
            {

                Console.Write("Enter '1' to logIN and '0' to logUP: ");
                var temp = Console.ReadLine();
                if (temp == "1")
                {
                    Console.Write("Please enter your login: ");
                    var login = Console.ReadLine();
                    var sha256ForStr1 = Convert.ToBase64String(ComputeHashSha256(Encoding.Unicode.GetBytes(login)));
                    if (logins.Contains(sha256ForStr1)){
                        var ind = logins.IndexOf(sha256ForStr1);
                        Console.Write("Please enter your password: ");
                        var pass = Console.ReadLine();
                        var sha256Forpass = Convert.ToBase64String(ComputeHashSha256(Encoding.Unicode.GetBytes(pass)));
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
                    var sha256ForStr1 = Convert.ToBase64String(ComputeHashSha256(Encoding.Unicode.GetBytes(login)));
                    if (!logins.Contains(sha256ForStr1)){
                        logins.Add(sha256ForStr1);
                        Console.Write("Please enter your password: ");
                        var pass = Console.ReadLine();
                        var sha256Forpass = Convert.ToBase64String(ComputeHashSha256(Encoding.Unicode.GetBytes(pass)));
                        passwords.Add(sha256Forpass);
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
