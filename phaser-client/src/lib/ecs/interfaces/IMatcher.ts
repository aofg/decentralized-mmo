import { IComponent, ComponentOrIndex } from "./IComponent";
import { IEntity } from "./IEntity";
import { IGroup } from "./IGroup";
import { IGroupObserver } from "./IGroupObserver";

export interface IMatcher<T extends IComponent> {
    readonly indexes : number[]
    readonly pattern : string 

    allOf(...components: ComponentOrIndex<T>[]) : IMatcher<T>
    anyOf(...components: ComponentOrIndex<T>[]) : IMatcher<T>
    noneOf(...components : ComponentOrIndex<T>[]) : IMatcher<T>
    matches(entity : IEntity<T>) : boolean
    
    getGroup() : IGroup<T>
    onUpdate() : IGroupObserver<T>
    onRemove() : IGroupObserver<T>
    onAdd() : IGroupObserver<T>
    onAddOrRemove() : IGroupObserver<T>
    onAnyChanges() : IGroupObserver<T>
}