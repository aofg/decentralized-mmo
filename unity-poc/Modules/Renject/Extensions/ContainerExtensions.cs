using Rentitas;
using Zenject;

namespace Renject
{
    public static class ContainerExtensions
    {
        public static ISystem ResolveSystem<TPool, TSystem>(this DiContainer container)
        where TPool : class, IComponent
        where TSystem : ISystem, new()
        {
            var system = new TSystem();
            container.Inject(system);
            var pool = container.Resolve<Pool<TPool>>();
            return pool.CreateSystem(system);
        }
        
        public static ISystem ResolveSystem<TSystem>(this DiContainer container)
            where TSystem : ISystem, new()
        {
            var system = new TSystem();
            container.Inject(system);
            return system;
        }
    }
}