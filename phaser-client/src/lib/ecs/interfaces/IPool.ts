import { IComponent, ComponentType, ComponentOrIndex } from "./IComponent";
import { IMatcher } from "./IMatcher";
import { IEntity } from "./IEntity";

export interface IPool<T extends IComponent> {
    /**
     * Component Indexes
     */
    [ component : string ] : number | any
    totalCount : number
    
    allOf(...components: ComponentType<T>[]): IMatcher<T> 
    noneOf(...components: ComponentType<T>[]): IMatcher<T>
    anyOf(...components: ComponentType<T>[]): IMatcher<T>
    getEntities(matcher?: IMatcher<T>): IEntity<T>[]
    getType(typeOrIndex : ComponentOrIndex<T>) : ComponentType<T>
    getIndex(typeOrIndex : ComponentOrIndex<T>) : number
    createEntity (name? : string) : IEntity<T>  
    destroyEntity (entity : IEntity<T>) : IPool<T>  
    createComponent (typeOrIndex : ComponentOrIndex<T>, ...args : any[]) : T
    returnComponent (typeOrIndex : ComponentOrIndex<T>, instance : T) : void

    get<T>(typeOrIndex: ComponentOrIndex<T>) : IEntity<T>
    getSingleton<T>(typeOrIndex: ComponentOrIndex<T>) : T
}