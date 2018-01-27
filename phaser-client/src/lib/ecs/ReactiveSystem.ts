import { IComponent } from "./interfaces/IComponent"
import { IEntity } from "./interfaces/IEntity"
import { IGroupObserver } from "./interfaces/IGroupObserver";
import { IExecuteSystem } from "./interfaces/systems/IExecuteSystem";
import { IInitializeSystem } from "./interfaces/systems/IInitializeSystem";
import { ICleanupSystem } from "./interfaces/systems/ICleanupSystem";

interface IReactiveSystem<T extends IComponent> {
    readonly trigger : IGroupObserver<T>
    executeOnTrigger(entities : IEntity<T>[]) : void
}

export abstract class ReactiveSystem<T extends IComponent> implements IReactiveSystem<T>, IExecuteSystem, IInitializeSystem, ICleanupSystem {
    abstract get trigger () : IGroupObserver<T>
    abstract executeOnTrigger(entities: IEntity<T>[]): void

    initialize() : void {
        this.trigger.activate()
    }

    execute() : void {
        const collected = this.trigger.collected
        if (collected.length > 0) {
          this.executeOnTrigger(collected)
        }
    }    

    activate() {
      this.trigger.activate()
    }
  
    deactivate() {
      this.trigger.deactivate()
    }
    cleanup() {
      this.trigger.clear()
    }
}