using System.Collections.Generic;
using Rentitas;

namespace Reviews
{
    public class TransformObjectSystem : IReactiveSystem<IViewPool>
    {
        public TriggerOnEvent Trigger => Matcher.AllOf(typeof (Transform), typeof(InScene)).OnEntityAdded();

        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach (var viewEntity in entities)
            {
                TransformObject(viewEntity);
            }
        }

        private void TransformObject(Entity<IViewPool> viewEntity)
        {
            var go = viewEntity.Get<InScene>().Object;
            var transformComponent = viewEntity.Get<Transform>();
            var tr = go.transform;

            if (tr.localPosition != transformComponent.Position)
            {
                tr.localPosition = transformComponent.Position;
            }

            if (tr.localRotation != transformComponent.Rotation)
            {
                tr.localRotation = transformComponent.Rotation;
            }

            if (tr.localScale != transformComponent.Scale)
            {
                tr.localScale = transformComponent.Scale;
            }
        }
    }
}
