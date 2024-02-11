using System;
using System.Collections;

namespace Lab4DP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string plain = "plain.txt";
            HashGenerator hashGenerator = new HashGenerator();
            hashGenerator.GenerateHash(plain);
            Console.WriteLine("Конец");
        }
    }
}