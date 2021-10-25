using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic; 
namespace Пр2
{
    class Program
    {
        static void Main(string[] args)
        {
            byte message = 1, key = 125;
            byte encryptedMessage = (byte)(message ^ key);
            byte decryptedMessage = (byte)(encryptedMessage ^ key);
            Console.WriteLine(encryptedMessage);
            Console.WriteLine(decryptedMessage);
            byte[] decData = File.ReadAllBytes("file.TXT").ToArray();
            byte[] encData = new byte[decData.Length];
            Console.WriteLine("----reading txt------");
            for (int i = 0; i < decData.Length; i++){
                Console.WriteLine((char)decData[i]);
            }
            for (int i = 0; i < decData.Length; i++){
                encData[i] = (byte)(decData[i] ^ key);
            }
            Console.WriteLine("----Encrypting txt in dat------");
            for (int i = 0; i < decData.Length; i++){
                Console.WriteLine((char)encData[i]);
            }
            Console.WriteLine("--------decrypting dat-----");
            File.WriteAllBytes("text.dat", encData);
            byte[] encDataNew = File.ReadAllBytes("file.TXT").ToArray();
            byte[] decDataNew = new byte[decData.Length];
            for (int i = 0; i < decData.Length; i++){
                decDataNew[i] = (byte)(encData[i] ^ key);
                Console.WriteLine((char)decDataNew[i]);
            }
            File.WriteAllBytes("newtext.txt", decDataNew);
            Console.WriteLine("------------------------Starting 3 task----------------------");
            //string s = "fE8%2";
            StreamReader sr = new StreamReader("C:\\encfile5.dat");
            string text = sr.ReadToEnd();
            int k = 0;
            var password = new StringBuilder(5);
            for (char p0 = 'f'; p0 <= '~'; p0++){

                for (char p1 = '!'; p1 <= '~'; p1++){
                    for (char p2 = '!'; p2 <= '~'; p2++){
                        for (char p3 = '!'; p3 <= '~'; p3++){
                            for (char p4 = '!'; p4 <= '~'; p4++){
                                k=0;
                                password.Clear();
                                password.Append(p0);
                                password.Append(p1);
                                password.Append(p2);
                                password.Append(p3);
                                password.Append(p4);
                                var res = string.Empty;
                                for (var i = 0; i < text.Length; i++){
                                    res += ((char)(text[i] ^ password[k])).ToString();
                                    k++;
                                    if (k>4){
                                        k = 0;
                                    }
                                }
                                bool b = res.Contains("Mit21");
                                if(b == true)
                                {
                                    Console.Write("Password is: "); 
                                    Console.WriteLine(password);
                                    Console.WriteLine("Your message is: ");
                                    Console.WriteLine(res);
                                    Console.WriteLine("----looking for a new key----");
                                }
                            }
                        }
                    }
                }
                Console.WriteLine(p0);
            }
            Console.WriteLine("end");
        }
    }
}