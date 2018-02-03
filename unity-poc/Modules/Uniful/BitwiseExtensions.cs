using System;

namespace Uniful
{
    public static class BitwiseExtensions
    {
        /// <summary>
        /// Sets the bit in short value
        /// </summary>
        /// <returns>Final short value after change bit at position.</returns>
        /// <param name="current">Current short value.</param>
        /// <param name="position">Position to set bit.</param>
        /// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static int SetBit(this int current, byte position, bool on = true)
        {
            if (position > 31)
                throw new ArgumentOutOfRangeException("Int position can't be more than 31");

            if (on)
                current = current | (1 << position);
            else
                current = current & ~(1 << position);

            return current;
        }

        /// <summary>
        /// Sets the bit in short value
        /// </summary>
        /// <returns>Final short value after change bit at position.</returns>
        /// <param name="current">Current short value.</param>
        /// <param name="position">Position to set bit.</param>
        /// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static uint SetBit(this uint current, byte position, bool on = true)
        {
            if (position > 31)
                throw new ArgumentOutOfRangeException("Uint position can't be more than 31");

            if (on)
                current = (uint)(current | (uint)(1 << position));
            else
                current = (uint)(current & ~(uint)(1 << position));

            return current;
        }

        /// <summary>
        /// Sets the bit in short value
        /// </summary>
        /// <returns>Final short value after change bit at position.</returns>
        /// <param name="current">Current short value.</param>
        /// <param name="position">Position to set bit.</param>
        /// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static long SetBit(this long current, byte position, bool on = true)
        {
            if (position > 63)
                throw new ArgumentOutOfRangeException("Long position can't be more than 63");

            if (on)
                current = (long)(current | (long)(1 << position));
            else
                current = (long)(current & ~(long)(1 << position));

            return current;
        }

        /// <summary>
        /// Sets the bit in short value
        /// </summary>
        /// <returns>Final short value after change bit at position.</returns>
        /// <param name="current">Current short value.</param>
        /// <param name="position">Position to set bit.</param>
        /// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static ulong SetBit(this ulong current, byte position, bool on = true)
        {
            if (position > 63)
                throw new ArgumentOutOfRangeException("ULong position can't be more than 63");

            if (on)
                current = (ulong)(current | (ulong)(1 << position));
            else
                current = (ulong)(current & ~(ulong)(1 << position));

            return current;
        }

        /// <summary>
        /// Sets the bit in short value
        /// </summary>
        /// <returns>Final short value after change bit at position.</returns>
        /// <param name="current">Current short value.</param>
        /// <param name="position">Position to set bit.</param>
        /// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static short SetBit(this short current, byte position, bool on = true)
        {
            if (position > 15)
                throw new ArgumentOutOfRangeException("Short position can't be more than 15");

            if (on)
                current = (short)(current | (short)(1 << position));
            else
                current = (short)(current & ~(short)(1 << position));

            return current;
        }

        /// <summary>
        /// Sets the bit in short value
        /// </summary>
        /// <returns>Final short value after change bit at position.</returns>
        /// <param name="current">Current short value.</param>
        /// <param name="position">Position to set bit.</param>
        /// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static ushort SetBit(this ushort current, byte position, bool on = true)
        {
            if (position > 15)
                throw new ArgumentOutOfRangeException("UShort position can't be more than 15");

            if (on)
                current = (ushort)(current | (ushort)(1 << position));
            else
                current = (ushort)(current & ~(ushort)(1 << position));

            return current;
        }

        /// <summary>
        /// Sets the bit in byte value
        /// </summary>
        /// <returns>Final byte value after change bit at position.</returns>
        /// <param name="current">Current byte value.</param>
        /// <param name="position">Position to set bit.</param>
		/// <param name="on">ONE if true; otherwise is ZERO.</param>
        public static byte SetBit(this byte current, byte position, bool on = true)
        {
            if(position > 7)
                throw new ArgumentOutOfRangeException("Byte position can't be more than 7");

            if (on)
                current = (byte)(current | (1 << position));
            else
                current = (byte)(current & ~(1 << position));

            return current;
        }

        /// <summary>
        /// Returns mask value with specified count of bits starts from offset
        /// </summary>
        /// <param name="countBits">Final count of mask bits</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int GetMask(byte countBits, byte offset = 0)
        {
            return ((1 << countBits) - 1) << offset;
        }

        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this long value, long mask)
        {
            return (value & mask) > 0;
        }
        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this ulong value, ulong mask)
        {
            return (value & mask) > 0;
        }

        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this int value, int mask)
        {
            return (value & mask) > 0;
        }

        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this uint value, uint mask)
        {
            return (value & mask) > 0;
        }

        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this short value, short mask)
        {
            return (value & mask) > 0;
        }

        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this ushort value, ushort mask)
        {
            return (value & mask) > 0;
        }

        /// <summary>
        /// Check if current value has any of mask bits
        /// </summary>
        /// <returns><c>true</c> if has one or more bits from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool Any(this byte value, byte mask)
        {
            return (value & mask) > 0;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this long value, long mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this ulong value, ulong mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this int value, int mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this uint value, uint mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this short value, short mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this ushort value, ushort mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if has all from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool All(this byte value, byte mask)
        {
            return (value & mask) > mask;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this long value, long mask)
        {
            return (value & mask) == 0;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this ulong value, ulong mask)
        {
            return (value & mask) == 0;
        }

        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this int value, int mask)
        {
            return (value & mask) == 0;
        }


        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this uint value, uint mask)
        {
            return (value & mask) == 0;
        }


        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this short value, short mask)
        {
            return (value & mask) == 0;
        }


        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this ushort value, ushort mask)
        {
            return (value & mask) == 0;
        }


        /// <summary>
        /// Check if current value has all of mask bits
        /// </summary>
        /// <returns><c>true</c> if haven't any bit from mask; otherwise, <c>false</c>.</returns>
        /// <param name="value">Current value</param>
        /// <param name="mask">Checking mask</param>
        public static bool None(this byte value, byte mask)
        {
            return (value & mask) == 0;
        }


        /// <summary>
        /// Dump to bit string the specified byte.
        /// </summary>
        /// <param name="b">The byte value.</param>
        public static string Dump(this byte b) {
			return Convert.ToString(b, 2);
		}

		/// <summary>
		/// Dump to bit string the specified short integer.
		/// </summary>
		/// <param name="s">The short integer value.</param>
		public static string Dump(this short s) {
			return Convert.ToString(s, 2);
		}

		/// <summary>
		/// Dump to bit string the specified integer.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public static string Dump(this int i) {
			return Convert.ToString(i, 2);
		}

        /// <summary>
        /// Pops the count.
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="b">The blue component.</param>
        public static int PopCount(this int b)
        {
            int i = b;
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}