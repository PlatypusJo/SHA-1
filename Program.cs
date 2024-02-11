using System;
using System.Collections;

namespace Lab4DP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string message = "The quick brown fox jumps over the lazy dog";
            HashGenerator hashGenerator = new HashGenerator();
            string hash = hashGenerator.GenerateHash(message);
            Console.WriteLine($"Сообщение: {message}");
            Console.WriteLine($"Вычисленный хэш: {hash}");
        }
    }
}