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
        private uint[] _k;
        private List<bool>[] _w = new List<bool>[80];

        public HashGenerator() 
        {
            _k = [0x5a827999, 0x6ed9eba1, 0x8f1bbcdc, 0xca62c1d6];
        }

        public void GenerateHash(string fileName)
        {
            byte[] inputStream = File.ReadAllBytes(fileName);
            inputStream = inputStream.Reverse().ToArray();
            List<bool> inputBits = new BitArray(inputStream).ToBitsList();
            uint[] hash = SHA1(inputBits);
            for (int i = 0; i < hash.Length; i++)
            {
                Console.Write(Convert.ToString(hash[i], toBase: 16));
                Console.Write(" ");
            }
        }

        /// <summary>
        /// Метод реализующий хеширование по методу SHA1.
        /// </summary>
        /// <param name="message"> Сообщение в виде списка битов. </param>
        /// <returns> Хэш в виде списка битов. </returns>
        public uint[] SHA1(List<bool> message)
        {
            uint[] hashes = [0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0];

            message = AddBitsToMessage(message);
            int blocksNumber = message.Count / 512;

            for (int i = blocksNumber - 1; i >= 0; i--)
            {
                hashes = CompressionFunction(message.Skip(i * 512).Take(512).ToList(), hashes);
            }

            return hashes;
        }

        /// <summary>
        /// Метод реализующий функцию сжатия.
        /// </summary>
        /// <param name="message"> Блок сообщения. </param>
        /// <param name="hashes"> Хэши. </param>
        /// <returns> Новые значения хэшей. </returns>
        private uint[] CompressionFunction(List<bool> message, uint[] hashes) 
        {
            int delta = 15;

            for (int i = 0; i < 16; i++)
            {
                _w[i] = message.Skip(32 * delta).Take(32).ToList();
                delta--;
            }

            for (int i = 16; i < 80; i++)
            {
                _w[i] = _w[i - 3].Xor(_w[i - 8]);
                _w[i] = _w[i].Xor(_w[i - 14]);
                _w[i] = _w[i].Xor(_w[i - 16]);
                _w[i] = _w[i].LeftShift(1).Or(_w[i].RightShift(_w[i].Count - 1));
            }

            uint[] curHashes = new uint[5];
            for (int i = 0; i < 5; i++)
            {
                curHashes[i] = hashes[i];
            }

            for (int i = 0; i < 80; i++ )
            {
                uint funcRes = 0;
                uint k = 0;
                if (i >= 0 &&  i <= 19)
                {
                    funcRes = Function1(curHashes[1], curHashes[2], curHashes[3]);
                    k = _k[0];
                }
                else if (i >= 20 &&  i <= 39)
                {
                    funcRes = Function2(curHashes[1], curHashes[2], curHashes[3]);
                    k = _k[1];
                }
                else if (i >= 40 &&  i <= 59)
                {
                    funcRes = Function3(curHashes[1], curHashes[2], curHashes[3]);
                    k = _k[2];
                }
                else if (i >= 60 &&  i <= 79)
                {
                    funcRes = Function2(curHashes[1], curHashes[2], curHashes[3]);
                    k = _k[3];
                }

                uint temp = ((curHashes[0] << 5) | (curHashes[1] >> 27)) + funcRes + curHashes[4] + k + _w[i].FromBitListToUint32();
                curHashes[4] = curHashes[3];
                curHashes[3] = curHashes[2];
                curHashes[2] = (curHashes[1] << 30) | (curHashes[1] >> 2);
                curHashes[1] = curHashes[0];
                curHashes[0] = temp;
            }

            hashes[0] += curHashes[0];
            hashes[1] += curHashes[1];
            hashes[2] += curHashes[2];
            hashes[3] += curHashes[3];
            hashes[4] += curHashes[4];

            return hashes;
        }

        private uint Function1(uint b, uint c, uint d) => (b & c) | (~b & d);
        private uint Function2(uint b, uint c, uint d) => b ^ c ^ d;
        private uint Function3(uint b, uint c, uint d) => (b & c) | (b & d) | (c & d);

        /// <summary>
        /// Метод дополнения исходного сообщения битами.
        /// </summary>
        /// <param name="message"> Исходное сообщение. </param>
        /// <returns> Дополненное сообщение. </returns>
        private List<bool> AddBitsToMessage(List<bool> message)
        {
            ulong msgLength = (ulong)message.Count;

            message.Insert(0, true);
            while (message.Count % 512 != 448)
                message.Insert(0, false);

            List<bool> bitsLength = BitsListExtensions.FromUint64ToBitList(msgLength);
            message.InsertRange(0, bitsLength);

            return message;
        }
    }
}
