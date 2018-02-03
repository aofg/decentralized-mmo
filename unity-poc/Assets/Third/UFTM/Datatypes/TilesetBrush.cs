using UnityEngine;

namespace UFTM.Datatypes
{
    [System.Serializable]
    public class TilesetBrush
    {
        public int Icon;
        public string Name;
        public string Description;
        public int[] TileIds = new int[16];
    }
}