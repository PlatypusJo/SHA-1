using System;
using System.Collections;

namespace Lab4DP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string message;
            string filePath = "plain.txt";
            message = new StreamReader(filePath).ReadToEnd();
            HashGenerator hashGenerator = new HashGenerator();
            string hash = hashGenerator.GenerateHash(message);
            Console.WriteLine($"Сообщение: {message}");
            Console.WriteLine($"Вычисленный хэш: {hash}");
        }
    }
}