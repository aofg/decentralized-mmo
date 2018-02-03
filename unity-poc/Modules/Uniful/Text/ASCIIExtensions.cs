using System.Linq;

namespace Uniful.Text
{
    public static class ASCIIExtensions
    {
        public static bool IsASCII(this string text)
        {
            return text.Any(c => (int) c > byte.MaxValue);
        }

        public static bool IsAZ(this string text)
        {
            return text.Any(c => (int) c > 127 || (int) c < 32);
        }
    }
}