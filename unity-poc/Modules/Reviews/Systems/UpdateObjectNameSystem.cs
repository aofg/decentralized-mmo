using System.Collections.Generic;
using Rentitas;

namespace Reviews
{
    public class UpdateObjectNameSystem : IReactiveSystem<IViewPool>
    {
        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach (var viewEntity in entities)
            {
                UpdateName(viewEntity);
            }
        }

        private void UpdateName(Entity<IViewPool> viewEntity)
        {
            var go = viewEntity.Get<InScene>().Object;
            var name = viewEntity.Get<Name>().Value;

            go.name = name;
        }

        public TriggerOnEvent Trigger => Matcher.AllOf(typeof (Name), typeof (InScene)).OnEntityAdded();
    }
}