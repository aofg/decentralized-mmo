using Rentitas;

namespace Revalue
{
    public static class RevalueExtensions
    {
        public static Entity<TPool> SetValue<TPool, TComponent, TValue>
            (this Entity<TPool> entity, TValue value)
            where TPool : class, IComponent
            where TComponent : class, TPool, IValueComponent<TValue>, new()
        {
            var valueComponent = entity.CreateComponent<TComponent>();
            valueComponent.Value = value;

            return entity.ReplaceInstance(valueComponent);
        }
        
        public static TValue GetValue<TPool, TComponent, TValue>
            (this Entity<TPool> entity)
            where TPool : class, IComponent
            where TComponent : class, TPool, IValueComponent<TValue>, new()
        {
            if (!entity.Has<TComponent>())
                return default(TValue);

            return entity.Get<TComponent>().Value;
        }
    }
}