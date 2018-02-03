using System.Collections.Generic;
using Rentitas;

namespace Reviews
{
    public class ParentObjectSystem : IReactiveSystem<IViewPool>, ISetPools
    {
        private Pool<IViewPool> _views;

        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach (var viewEntity in entities)
            {
                AttachObject(viewEntity);
            }
        }

        private void AttachObject(Entity<IViewPool> viewEntity)
        {
            if (!viewEntity.Has<InScene>()) return;

            if (viewEntity.Has<Parent>())
            {
                var go = viewEntity.Get<InScene>();
                var parentComponent = viewEntity.Get<Parent>();
                var parent = _views.GetByIdentity(parentComponent.ParentId);
                var parentObject = parent.Get<InScene>().Object;
                go.Object.transform.SetParent(parentObject.transform);
            }
            else
            {
                var go = viewEntity.Get<InScene>();
                go.Object.transform.parent = null;
            }

            viewEntity.ForceUpdateTransform();
        }

        public TriggerOnEvent Trigger => Matcher.AllOf(typeof(InScene), typeof (Parent)).OnEntityAddedOrRemoved();

        public void SetPools(Pools pools)
        {
            _views = pools.Get<IViewPool>();
        }
    }
}