using UnityEngine;

namespace UI
{
    public class AbstractView : MonoBehaviour
    {
        private RectTransform cachedRect;

        public RectTransform Rect
        {
            get
            {
                if (cachedRect == null)
                {
                    cachedRect = transform as RectTransform;
                }
                return cachedRect;
            }
        }
    }
}