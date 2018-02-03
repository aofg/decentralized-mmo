using System;
using System.Collections.Generic;
using Rentitas;
using UnityEngine;

namespace Reviews
{
    public class ViewRoot : MonoBehaviour, ISetPools
    {
        public Pools Context { get; private set; }
        public HashSet<ViewBehaviour> Children = new HashSet<ViewBehaviour>();

        public void RegisterBehaviour(ViewBehaviour behaviour)
        {
            if (!Children.Add(behaviour))
                throw new ArgumentException($"Already registered behaviour ({behaviour}) in view {this}");

            if (behaviour is ISetPools)
            {
                ((ISetPools)behaviour).SetPools(Context);
            }
        }

        public void SetPools(Pools pools)
        {
            Context = pools;
        }
    }
}