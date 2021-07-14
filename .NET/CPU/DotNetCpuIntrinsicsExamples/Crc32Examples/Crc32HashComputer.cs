using System;
using System.Runtime.Intrinsics.X86;

namespace Crc32Examples
{
    public static class Crc32HashComputer
    {
        /// <summary>
        /// We can further optimize the algorithm when X64 is available.
        /// </summary>
        private static readonly bool _x64Available;

        /// <summary>
        /// Default constructor
        /// </summary>
        static Crc32HashComputer()
        {
            if (!Sse42.IsSupported)
            {
                throw new NotSupportedException("SSE4.2 is not supported");
            }

            _x64Available = Sse42.X64.IsSupported;

            // The size, in bits, of the computed hash code.
        }

        public static int ComputeInt32Number(byte[] array)
        {
            int ibStart = 0;
            int cbSize = array.Length;
            uint crcNumber = 0;

            while (cbSize > 0)
            {
                crcNumber = Sse42.Crc32(crcNumber, array[ibStart]);
                ibStart++;
                cbSize--;
            }

            //Span<byte> crcNumberBytes = stackalloc byte[8];

            //BitConverter.TryWriteBytes(crcNumberBytes, crcNumber);

            byte[] crcNumberBytes = BitConverter.GetBytes(crcNumber);

            int signedCrcNumber = BitConverter.ToInt32(crcNumberBytes);

            return signedCrcNumber;
        }

        public static long ComputeInt64Number(byte[] array)
        {
            int ibStart = 0;
            int cbSize = array.Length;
            ulong crcNumber = 0;

            if (_x64Available)
            {
                while (cbSize >= 8)
                {
                    crcNumber = Sse42.X64.Crc32(crcNumber, BitConverter.ToUInt64(array));
                    ibStart += 8;
                    cbSize -= 8;
                }
            }

            while (cbSize > 0)
            {
                crcNumber = Sse42.X64.Crc32(crcNumber, array[ibStart]);
                ibStart++;
                cbSize--;
            }

            //Span<byte> crcNumberBytes = stackalloc byte[8];

            //BitConverter.TryWriteBytes(crcNumberBytes, crcNumber);

            byte[] crcNumberBytes = BitConverter.GetBytes(crcNumber);

            long signedCrcNumber = BitConverter.ToInt64(crcNumberBytes);

            return signedCrcNumber;
        }
    }
}
