import { ClassRef, Factory, IoC } from '../ioc/IoC'
import { Scenario } from '../ecs/Scenario'
import { ISystem } from '../ecs/interfaces/systems/ISystem'
import { IPool } from '../ecs/interfaces/IPool'
import { Pool } from '../ecs/Pool'
import { IComponent } from '../ecs/interfaces/IComponent'

type SystemRef = (ioc: IoC) => ISystem
type PoolRef<T extends IComponent> = ClassRef

interface KernelRef<T extends IComponent> {
  pools: PoolRef<T>[]
  scenario: SystemRef
  new(): any
}

export function systemFactory(scenarioRef: SystemRef): Factory<ISystem> {
  return (ioc) => {
    return scenarioRef(ioc)
  }
}

export function kernelFactory(KernelType: KernelRef<any>, context: IoC) {
  if (KernelType.pools && KernelType.pools.length) {
    KernelType.pools.forEach(pool => {
      context.bind(pool.name).class(pool)
    })
  }

  if (KernelType.scenario) {
    context.bind(KernelType.name + 'Scenario').factory(systemFactory(KernelType.scenario))
  }

  return (ioc) => new KernelType()
}
