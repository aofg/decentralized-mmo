import { ClassRef, Factory, IoC } from "../ioc/IoC"
import { Scenario } from "../ecs/Scenario"
import { ISystem } from "../ecs/interfaces/systems/ISystem";
import { IPool } from "../ecs/interfaces/IPool";
import { Pool } from "../ecs/Pool";
import { IComponent } from "../ecs/interfaces/IComponent";

type SystemRef = (ioc : IoC) => ISystem

interface KernelRef {
    (): void
    pools : (new () => IPool<any>)[]
    scenario : SystemRef
}

export function systemFactory(scenarioRef : SystemRef) : Factory<ISystem> {
    return (ioc) => {
        return scenarioRef(ioc)
    }
}

export function kernelFactory(KernelType : KernelRef, context : IoC) {
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

interface ITestComponents extends IComponent {

}

class ComponentA implements ITestComponents {
    public x : number

    create(x : number) {
        this.x = x
    }
}

class ComponentB implements ITestComponents {
    public x : number
    public y : number

    create(x : number, y : number) {
        this.x = x
        this.y = y
    }
}

class TestPoolA extends Pool<ITestComponents>{
    constructor() {
        super(ComponentA)
    }
}

class TestPoolB extends Pool<ITestComponents> {
    constructor() {
        super(ComponentA, ComponentB)
    }
}

class TestKernel {
    static get pools () : (new () => IPool<any>)[] {
        return [TestPoolA, TestPoolB]
    }
}