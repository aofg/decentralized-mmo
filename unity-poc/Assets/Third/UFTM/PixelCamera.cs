using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UFTM.Datatypes;
using UFTM.Interfaces;
using UnityEngine;

namespace UFTM
{
    public class PixelCamera : MonoBehaviour, IWorldData
    {
        struct Point2d
        {
            public int X;
            public int Y;

            public Point2d(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        
        private static ushort[] STANDARD_TILE = new ushort[] { 651 };
        
        [Range(1, 4)]
        public int PixelSize = 2;
        public int UnitSize = 32;
        private Camera camera;
        public Tilemap Map;
        private int mapX;
        private int mapY;

        private float pixelOffsetX;
        private float pixelOffsetY;

        private Dictionary<Point2d, ushort[]> diffs = new Dictionary<Point2d, ushort[]>(); 

        private void Awake()
        {
            Map.World = this;
        }

        private void Start()
        {
            camera = GetComponent<Camera>();
            if (camera == null)
            {
                Debug.LogError("Camera component not found");
                Destroy(this);
            }
        }

        private void Update()
        {
            camera.orthographicSize = (float) Screen.height / UnitSize / 2 / PixelSize;

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0f)
            {
                pixelOffsetX += Input.GetAxis("Horizontal") * Time.deltaTime * 10f * UnitSize;
            }

            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0f)
            {
                pixelOffsetY += Input.GetAxis("Vertical") * Time.deltaTime * 10f * UnitSize;
            }

            if (Mathf.Abs(pixelOffsetX) > 1f)
            {
                transform.position += Vector3.right * Mathf.FloorToInt(pixelOffsetX) / UnitSize;
                pixelOffsetX %= 1;
            }

            if (Mathf.Abs(pixelOffsetY) > 1f)
            {
                transform.position += Vector3.up * Mathf.FloorToInt(pixelOffsetY) / UnitSize;
                pixelOffsetY %= 1;
            }

//            var x = Mathf.RoundToInt((transform.position.x + mapX * 32) / 32);
//            var y = Mathf.RoundToInt((transform.position.y + mapY * 32) / 32);
            var x = Mathf.RoundToInt(transform.position.x / 32);
            var y = Mathf.RoundToInt(transform.position.y / 32);

            if (mapX != x || mapY != y)
            {
//                var deltaX = mapX - x;
//                var deltaY = mapY - y;
                
                mapX = x;
                mapY = y;
                
                Map.SetXY(mapX, mapY);
                
//                transform.position -= Vector3.left * deltaX * 32 + Vector3.down * deltaY * 32;
            }
        }

        public void SetTile(int x, int y, ushort id, int level = 0)
        {
            var hash = GetXYHash(x, y);
            if (!diffs.ContainsKey(hash))
            {
                var fill = new ushort[level + 1];
                for (int i = 0; i < level; i++)
                {
                    fill[i] = STANDARD_TILE[0];
                }

                fill[level] = id;

                diffs.Add(hash, fill);
            }
            else if (diffs[hash].Length > level)
            {
                diffs[hash][level] = id;
            }
            else
            {
                var tmp = diffs[hash];
                Array.Resize(ref tmp, level + 1);
                tmp[level] = id;
                diffs[hash] = tmp;
            }
        }

        public void SetBrush(int x1, int y1, int x2, int y2, ushort[] brush, int level = 0)
        {
            if (x1 > x2 || y1 > y2)
            {
                throw new ArgumentException("x1 and y1 should be less than x2 and y2");
            }
            
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    SetTile(x, y, brush[15], level);
                }
            }
            
            // TODO: diagonal isn't required!
            for (int x = x1 - 1; x <= x2 + 1; x++)
            {
                for (int y = y1 - 1; y <= y2 + 1; y++)
                {
                    UpdateBrush(x, y, brush, level);
                }
            }
        }

        private void UpdateBrush(int x, int y, ushort[] brush, int level)
        {
            var ids = brush;

            var current = GetTiles(x, y);
            
            if (IsSame(ids, current, level) == 0)
            {
                return;
            }
            
            var northEast = GetTiles(x + 1, y + 1);
            var southEast = GetTiles(x + 1, y - 1);
            var southWest = GetTiles(x - 1, y - 1);
            var northWest = GetTiles(x - 1, y + 1);

            var mask = IsSame(ids, northEast, level) << 0 | IsSame(ids, southEast, level) << 1 |
                       IsSame(ids, southWest, level) << 2 | IsSame(ids, northWest, level) << 3;

            SetTile(x, y, ids[mask], level);
        }

        private int IsSame(ushort[] brushTileIds, ushort[] another, int level)
        {
            return another.Length > level && brushTileIds.Any(id => id == another[level]) ? 1 : 0;
        }

        public ushort[] GetTiles(int x, int y)
        {
            var hash = GetXYHash(x, y);
            
            if (diffs.ContainsKey(hash))
            {
                return diffs[hash];
            }
            else
            {
                return STANDARD_TILE;
            }
        }

        private Point2d GetXYHash(int x, int y)
        {
            return new Point2d(x, y);
//            if (x == 0)
//            {
//                x = int.MinValue;
//            }
//
//            if (y == 0)
//            {
//                y = int.MinValue;
//            }
//            
//            int hash = 17;
//            // Suitable nullity checks etc, of course :)
//            hash = (hash * 16777619) ^ x.GetHashCode();
//            hash = (hash * 16777619) ^ y.GetHashCode();
//            return hash;
        }
    }
}