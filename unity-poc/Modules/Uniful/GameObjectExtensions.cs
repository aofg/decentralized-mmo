using UnityEngine;

namespace Uniful
{
    public static class GameObjectExtensions
    {
        public static T RequireComponent<T>(this GameObject @this) where T : Component
        {
            var found = @this.GetComponent<T>();
            if (!found)
                found = @this.AddComponent<T>();

            return found;
        }


        public static void ResetTrasform(this GameObject @this)
        {
            @this.transform.Reset();
        }

        public static void Reset(this Transform @this)
        {
            @this.localPosition = Vector3.zero;
            @this.localScale = Vector3.one;
            @this.localRotation = Quaternion.identity;
        }
    }
}