using System.Collections.Generic;
using Rentitas;
using UnityEngine;

namespace Reviews
{
    public class DestroyViewSystem : IReactiveSystem<IViewPool>, ISetPools, ICleanupSystem
    {
        private Pool<IViewPool> _viewPool;
        private Group<IViewPool> _removing;

        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach (var entity in entities)
            {
                var inscene = entity.Get<InScene>();
                Object.Destroy(inscene.Object);
                entity.Toggle<Destroyed>(true);
            }

        }

        public TriggerOnEvent Trigger => Matcher.AllOf(typeof(Destroying), typeof(InScene)).OnEntityAdded();

        public void SetPools(Pools pools)
        {
            _viewPool = pools.Get<IViewPool>();
            _removing = _viewPool.GetGroup(Matcher.AllOf(typeof(Destroyed)));
        }

        public void Cleanup()
        {
            foreach (var entity in _removing.GetEntities())
            {
                _viewPool.DestroyEntity(entity);
            }
        }
    }
}