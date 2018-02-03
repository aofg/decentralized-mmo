using UnityEngine;

namespace UI.Helpers
{
    public static class RectTransformExtensions
    {
        public static void RemoveAllChildren(this RectTransform rect)
        {
            foreach (Transform child in rect)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}