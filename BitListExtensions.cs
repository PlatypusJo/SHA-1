﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab4DP
{
    /// <summary>
    /// Методы расширения для BitArray и List<bool>.
    /// </summary>
    public static class BitsListExtensions
    {
        public static List<bool> ToBitsList(this BitArray bitArr)
        {
            bool[] bits = new bool[bitArr.Length];
            bitArr.CopyTo(bits, 0);
            return bits.ToList();
        }

        public static byte FromBitsToByte(this List<bool> bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("Кол-во битов превышает размер байта");

            byte mask = (byte)(1 << bits.Count - 1);
            byte number = 0;
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    number |= mask;
                mask >>= 1;
            }

            return number;
        }

        public static List<bool> LeftShift(this List<bool> bits, int count)
        {
            BitArray bitArr = new BitArray(bits.ToArray());
            return bitArr.LeftShift(count).ToBitsList();
        }

        public static List<bool> RightShift(this List<bool> bits, int count)
        {
            BitArray bitArr = new BitArray(bits.ToArray());
            return bitArr.RightShift(count).ToBitsList();
        }

        public static List<bool> Not(this List<bool> bits) => new BitArray(bits.ToArray()).Not().ToBitsList();

        public static List<bool> Or(this List<bool> bits, List<bool> value)
        {
            BitArray bitArr1 = new BitArray(bits.ToArray());
            BitArray bitArr2 = new BitArray(value.ToArray());
            return bitArr1.Or(bitArr2).ToBitsList();
        }

        public static List<bool> And(this List<bool> bits, List<bool> value)
        {
            BitArray bitArr1 = new BitArray(bits.ToArray());
            BitArray bitArr2 = new BitArray(value.ToArray());
            return bitArr1.And(bitArr2).ToBitsList();
        }

        public static List<bool> Xor(this List<bool> bits, List<bool> value)
        {
            BitArray bitArr1 = new BitArray(bits.ToArray());
            BitArray bitArr2 = new BitArray(value.ToArray());
            return bitArr1.Xor(bitArr2).ToBitsList();
        }

        public static List<bool> FromUint32ToBitList(uint value)
        {
            byte[] bits32 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bits32[i] = (byte)value;
                value >>= 8;
            }
            return new BitArray(bits32).ToBitsList();
        }

        public static List<bool> FromUint64ToBitList(ulong value)
        {
            byte[] bits64 = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bits64[i] = (byte)value;
                value >>= 8;
            }
            return new BitArray(bits64).ToBitsList();
        }

        public static uint FromBitListToUint32(this List<bool> bits)
        {
            byte[] numberBytes = new byte[4];
            new BitArray(bits.ToArray()).CopyTo(numberBytes, 0);
            return BitConverter.ToUInt32(numberBytes, 0);
        }
    }
}
