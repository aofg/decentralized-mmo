using System.Reflection;
using Rentitas;
using Zenject;

namespace Renject
{
    public class RentitasKernelFactory<TKernel> : IFactory<TKernel>
    where TKernel : class, IKernel, new()
    {
        private DiContainer container;

        public RentitasKernelFactory(DiContainer container)
        {
            this.container = container;
        }

        public TKernel Create()
        {
            var kernel = new TKernel();//container.Resolve<TKernel>();
            container.Inject(kernel);

            var poolGeneric = typeof(Pool<>);
            
            for (var index = 0; index < kernel.PoolInterfaces.Length; index++)
            {
                UnityEngine.Debug.Log(poolGeneric.MakeGenericType(kernel.PoolInterfaces[index].PoolType));
                container
                    .Bind(poolGeneric.MakeGenericType(kernel.PoolInterfaces[index].PoolType))
                    .FromInstance(kernel.PoolInterfaces[index])
                    .AsSingle();
            }
            return kernel;
        }
    }
}