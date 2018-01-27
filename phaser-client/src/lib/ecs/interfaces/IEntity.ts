import { ComponentType } from './IComponent';
import { ISignal } from "../utils/Signal";
import { IComponent } from "./IComponent";
import { IPool } from "./IPool";


export interface EntityRelease<T extends IComponent> { (e : IEntity<T>) : void }
export interface IEntityRelease<T extends IComponent, TSignal> extends ISignal<TSignal> {
  dispatch(e : IEntity<T>):void
}

export interface EntityComponentChange<T extends IComponent> {(e : IEntity<T>, index : number, component : IComponent) : void;}
export interface IEntityComponentChange<T extends IComponent, TSignal> extends ISignal<TSignal> {
  dispatch(e : IEntity<T>, index : number, component : IComponent) : void
}

export interface EntityComponentUpdate<T extends IComponent> {(e : IEntity<T>, index : number, component : IComponent, replacement :  IComponent) : void;}
export interface IEntityComponentUpdate<T extends IComponent, TSignal> extends ISignal<TSignal> {
  dispatch(e : IEntity<T>, index : number, component : IComponent, replacement : IComponent) : void
}

export interface IEntity<T extends IComponent> {
    [shortcut : string] : T | any

    owners : any
    ownersCount : number

    id : string
    creationIndex : number

    onComponentAdd : ISignal<EntityComponentChange<T>>
    onComponentUpdate : ISignal<EntityComponentUpdate<T>>
    onComponentRemove : ISignal<EntityComponentChange<T>>
    onEntityRelease : ISignal<EntityRelease<T>>

    hasComponents(indexes: ComponentType<T>[]): boolean
    hasAnyComponent(indexes: ComponentType<T>[]) : boolean

    get(component : ComponentType<T>) : T
    add(component : ComponentType<T>, ...args : any[]) : IEntity<T>
    update(component : ComponentType<T>, ...args : any[]) : IEntity<T>
    remove(component : ComponentType<T>) : IEntity<T>
    toggle(component : ComponentType<T>, flag? : boolean) : IEntity<T>
    has(component : ComponentType<T>) : boolean

    retain(owner : Object) : void
    release(owner : Object) : void
}