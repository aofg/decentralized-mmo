export interface IComponent {
    create (...args : any[]) : void
}
export interface ComponentType<T extends IComponent> { (...args : any[]) : void }
export interface IFlag {}
export interface ISingleton {}

export type ComponentOrIndex<T extends IComponent> = ComponentType<T> | number