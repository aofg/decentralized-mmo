using Rentitas;
using Rentitas.Unity;

namespace Reviews
{
    public class ViewKernel : IKernel
    {
        public ViewKernel()
        {
            PoolInterfaces = new IPool[]
            {
                new ViewPool(), 
            };
        }

        public IPool[] PoolInterfaces { get; private set; }

        public BaseScenario SetupScenario(Pools pools)
        {
            var views = pools.Get<IViewPool>();
            return new Scenario("View Systems")
                    .Add(views.CreateSystem(new CreateObjectForIdentity()))
                    .Add(views.CreateSystem(new SetupViewContextSystem()))
                    .Add(views.CreateSystem(new ParentObjectSystem()))
                    .Add(views.CreateSystem(new TransformObjectSystem()))
                    .Add(views.CreateSystem(new UpdateObjectNameSystem()))
                    .Add(views.CreateSystem(new DisableViewSystem()))
                    .Add(views.CreateSystem(new DestroyViewSystem()))
                ;
        }
    }
}
