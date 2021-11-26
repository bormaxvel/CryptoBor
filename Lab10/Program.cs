using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lab10
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

        public static byte[] SignData(byte[] hashOfDataToSign)
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
            };
            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");
                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }
        public static bool VerifySignature(byte[] hashOfDataToSign, byte[] signature, string publicKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");
                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
        static void Main(string[] args)
        {
            var document = Encoding.UTF8.GetBytes("Private Document (original)");
            byte[] hashedDocument = ComputeHashSha256(document);
            AssignNewKey("Borysenko_Public.xml");
            var signature = SignData(hashedDocument);
            var verified = VerifySignature(hashedDocument, signature, "Borysenko_Public.xml");
            Console.WriteLine(" Original Text = " + Encoding.Default.GetString(document));
            Console.WriteLine();
            Console.WriteLine(" Original Signature = " + Convert.ToBase64String(signature));
            Console.WriteLine(verified ? "Document verified." : "Document NOT verified");



            
            var documentFake = Encoding.UTF8.GetBytes("Fake doc");
            byte[] NotOriginalhashedDocument = ComputeHashSha256(documentFake);
            var verified2 = VerifySignature(NotOriginalhashedDocument, signature, "Borysenko_Public.xml");
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Second try with Fake Doc");
            Console.WriteLine(" Fake Text = " + Encoding.Default.GetString(documentFake));
            Console.WriteLine(verified2 ? "Document verified." : "Document NOT verified");



            var signature2  = SignData(NotOriginalhashedDocument);
            var verified3 = VerifySignature(hashedDocument, signature2, "Borysenko_Public.xml");
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Third try with Fake Signature");
            Console.WriteLine(" Fake Signature = " + Convert.ToBase64String(signature2));
            Console.WriteLine(verified2 ? "Document verified." : "Document NOT verified");
            Console.WriteLine();
        }
    }
}