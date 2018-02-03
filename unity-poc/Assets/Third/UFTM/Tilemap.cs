using UFTM.Interfaces;
using UnityEngine;

namespace UFTM
{
    public class Tilemap : MonoBehaviour
    {
        public IWorldData World;
        public Tileset Tileset;
        private Renderer[] renderers;
        private RenderTexture atlas;
        private Material Material;

        private int worldX;
        private int worldY;
        
        private void Start()
        {
            if (World == null)
            {
                World = GetComponent<IWorldData>();
            }
            
            Material = new Material(Shader.Find("Unlit/Transparent Cutout"));
            Material.mainTexture = Tileset.BaseTexture;
            
            renderers = new Renderer[9];
            for (int index = 0; index < 9; index++)
            {
                renderers[index] = CreateRenderSystem(Tileset);
            }

            OrganizeSystems();
            LoadWorld(true);
        }

        public void SetXY(int x, int y, bool refresh = true)
        {
            if (worldX != x || worldY != y)
            {
                worldX = x;
                worldY = y;
                OrganizeSystems();
                LoadWorld(refresh);
            }
        }

        private void LoadWorld(bool whole)
        {
            if (whole)
            {
                for (int row = -1; row < 2; row++)
                {
                    for (int col = -1; col < 2; col++)
                    {
                        LoadWorldAt(row, col);
                    }
                }
            }
            else
            {
                LoadWorldAt(0, 0);
            }
        }

        private void LoadWorldAt(int row, int col)
        {
            var offsetX = worldX * Renderer.COLS;
            var offsetY = worldY * Renderer.ROWS;
            var halfChunkX = Renderer.COLS / 2 + col * Renderer.COLS;
            var halfChunkY = Renderer.ROWS / 2 + row * Renderer.ROWS;
            
            var rendererIndex = (row + 1) * 3 + (col + 1);
            for (int y = 0; y < Renderer.ROWS; y++)
            {
                for (int x = 0; x < Renderer.COLS; x++)
                {
                    var tiles = World.GetTiles(offsetX + x - halfChunkX, offsetY + y - halfChunkY);
                    
                    renderers[rendererIndex].RemoveTiles(x, y);
                        
                    for (var tileIndex = 0; tileIndex < tiles.Length; tileIndex++)
                    {
                        renderers[rendererIndex].ShowTile(x, y, tiles[tileIndex], tileIndex);   
                    }
                }
            }
        }

        private void OrganizeSystems()
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    var index = (y + 1) * 3 + (x + 1);
                    renderers[index].gameObject.transform.localPosition =
                        Vector3.right * Renderer.COLS * (x + worldX) + Vector3.up * Renderer.ROWS * (y + worldY);
                }
            }
        }


        private Renderer CreateRenderSystem(Tileset tileset)
        {
            var systemgo = new GameObject("Tileset Rendering");
            var renderer = systemgo.AddComponent<Renderer>();
            renderer.Tileset = tileset;
            renderer.SharedMaterial = Material;
            
            systemgo.transform.SetParent(transform, false);

            return renderer;
        }

        public void RefreshAt(int x, int y)
        {
            LoadWorldAt((x - worldX) / Renderer.COLS, (y - worldY) / Renderer.ROWS);
        }
    }
}
