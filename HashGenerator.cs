using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4DP
{
    public class HashGenerator
    {
        private uint[] _abcde = [0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0];
        private List<bool>[] _block160Bit = new List<bool>[5];
        private List<bool>[] _w = new List<bool>[80];

        public HashGenerator() 
        {
            for (int i = 0; i < _block160Bit.Length; i++)
            {
                _block160Bit[i] = BitsListExtensions.FromUint32ToBitList(_abcde[i]);
            }
        }

        public void GenerateHash(string fileName)
        {
            // к массиву байтов применить Reverse() чтобы биты в списке битов имели прямой порядок
        }

        /// <summary>
        /// Метод реализующий хеширование по методу SHA1.
        /// </summary>
        /// <param name="message"> Сообщение в виде списка битов. </param>
        /// <returns> Хэш в виде списка битов. </returns>
        public List<bool> SHA1(List<bool> message)
        {
            message = AddBitsToMessage(message);


            return null;
        }

        public List<bool>[] CompressionFunction(List<bool> message) 
        {

            return null;
        }

        /// <summary>
        /// Метод дополнения исходного сообщения битами.
        /// </summary>
        /// <param name="message"> Исходное сообщение. </param>
        /// <returns> Дополненное сообщение. </returns>
        public List<bool> AddBitsToMessage(List<bool> message)
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
