using UnityEngine;

namespace Reviews
{
    public abstract class ViewBehaviour : MonoBehaviour
    {
        protected bool IsReady { get; private set; }

        private ViewRoot _root;
        public ViewRoot Root => GetRoot();

        private ViewRoot GetRoot()
        {
            if (!_root)
                _root = GetComponentInParent<ViewRoot>();

            return _root;
        }

        protected virtual void Start()
        {
            var root = transform.GetComponentInParent<ViewRoot>();
            if (root)
            {
                root.RegisterBehaviour(this);
                IsReady = true;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}