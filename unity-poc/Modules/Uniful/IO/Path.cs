using System.IO;

namespace Uniful
{
    public static class UnifulPath
    {
        public static string Combine(string path, params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return path;

            foreach (var part in parts)
            {
                path = Path.Combine(path, part);
            }

            return path.Replace("\\", "/");
        }
    }
}