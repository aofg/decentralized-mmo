import { IComponent } from "./IComponent";
import { IPool } from "./IPool";
import { ISignal } from "../utils/Signal";
import { IEntity } from "./IEntity";
import { IGroupObserver } from "./IGroupObserver";

export interface GroupEntityAdded<T extends IComponent> { (e: IEntity<T>): void }
export interface IGroupEntityAdded<T extends IComponent, TSignal> extends ISignal<TSignal> {
    dispatch(e: IEntity<T>, index: number, component: IComponent): void
}

export interface GroupEntityRemoved<T extends IComponent> { (e: IEntity<T>, index: number, component: IComponent): void; }
export interface IGroupEntityRemoved<T extends IComponent, TSignal> extends ISignal<TSignal> {
    dispatch(e: IEntity<T>, index: number, component: IComponent): void
}

export interface GroupEntityUpdated<T extends IComponent> { (e: IEntity<T>, index: number, component: IComponent, replacement: IComponent): void; }
export interface IGroupEntityUpdated<T extends IComponent, TSignal> extends ISignal<TSignal> {
    dispatch(e: IEntity<T>, index: number, component: IComponent, replacement: IComponent): void
}

export interface IGroup<T extends IComponent> {
    pool: IPool<T>
    onEntityAdded: ISignal<GroupEntityAdded<T>>
    onEntityRemoved: ISignal<GroupEntityRemoved<T>>
    onEntityUpdated: ISignal<GroupEntityUpdated<T>>

    getEntities(): IEntity<T>[]
    getSingleEntity(): IEntity<T>
    handleEntitySilently(entity: IEntity<T>): void
    handleEntity(entity: IEntity<T>, index: number, component: T): void
    updateEntity(entity: IEntity<T>, index: number, previousComponent: T, nextComponent: T): void
    addEntitySilently(entity: IEntity<T>): void
    removeEntitySilently(entity: IEntity<T>): void
    addEntity(entity: IEntity<T>, index: number, component: T): void
    removeEntity(entity: IEntity<T>, index: number, component: T): void
    containsEntity(entity)
    onUpdate(): IGroupObserver<T>
    onAdd(): IGroupObserver<T>
    onRemove(): IGroupObserver<T>
    onAddOrRemove(): IGroupObserver<T>
    onAnyChanges(): IGroupObserver<T>
}