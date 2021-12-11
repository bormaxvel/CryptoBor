using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Security;
using System.Threading;
using NLog;
using Microsoft.Extensions.Logging;

namespace Lab13;
class User
{
    public string Login { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public string[] Roles { get; set; }
}

public class PBKDF2
{
    private static Logger log = NLog.LogManager.GetCurrentClassLogger();
    public static byte[] GenerateSalt()
    {
        log.Trace($"creating RNGCryptoServiceProvider");
        using (var randomNumberGenerator = new RNGCryptoServiceProvider())
        {
            log.Trace($"creating empty randomNumber array");
            var randomNumber = new byte[32];
            randomNumberGenerator.GetBytes(randomNumber);
            log.Trace($"Assigning {randomNumber} to randomNumber");
            log.Debug($"Assigning {randomNumber} to randomNumber");
            log.Trace($"Returning {randomNumber}");
            return randomNumber;
        }
    }
    public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, Int32 NumberOfBytes)
    {
        log.Trace($"creating Rfc2898DeriveBytes");
        using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, hashAlgorithm))
        {
            log.Trace($"Returning {rfc2898.GetBytes(NumberOfBytes)}");
            return rfc2898.GetBytes(NumberOfBytes);
        }
    }
}
class Protector
{
    private static Logger log = NLog.LogManager.GetCurrentClassLogger();
    private static Dictionary<string, User> _users = new Dictionary<string, User>();
    public static void Register(string userName, string password, string[] roles = null)
    {
        log.Trace($"Checking userName before start registration");
        log.Debug($"Checking userName before start registration");
        if (_users.ContainsKey(userName))
        {
            Console.WriteLine("Error, User exist");
            log.Warn($"Error, User exist while logining");
        }
        else
        {

            var Tempuser = new User();
            log.Trace($"Creating new variable of class User");
            Tempuser.Login = userName;
            log.Trace($"Assigning {Tempuser.Login} to Tempuser.Login");
            log.Debug($"Assigning {Tempuser.Login} to Tempuser.Login");
            Tempuser.Salt = PBKDF2.GenerateSalt();
            log.Trace($"Assigning {Tempuser.Salt} to Tempuser.Salt by calling function");
            log.Debug($"Assigning {Tempuser.Salt} to Tempuser.Salt by calling function");
            Tempuser.PasswordHash = PBKDF2.HashPassword(Encoding.Default.GetBytes(password), Tempuser.Salt, 1000, HashAlgorithmName.SHA256, 32);
            log.Trace($"Assigning {Tempuser.PasswordHash} to Tempuser.PasswordHash by calling function");
            log.Debug($"Assigning {Tempuser.PasswordHash} to Tempuser.PasswordHash by calling function");
            Tempuser.Roles = roles;
            log.Trace($"Assigning {Tempuser.Roles} to Tempuser.Roles");
            log.Debug($"Assigning {Tempuser.Roles} to Tempuser.Roles");
            _users.Add(Tempuser.Login, Tempuser);
            log.Trace($"Adding user to dictonary");
            log.Debug($"Adding user to dictonary");
            Console.WriteLine("Succes, User " + Tempuser.Login + " is registered");

        }
    }




    public static bool CheckPassword(string userName, string password)
    {
        log.Trace($"Checking Password function");
        log.Trace($"Checking userName before start");
        log.Debug($"Checking userName before start");
        if (_users.ContainsKey(userName))
        {

            var Tempuser2 = _users[userName];
            log.Trace($"Getting user from dictionary");
            var tempPassword = PBKDF2.HashPassword(Encoding.Default.GetBytes(password), Tempuser2.Salt, 1000, HashAlgorithmName.SHA256, 32);
            log.Trace($"Computed tempPassword {tempPassword} by calling function");
            log.Trace($"Comparing 2 Passwords");
            log.Debug($"Comparing 2 Passwords");
            if (Convert.ToBase64String(Tempuser2.PasswordHash) == Convert.ToBase64String(tempPassword))
            {
                log.Trace($"Succes");
                return true;
            }
            else
            {
                Console.WriteLine("bad password");
                log.Warn($"CheckPassword - bad password");
                return false;
            }

        }
        else
        {
            Console.WriteLine("bad login");
            log.Warn($"CheckPassword - bad login");
            return false;
        }
    }
    public static void LogIn(string userName, string password)
    {
        log.Trace($"Checking password before start");
        log.Debug($"Checking password before start");
        if (CheckPassword(userName, password))
        {
            log.Trace($"Creating new GenericIdentity");
            var identity = new GenericIdentity(userName, "OIBAuth");
            log.Trace($"Creating new GenericPrincipal");
            var principal = new GenericPrincipal(identity, _users[userName].Roles);
            log.Trace($"Assigning {principal} to CurrentPrincipal");
            log.Debug($"Assigning {principal} to CurrentPrincipal");
            System.Threading.Thread.CurrentPrincipal = principal;
            Console.WriteLine("You Loggined");
        }
    }
    public static void CheckUserFeatures()
    {
        log.Trace($"Checking CurrentPrincipal");
        log.Debug($"Checking CurrentPrincipal");
        if (Thread.CurrentPrincipal == null)
        {
            log.Fatal($"Thread.CurrentPrincipal cannot be null");
            throw new SecurityException("Thread.CurrentPrincipal cannot be null.");

        }
        log.Trace($"Checking Features for CurrentPrincipal");
        if (Thread.CurrentPrincipal.IsInRole("Admin"))
        {
            log.Trace($"Giving acces for Admin");
            Console.WriteLine("You have access to Admin secure feature.");
        }
        else
        {
            Console.WriteLine("User must be a member of Admin to access this feature.");
            log.Warn($"Have no acces for Admin");
        }


        if (Thread.CurrentPrincipal.IsInRole("Owner"))
        {
            Console.WriteLine("You have access to Owner secure feature.");
            log.Warn($"Have no acces for Owner");
        }
        else
        {
            Console.WriteLine("User must be Owner to access this feature.");
            log.Trace($"Giving acces for Owner");
        }


        if (Thread.CurrentPrincipal.IsInRole("User"))
        {
            Console.WriteLine("You have access to User feature.");
            log.Trace($"Giving acces for ");
        }
        else
        {
            Console.WriteLine("User must be User to access this feature.");
            log.Warn($"Have no acces for User");
        }


        if (Thread.CurrentPrincipal.IsInRole("Guest"))
        {
            Console.WriteLine("You have access to Guest feature.");
            log.Trace($"Giving acces for Guest");

        }
        else
        {
            Console.WriteLine("User must be Guest to access this feature.");
            log.Warn($"Have no acces for Guest");
        }


        Console.WriteLine();
    }
}
class Program
{

    static void Main(string[] args)
    {

        Logger log = NLog.LogManager.GetCurrentClassLogger();
        log.Trace("Starting registration");
        Console.WriteLine("Registration of users");
        Console.WriteLine();
        log.Trace("Starting cycle for Registration");
        string templogin = null;
        string temppass = null;
        int tempk = 0;
        for (int i = 0; i < 2; i++)
        {
            log.Info($" New User Registration");
            log.Trace($" {i} iteration of redistration cycle");
            log.Debug($" {i} iteration of redistration cycle");
            Console.WriteLine("New user registration");
            Console.Write("Please enter your login: ");
            var login = Console.ReadLine();
            log.Trace($"login changed from '{templogin}' to '{login}' - login getted from user");
            log.Debug($"login changed from '{templogin}' to '{login}' - login getted from user");
            templogin = login;
            Console.Write("Please enter your password: ");
            var pass = Console.ReadLine();
            log.Trace($"password changed from '{temppass}' to '{pass}' - password getted from user");
            log.Debug($"password changed from '{temppass}' to '{pass}' - password getted from user");
            temppass = pass;
            try
            {
                Console.Write("Please enter number of roles: ");
                int k = Convert.ToInt32(Console.ReadLine());
                log.Trace($"k changed from '{tempk}' to '{k}' - number of roles getted from user");
                log.Debug($"k changed from '{tempk}' to '{k}' - number of roles getted from user");
                tempk = k;
                string[] tags = new string[k];
                log.Trace($" {tags} - initialisated empty array of tags");
                log.Debug($" {tags} - initialisated empty array of tags");
                log.Trace($"Starting cycle to enter roles");
                for (int n = 0; n < k; n++)
                {
                    log.Trace($" {n} iteration of role cycle");
                    log.Debug($" {n} iteration of role cycle");
                    Console.Write("Please enter role: ");
                    tags[n] = Console.ReadLine();
                    log.Trace($"Changed tags[{n}] from '' to '{tags[n]}' iteration of cycle");
                    log.Debug($"Changed tags[{n}] from '' to '{tags[n]}' iteration of cycle");

                }
                Protector.Register(login, pass, tags);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                log.Error(ex, "You entered not a positive NUMBER");
            }
        }
        log.Trace($"Finishing cycle for redistration");


        Console.WriteLine("Is it possible to divide by 0? (enter '1' if yes and you want to try)");
        var a = Console.ReadLine();
        if (a == "1")
        {
            int x1 = 0;
            int x2 = 1;
            var ex = new DivideByZeroException();
            log.Fatal(ex, "Fatal error - DivideByZeroException");
            int x3 = x2 / x1;
        }
        else { Console.WriteLine("Corect ancwer"); }





        log.Trace("Starting Logining");
        Console.WriteLine("Now you can try to logIN");
        log.Trace("Starting cycle for Logining");
        while (true)
        {
            log.Info($" New User Logining");
            log.Trace($" New iteration of while login cycle");
            log.Debug($" New iteration of while login cycle");
            Console.Write("Please enter your login: ");
            var login = Console.ReadLine();
            log.Trace($"login changed from '{templogin}' to '{login}' - login getted from user");
            log.Debug($"login changed from '{templogin}' to '{login}' - login getted from user");
            templogin = login;
            Console.Write("Please enter your password: ");
            var pass = Console.ReadLine();
            log.Trace($"password changed from '{temppass}' to '{pass}' - password getted from user");
            log.Debug($"password changed from '{temppass}' to '{pass}' - password getted from user");
            temppass = pass;
            log.Trace($"Checking password");
            if (!Protector.CheckPassword(login, pass))
            {
                continue;
            }
            log.Trace($"Calling LogIn function");
            Protector.LogIn(login, pass);
            log.Trace($"Calling CheckUserFeatures function");
            Protector.CheckUserFeatures();
            Console.WriteLine();
        }
    }
}
