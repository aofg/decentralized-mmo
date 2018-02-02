using System.Collections.Generic;
using UFTM.Interfaces;
using UnityEngine;

namespace UFTM
{
    public class PixelCamera : MonoBehaviour, IWorldData
    {
        [Range(1, 4)]
        public int PixelSize = 2;
        public int UnitSize = 32;
        private Camera camera;
        public Tilemap Map;
        private int mapX;
        private int mapY;

        private float pixelOffsetX;
        private float pixelOffsetY;

        private Dictionary<int, ushort[]> diffs = new Dictionary<int, ushort[]>();

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

        public ushort[] GetTiles(int x, int y)
        {
            var hash = GetXYHash(x, y);
            
            if (diffs.ContainsKey(hash))
            {
                return diffs[hash];
            }
            else
            {
                return new ushort[] { 651 };
            }
        }

        public int GetXYHash(int x, int y)
        {
            unchecked // integer overflows are accepted here
            {
                int hashCode = 0;
                hashCode = (hashCode * 397) ^ x;
                hashCode = (hashCode * 397) ^ y;
                return hashCode;
            }
        }
    }
}