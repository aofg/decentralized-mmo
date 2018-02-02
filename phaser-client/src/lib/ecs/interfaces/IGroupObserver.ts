import { IComponent } from "./IComponent";
import { IGroup } from "./IGroup";
import { IEntity } from "./IEntity";

export enum GroupObserverTrigger {
    ENTITY_ADD,
    ENTITY_UPDATE,
    ENTITY_REMOVE,
    ENTITY_ADD_OR_REMOVE,
    ENTITY_ANY
}

export interface IGroupObserver<T extends IComponent> {
    readonly collected : IEntity<T>[]
    activate() : void
    deactivate() : void 
    clear() : void 
    addEntity(group : IGroup<T>, entity : IEntity<T>, index : number, component : T) : void
}