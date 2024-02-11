using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4DP
{
    public class HashGenerator
    {
        public string GenerateHash(string input)
        {
            byte[] message = Encoding.UTF8.GetBytes(input);
            uint[] hash = SHA1(message);

            byte[] hashBytes = new byte[20];
            for (int i = 0; i < 5; i++)
            {
                BitConverter.GetBytes(hash[i]).CopyTo(hashBytes, i * 4);
            }

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        /// <summary>
        /// Метод реализующий хеширование по методу SHA1.
        /// </summary>
        /// <param name="message"> Сообщение в виде массива байтов. </param>
        /// <returns> Хэш в виде массива uint. </returns>
        public uint[] SHA1(byte[] message)
        {
            uint[] hashes = [0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0];

            message = AddBitsToMessage(message);
            byte[] chunk512 = new byte[64];
            int chunksNumber = message.Length / 64;

            for (int i = 0; i < chunksNumber; i++)
            {
                Array.Copy(message, i * 64, chunk512, 0, 64);
                hashes = CompressionFunction(chunk512, hashes);
            }

            return hashes;
        }

        /// <summary>
        /// Метод реализующий функцию сжатия.
        /// </summary>
        /// <param name="message"> Блок сообщения. </param>
        /// <param name="hashes"> Хэши. </param>
        /// <returns> Новые значения хэшей. </returns>
        private uint[] CompressionFunction(byte[] message, uint[] hashes) 
        {
            uint[] chunks = new uint[80];

            // Делим блок 512 бит на 16 блоков по 32 бита
            for (int i = 0; i < 16; i++)
            {
                chunks[i] = (uint)(message[i] << 24 | message[i + 1] << 16 | message[i + 2] << 8 | message[i + 3]);
            }

            for (int i = 16; i < 80; i++)
            {
                chunks[i] = LeftRotate(chunks[i - 3] ^ chunks[i - 8] ^ chunks[i - 14] ^ chunks[i - 16], 1);
            }

            uint a = hashes[0];
            uint b = hashes[1];
            uint c = hashes[2];
            uint d = hashes[3];
            uint e = hashes[4];

            for (int i = 0; i < 80; i++ )
            {
                uint f;
                uint k;
                if (i < 20)
                {
                    f = (b & c) | (~b & d);
                    k = 0x5a827999;
                }
                else if (i < 40)
                {
                    f = b ^ c ^ d;
                    k = 0x6ed9eba1;
                }
                else if (i < 60)
                {
                    f = (b & c) | (b & d) | (c & d);
                    k = 0x8f1bbcdc;
                }
                else
                {
                    f = b ^ c ^ d;
                    k = 0xca62c1d6;
                }

                uint temp = LeftRotate(a, 5) + f + e + k +chunks[i];
                e = d;
                d = c;
                c = LeftRotate(b, 30);
                b = a;
                a = temp;
            }

            hashes[0] += a;
            hashes[1] += b;
            hashes[2] += c;
            hashes[3] += d;
            hashes[4] += e;

            return hashes;
        }

        /// <summary>
        /// Метод циклического левого сдвига.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"> Количество битов, на которое необходимо сдвинуть. </param>
        /// <returns></returns>
        private uint LeftRotate(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        /// <summary>
        /// Метод дополнения исходного сообщения битами.
        /// </summary>
        /// <param name="message"> Исходное сообщение. </param>
        /// <returns> Дополненное сообщение. </returns>
        private byte[] AddBitsToMessage(byte[] message)
        {
            ulong initLength = (ulong)message.Length; // Сохраняем исходную длину сообщения.
            ulong extendedLength = initLength + 1;

            // Вычисляем дополненную длину сообщения.
            while(extendedLength % 64 != 56)
            {
                extendedLength++;
            }

            byte[] extendedMessage = new byte[extendedLength + 8];
            Array.Copy(message, extendedMessage, (int)initLength);
            extendedMessage[initLength] = 0x80; // Добавляем один бит.

            // Добавляем 64 бита.
            byte[] bitsLength = BitConverter.GetBytes(initLength);
            Array.Copy(bitsLength, 0, extendedMessage, (int)extendedLength, 8);

            return extendedMessage;
        }
    }
}
