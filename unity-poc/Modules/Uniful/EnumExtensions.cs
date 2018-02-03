using System;

namespace Uniful
{
    public static class EnumExtensions
    {
		/// <summary>
		/// Tries to parse string value to Enum.
		/// </summary>
		/// <returns><c>true</c>, if parse was success, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="result">Result.</param>
		/// <typeparam name="TEnum">Generic enum type.</typeparam>
		public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
		{
			var retValue = value == null ? false : Enum.IsDefined(typeof(TEnum), value);
			result = retValue ? (TEnum)Enum.Parse(typeof(TEnum), value) : default(TEnum);
			return retValue;
		}

        /// <summary>
		/// Determines if enum value has flag. (Mono 3.5 hack)
        /// </summary>
        /// <returns><c>true</c> if value has flag; otherwise, <c>false</c>.</returns>
        /// <param name="value">Value.</param>
        /// <param name="flag">Flag.</param>
        public static bool HasFlag(this Enum value, Enum flag)
        {
            long num = Convert.ToInt64(flag);
			return HasFlag (value, num);
        }

		/// <summary>
		/// Determines if has flag the specified value flag.
		/// </summary>
		/// <returns><c>true</c> if has flag the specified value flag; otherwise, <c>false</c>.</returns>
		/// <param name="value">Value.</param>
		/// <param name="flag">Flag.</param>
		public static bool HasFlag(this Enum value, byte flag) {
			return (Convert.ToByte(value) & flag) == flag;
		}

		/// <summary>
		/// Determines if has flag the specified value flag.
		/// </summary>
		/// <returns><c>true</c> if has flag the specified value flag; otherwise, <c>false</c>.</returns>
		/// <param name="value">Value.</param>
		/// <param name="flag">Flag.</param>
		public static bool HasFlag(this Enum value, int flag) {
			return (Convert.ToInt32(value) & flag) == flag;
		}

		/// <summary>
		/// Determines if has flag the specified value flag.
		/// </summary>
		/// <returns><c>true</c> if has flag the specified value flag; otherwise, <c>false</c>.</returns>
		/// <param name="value">Value.</param>
		/// <param name="flag">Flag.</param>
		public static bool HasFlag(this Enum value, long flag) {
			return (Convert.ToInt64(value) & flag) == flag;
		}
    }
}