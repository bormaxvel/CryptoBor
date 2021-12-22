using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lab8._3
{

    class Program
    {

        private readonly static string CspContainerName = "RsaContainer";
        public static void AssignNewKey(string publicKeyPath)
        {
            CspParameters cspParameters = new CspParameters(1)
            {
                KeyContainerName = CspContainerName,

                ProviderName = "Microsoft Strong Cryptographic Provider"
            };
            var rsa = new RSACryptoServiceProvider(cspParameters)
            {
                PersistKeyInCsp = true
            };
            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
        }
        public static byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plainBytes;
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,

            };
            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                plainBytes = rsa.Decrypt(dataToDecrypt, false);
            }
            return plainBytes;
        }
        public static byte[] EncryptData(string publicKeyPath, byte[] dataToEncrypt)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                cipherbytes = rsa.Encrypt(dataToEncrypt, false);
            }
            return cipherbytes;
        }
        static void Main(string[] args)
        {
            //Змінні
            string mess = "Secret message from Maksym";

            string NamePublicKey = "LarionovPublicKey.xml";          // для зашифровки назва ключа
            
            string NameForMessageToDecrypt = "Message.dat";    // для розшифровки назва повыдомлення
            
            // AssignNewKey("Borysenko_Public.xml");

            //Зашифровка
            var encrypted = EncryptData(NamePublicKey, Encoding.Unicode.GetBytes(mess));
            File.WriteAllBytes ("NewMyMessage(maks).dat", encrypted);
            Console.WriteLine("Message = " + mess);
            Console.WriteLine();

            // // Розшифровка
            // var encryptedmessage =  File.ReadAllBytes(NameForMessageToDecrypt);
            // var decrypted = DecryptData(encryptedmessage);
            // Console.WriteLine("Decrypted message = " + Encoding.Unicode.GetString(decrypted));
        }
    }
}