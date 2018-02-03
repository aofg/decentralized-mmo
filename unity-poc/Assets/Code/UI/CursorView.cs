using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CursorView : AbstractView
    {
        public bool Snap;
        public bool SnapInWorld;
        public int SnapSize = 64;
        public Vector2 Offset;
        
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
            Cursor.visible = false;
        }

        private void Update()
        {
            var mouse = Input.mousePosition;
            if (Snap)
            {
                if (!SnapInWorld)
                {
                    mouse.x = Mathf.RoundToInt(mouse.x / SnapSize);
                    mouse.y = Mathf.RoundToInt(mouse.y / SnapSize);
                    mouse *= SnapSize;
                }
                else
                {
                    var world = camera.ScreenToWorldPoint(mouse);
                    world.x = Mathf.RoundToInt((world.x - Offset.x) / SnapSize) + Offset.x;
                    world.y = Mathf.RoundToInt((world.y - Offset.y) / SnapSize) + Offset.y;
                    world *= SnapSize;

                    mouse = camera.WorldToScreenPoint(world);
                }

            }
            
            Rect.position = mouse;
        }
    }
}