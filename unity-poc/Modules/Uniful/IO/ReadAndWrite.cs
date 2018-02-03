using System.IO;
using UnityEngine;

namespace Uniful
{
    public static partial class IO
    {
        public static FileStream GetFile(string path, FileMode mode = FileMode.OpenOrCreate)
        {
            return GetFileAtAbsolutePath(GetAbsolutePath(path), mode);
        }

        private static FileStream GetFileAtAbsolutePath(string path, FileMode mode = FileMode.OpenOrCreate)
        {
            return new FileStream(path, mode, FileAccess.ReadWrite);
        }

        public static byte[] ReadAll(string path)
        {
            return File.ReadAllBytes(GetAbsolutePath(path));
        }

        public static bool WriteAtStart(string path, byte[] data)
        {
            using (var file = GetFile(path, FileMode.Open))
            {
                file.Write(data, 0, data.Length);
            }

            return true;
        }

        public static bool WriteAtEnd(string path, byte[] data)
        {
            using (var file = GetFile(path, FileMode.Append))
            {
                file.Write(data, 0, data.Length);
            }

            return true;
        }

        public static bool Exists(string path)
        {
            return File.Exists(GetAbsolutePath(path));
        }

        public static bool Rewrite(string path, byte[] data)
        {
            using (var file = GetFile(path, Exists(path) ? FileMode.Truncate : FileMode.Create))
            {
                file.Write(data, 0, data.Length);
            }

            return true;
        }

        private static string GetAbsolutePath(string relative)
        {
            return Path.Combine(Application.persistentDataPath, relative);
        }
    }
}