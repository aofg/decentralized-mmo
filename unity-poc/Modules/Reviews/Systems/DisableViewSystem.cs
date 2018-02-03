using System.Collections.Generic;
using Rentitas;

namespace Reviews
{
    public class DisableViewSystem : IReactiveSystem<IViewPool>
    {
        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach (var viewEntity in entities)
            {
                if (!viewEntity.Has<InScene>())
                    continue;

                viewEntity.Get<InScene>().Object.SetActive(!viewEntity.Is<Disabled>());
            }
        }

        public TriggerOnEvent Trigger => Matcher.AllOf(typeof(InScene), typeof (Disabled)).OnEntityAddedOrRemoved();
    }
}