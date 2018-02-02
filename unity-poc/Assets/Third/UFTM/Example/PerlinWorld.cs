using System.Collections;
using System.Collections.Generic;
using UFTM.Datatypes;
using UFTM.Interfaces;
using UnityEngine;

public class PerlinWorld : MonoBehaviour, IWorldData {
    [System.Serializable]
    public class PerlinTileWeight
    {
        public int TileId;
        [Range(0f, 0.999f)]
        public float Weight;
    }
    
    public PerlinTileWeight[] Weights;
    public TilesetBrush Stones;
    public TilesetBrush Dirt;

    public float scale = 0.2f;

    public float GetRandomAt(int x, int y)
    {
//        return Mathf.PerlinNoise((32 + 16 + x) / (32f * 3) * 20, (32 + 16 + y) / (32f * 3) * 20);
        return Mathf.PerlinNoise(x * scale, y * scale);
    }
    
    public ushort[] GetTiles(int x, int y)
    {
        x *= 2;
        y *= 2;
        
        var output = new List<ushort>();
        var current = GetRandomAt(x, y);

        var northEast = GetRandomAt(x + 1, y + 1);
        var southEast = GetRandomAt(x + 1, y - 1);
        var southWest  = GetRandomAt(x - 1, y - 1);
        var northWest  = GetRandomAt(x - 1, y + 1);
        
        output.Add(651);

        var stoneMask = IsStone(northEast) << 0 | IsStone(southEast) << 1 | IsStone(southWest) << 2 | IsStone(northWest) << 3;
        output.Add((ushort)(Stones.TileIds[stoneMask] - 1));

        var dirtMask = IsDirt(northEast) << 0 | IsDirt(southEast) << 1 | IsDirt(southWest) << 2 | IsDirt(northWest) << 3;
        output.Add((ushort)(Dirt.TileIds[dirtMask] - 1));
        
        return output.ToArray();
    }

    private int IsStone(float noise)
    {
        return noise > 0.65f ? 1 : 0;
    }

    private int IsDirt(float noise)
    {
        return noise > 0.85f ? 1 : 0;
    }

//    public int WangTile(int x, int y)
//    {
//        
//    }
}
