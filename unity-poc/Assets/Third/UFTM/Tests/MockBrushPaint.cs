using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UFTM.Tests
{
    public class MockBrushPaint : MonoBehaviour
    {
        public Tilemap Map;
        public PixelCamera Mocking;
        public Tileset Atlas;

        private Dictionary<int, ushort[]> ids = new Dictionary<int, ushort[]>();
        private int lastX;
        private int lastY;
        
        private Camera cachedCamera;
        protected Camera camera
        {
            get
            {
                if (cachedCamera == null)
                {
                    cachedCamera = Camera.main;
                }
                
                return cachedCamera;
            }
        }

        private void Start()
        {
            for (var brushIndex = 0; brushIndex < Atlas.Brushes.Length; brushIndex++)
            {
                ids.Add(brushIndex, Atlas.Brushes[brushIndex].TileIds.Select(id => (ushort)(Mathf.Max(0, id - 1))).ToArray());
            }
            
            Mocking.SetBrush(2, 2, 3, 3, ids[1], 1);
            Mocking.SetBrush(2, 4, 3, 5, ids[1], 1);
            Mocking.SetBrush(3, 4, 4, 5, ids[1], 1);
            Mocking.SetTile(0, 0, 724, 2);
            Mocking.SetTile(2, 3, 724, 2);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mouse = Input.mousePosition;
                var world = camera.ScreenToWorldPoint(mouse);
                var x = Mathf.RoundToInt(world.x - 0.5f);
                var y = Mathf.RoundToInt(world.y - 0.5f);

                if (lastX != x || lastY != y)
                {
                    lastX = x;
                    lastY = y;
                    Debug.LogWarningFormat("{0}, {1}", x, y);
                    Mocking.SetBrush(x, y, x + 1, y + 1, ids[1], 1);

                    Map.RefreshAt(x, y);
                }
            }
        }
    }
}