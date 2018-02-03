using System.Collections.Generic;
using Rentitas;
using Revalue;
using Uniful;

namespace Reviews
{
    public class SetupViewContextSystem : IReactiveSystem<IViewPool>, ISetPools
    {
        private Pools _context;

        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach (var viewEntity in entities)
            {
                var go = viewEntity.Get<InScene>().Object;
                var root = go.RequireComponent<ViewRoot>();
                root.SetPools(_context);

                viewEntity.SetValue<IViewPool, Root, ViewRoot>(root);

                if(!viewEntity.Is<Disabled>())
                    go.SetActive(true);
            }
        }

        public TriggerOnEvent Trigger => Matcher.AllOf(typeof (InScene)).OnEntityAdded();
        public void SetPools(Pools pools)
        {
            _context = pools;
        }
    }
}