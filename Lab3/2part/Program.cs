using System;
using System.Text;
using System.Security.Cryptography;

namespace _2part
{
    class Program
    {
        static byte[] ComputeHashMd5(byte[] dataForHash)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(dataForHash);
            }
        }
        static void Main(string[] args)
        {
            Guid guid1 = new Guid("564c8da6-0440-88ec-d453-0bbad57c6036");
            Console.WriteLine(guid1);
            int mem;
            for(int i = 100000000; i < 200000000; i++)
            {
                var md5= ComputeHashMd5(Encoding.Unicode.GetBytes(i.ToString().Substring(1,8)));
                if(new Guid(md5) == guid1)
                {
                    Console.WriteLine("Password= "+ i);
                    Console.WriteLine(Convert.ToBase64String(md5));
                    mem = i;
                }
            }

        }
    }
}
