using System.Collections.Generic;
using Rentitas;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Reviews
{
    public class CreateObjectForIdentity : IReactiveSystem<IViewPool>, ISetPools
    {
        private Pools _context;
        public TriggerOnEvent Trigger => Matcher.AllOf(typeof(Unique)).OnEntityAdded();

        public void Execute(List<Entity<IViewPool>> entities)
        {
            foreach(var viewEntity in entities)
            {
                CreateObject(viewEntity);
            }
        }

        private void CreateObject(Entity<IViewPool> viewEntity)
        {
            if (viewEntity.Has<InScene>()) return;

            var go = viewEntity.Need<InScene>();
            if (viewEntity.Has<Prototype>())
            {
                if (go.Object != null)
                    Object.Destroy(go.Object);

                go.Object = Object.Instantiate(viewEntity.Get<Prototype>().Prefab);
            }
            else
            {
                if (go.Object == null)
                    go.Object = new GameObject("view");
                else
                    foreach (var component in go.Object.GetComponents<MonoBehaviour>())
                        Object.Destroy(component);
            }

            go.Object.name = $"View {viewEntity.Get<Unique>().Id}";
            viewEntity.ReplaceInstance(go);

        }

        public void SetPools(Pools pools)
        {
            _context = pools;
        }
    }
}
