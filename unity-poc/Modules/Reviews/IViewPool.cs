using System;
using Rentitas;
using Revalue;
using UnityEngine;

namespace Reviews
{
    public interface IViewPool : IComponent { }

    public class ViewPool : Pool<IViewPool>
    {
        public PrimaryEntityIndex<IViewPool, Guid> IdIndex { get; private set; }

        public ViewPool() : base(RentitasUtility.CollectComponents<IViewPool>().Build())
        {
            IdIndex = new PrimaryEntityIndex<IViewPool, Guid>(
                GetGroup(Matcher.AllOf(typeof(Unique))),
                (e, c) => ((Unique)c).Id
            );
        }
    }

    public class Unique : IViewPool
    {
        public Guid Id;
    }
    
    public class Enabled : IViewPool, IFlag { }
    public class Destroying : IViewPool, IFlag { }
    public class Destroyed : IViewPool, IFlag { }
    public class Disabled : IViewPool, IFlag { }

    public class InScene : IViewPool
    {
        public GameObject Object;
    }

    public class Root : RentitasValue<ViewRoot>, IViewPool
    {
    }

    public class Parent : IViewPool
    {
        public Guid ParentId;
    }

    public class Transform : IViewPool
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale = Vector3.one;
    }

    public class Name : IViewPool
    {
        public string Value;
    }

    public class Prototype : IViewPool
    {
        public GameObject Prefab;
    }
}