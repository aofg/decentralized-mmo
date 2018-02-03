using UnityEngine;

namespace UI
{
    public class SnapToTileView : AbstractView
    {
//        public Vector2 Offset;
//        public int Snap = 1;
//        
//        private Camera cachedCamera;
//        private Vector3 initialOffset;
//
//        private Vector3 delta;
//        
//        private void Start()
//        {
//            cachedCamera = Camera.main;
//            initialOffset = Rect.localPosition;
//            Cursor.visible = false;
//        }
//        private void LateUpdate()
//        {
//            delta += Rect.localPosition - initialOffset;
//            Rect.localPosition = initialOffset;
//
//            if (Mathf.Abs(delta.x) > 1f)
//            {
//                
//            }
//            var screen = Rect.position;
//            var world = cachedCamera.ScreenToWorldPoint(screen);
//
//            var intX = Mathf.RoundToInt(world.x - Offset.x) + Offset.x;
//            var intY = Mathf.RoundToInt(world.y - Offset.y) + Offset.y;
//
//            var snapped = cachedCamera.WorldToScreenPoint(new Vector3(intX, intY, 0));
//
//            Rect.position = snapped;
//        }
    }
}