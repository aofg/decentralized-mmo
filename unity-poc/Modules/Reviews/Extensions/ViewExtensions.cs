using System;
using Rentitas;
using UnityEngine;

namespace Reviews
{
    public static class ViewExtensions
    {
        public static Entity<IViewPool> GetByIdentity(this Pool<IViewPool> pool, Guid identity)
        {
            return ((ViewPool) pool).IdIndex.TryGetEntity(identity);
        }

        public static Entity<IViewPool> CreateChild(
            this Pool<IViewPool> pool, 
            Guid identity, 
            GameObject prefab = null, 
            bool realInstance = false, 
            bool resetTransform = true)
        {
            var view = pool.GetByIdentity(identity);
            if (view == null) return null;

            var child = pool.CreateView(prefab, realInstance, resetTransform);

            return child.Add<Parent>(p => p.ParentId = identity);
        }

        public static Entity<IViewPool> CreateView(
            this Pool<IViewPool> pool, 
            GameObject prefab = null, 
            bool realInstance = false, 
            bool resetTransform = true)
        {
            var entity = pool.CreateEntity();
            var identity = entity.CreateComponent<Unique>();
            identity.Id = Guid.NewGuid();


            if (prefab)
            {
                if (!realInstance)
                {
                    var prototype = entity.CreateComponent<Prototype>();
                    prototype.Prefab = prefab;
                    entity.AddInstance(prototype);
                }
                else
                {
                    entity.Add<InScene>(s => s.Object = prefab);
                }
            }


            entity.AddInstance(identity);
            if (resetTransform)
                entity.ResetTransform();
            return entity;
        }

        public static Entity<IViewPool> ResetTransform(this Entity<IViewPool> entity)
        {
            var transform = entity.CreateComponent<Transform>();
            transform.Scale = Vector3.one;
            transform.Position = Vector3.zero;
            transform.Rotation = Quaternion.identity;

            return entity.SetInstance(transform);
        }
        
        public static Entity<IViewPool> SetPosition(this Entity<IViewPool> entity, Vector3 position)
        {
            var currentTransform = entity.Need<Transform>();
            var nextTransform = entity.CreateComponent<Transform>();

            nextTransform.Rotation = currentTransform.Rotation;
            nextTransform.Scale = currentTransform.Scale;
            nextTransform.Position = position;
            
            return entity.SetInstance(nextTransform);
        }
        
        public static Entity<IViewPool> SetRotation(this Entity<IViewPool> entity, Quaternion rotation)
        {
            var currentTransform = entity.Need<Transform>();
            var nextTransform = entity.CreateComponent<Transform>();

            nextTransform.Rotation = rotation;
            nextTransform.Scale = currentTransform.Scale;
            nextTransform.Position = currentTransform.Position;
            
            return entity.SetInstance(nextTransform);
        }



        public static Entity<IViewPool> ForceUpdateTransform(this Entity<IViewPool> entity)
        {
            if(!entity.Has<Transform>())
                return entity;

            return entity.SetInstance(entity.Need<Transform>());
        }
    }

}