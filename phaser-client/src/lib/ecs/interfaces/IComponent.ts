export interface IComponent {
    create (...args : any[]) : void
}
export interface ComponentType<T extends IComponent> { new (...args : any[]) : T }
export interface IFlag {}
export interface ISingleton {}

export type ComponentOrIndex<T extends IComponent> = ComponentType<T> | number