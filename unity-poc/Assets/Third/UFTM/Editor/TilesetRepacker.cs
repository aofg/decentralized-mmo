using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UFTM;
using UnityEditor;
using UnityEngine;

public class TilesetRepacker : EditorWindow {
    [MenuItem("Assets/Repack Selected")]
    public static void RepackSelected()
    {
        if (Selection.objects.Length <= 1)
        {
            throw new ArgumentException("Select more than one Tileset assets");
        }

        if (!Selection.objects.All(o => o is Tileset))
        {
            throw new ArgumentException("Please select only Tileset assets");
        }

        RepackIt(Selection.objects.Select(o => o as Tileset).ToArray());
    }

    private static void RepackIt(Tileset[] tilesets)
    {
        var size = tilesets[0].TileSize;
        
        var path = AssetDatabase.GetAssetPath(tilesets[0]);

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Tileset isn't asset");
        }
        
        var atlasPath = AssetDatabase.GenerateUniqueAssetPath(Path.GetDirectoryName(path) + "/Atlas.png");

        if (tilesets.Any(t => t.TileSize != size))
        {
            throw new ArgumentException("All tileset should share same size.. yet :)");
        }

        // TODO: magic numbers is worst thing what I could write
        var atlasCols = 2048 / size;
        var atlasRows = 2048 / size;
        
        var atlas = new Texture2D(2048, 2048, TextureFormat.ARGB32, false);
        var atlasIndex = 0;

        for (int tilesetIndex = 0; tilesetIndex < tilesets.Length; tilesetIndex++)
        {
            var tileset = tilesets[tilesetIndex];
            var cols = tileset.Cols;
            var rows = tileset.Rows;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var pixels = tileset.BaseTexture.GetPixels(col * size, row * size, size, size);

                    var atlasY = atlasIndex / atlasCols;
                    var atlasX = atlasIndex % atlasCols;
                    
                    atlas.SetPixels(atlasX * size, atlasY * size, size, size, pixels);

                    atlasIndex++;
                }
            }
        }

        File.WriteAllBytes(atlasPath, atlas.EncodeToPNG());
        AssetDatabase.Refresh();
    }
}
