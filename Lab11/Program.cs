using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;


namespace Lab11
{
    class User
    {
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string[] Roles { get; set; }
    }

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
    class Protector
    {
        private static Dictionary<string, User> _users = new Dictionary<string, User>();
        public static void Register(string userName, string password, string[] roles = null)
        {
            if (_users.ContainsKey(userName))
            {
                Console.WriteLine("Error, User exist");
            }
            else
            {
                var Tempuser = new User();
                Tempuser.Login = userName;
                Tempuser.Salt = PBKDF2.GenerateSalt();
                Tempuser.PasswordHash = PBKDF2.HashPassword(Encoding.Default.GetBytes(password), Tempuser.Salt, 1000, HashAlgorithmName.SHA256, 32);
                Tempuser.Roles = roles;
                _users.Add(Tempuser.Login, Tempuser);
                Console.WriteLine("Succes, User " + Tempuser.Login + " is registered");
            }
        }




        public static bool CheckPassword(string userName, string password)
        {
            if (_users.ContainsKey(userName))
            {
                var Tempuser2 = _users[userName];
                var tempPassword = PBKDF2.HashPassword(Encoding.Default.GetBytes(password), Tempuser2.Salt, 1000, HashAlgorithmName.SHA256, 32);
                if (Convert.ToBase64String(Tempuser2.PasswordHash) == Convert.ToBase64String(tempPassword))
                {
                    
                    return true;

                }
                else
                {
                    Console.WriteLine("bad password");
                    return false;

                }

            }
            else
            {
                Console.WriteLine("bad login");
                return false;

            }
        }
        public static void LogIn(string userName, string password)
        {
            // Перевірка пароля
            if (CheckPassword(userName, password))
            {
                // Створюється екземпляр автентифікованого користувача 
                var identity = new GenericIdentity(userName, "OIBAuth");
                // Виконується прив’язка до ролей, до яких належить користувач
                var principal = new GenericPrincipal(identity, _users[userName].Roles);
                // Створений екземпляр автентифікованого користувача з відповідними
                // ролями присвоюється потоку, в якому виконується програма
                System.Threading.Thread.CurrentPrincipal = principal;
                Console.WriteLine("You Loggined");
            }
        }
        public static void CheckUserFeatures()
        {
            if (Thread.CurrentPrincipal == null)
            {
                Console.WriteLine("Thread.CurrentPrincipal cannot be null.");
            }

            if (Thread.CurrentPrincipal.IsInRole("Admin"))
            {
                Console.WriteLine("You have access to Admin secure feature.");
            }
            else
            {
                Console.WriteLine("User must be a member of Admin to access this feature.");
            }


            if (Thread.CurrentPrincipal.IsInRole("Owner"))
            {
                Console.WriteLine("You have access to Owner secure feature.");
            }
            else
            {
                Console.WriteLine("User must be Owner to access this feature.");
            }


            if (Thread.CurrentPrincipal.IsInRole("User"))
            {
                Console.WriteLine("You have access to User feature.");
            }
            else
            {
                Console.WriteLine("User must be User to access this feature.");
            }


            if (Thread.CurrentPrincipal.IsInRole("Guest"))
            {
                Console.WriteLine("You have access to Guest feature.");
            }
            else
            {
                Console.WriteLine("User must be Guest to access this feature.");
            }


            Console.WriteLine();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Registration of users");
            Console.WriteLine();
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("New user registration");
                Console.Write("Please enter your login: ");
                var login = Console.ReadLine();
                Console.Write("Please enter your password: ");
                var pass = Console.ReadLine();
                Console.Write("Please enter number of roles: ");
                int k = Convert.ToInt32(Console.ReadLine());
                string[] tags = new string[k];
                for (int n = 0; n < k; n++)
                {
                    Console.Write("Please enter role: ");
                    tags[n] = Console.ReadLine();
                }
                Protector.Register(login, pass, tags);
                Console.WriteLine();
            }
            Console.WriteLine("Now you can try to logIN");
            while (true)
            {
                Console.Write("Please enter your login: ");
                var login = Console.ReadLine();
                Console.Write("Please enter your password: ");
                var pass = Console.ReadLine();
                if (!Protector.CheckPassword(login, pass))
                {
                    continue;
                }
                Protector.LogIn(login, pass);
                Protector.CheckUserFeatures();
                Console.WriteLine();
            }
        }
    }
}