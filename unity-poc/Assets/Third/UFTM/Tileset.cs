using UFTM.Datatypes;
using UnityEngine;

namespace UFTM
{
    [CreateAssetMenu(fileName ="Tileset.asset", menuName = "Tileset", order = 0)]
    public class Tileset : ScriptableObject
    {
        public Texture2D BaseTexture;
        public int TileSize;
        public TilesetBrush[] Brushes;

        public int Rows
        {
            get { return BaseTexture.height / TileSize; }
        }

        public int Cols
        {
            get { return BaseTexture.width / TileSize; }
        }

        public int Count
        {
            get { return Rows * Cols; }
        }
    }
}