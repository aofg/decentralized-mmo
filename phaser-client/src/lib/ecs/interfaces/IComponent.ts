export interface IComponent {
    create (...args : any[]) : void
}

export class Singleton {
  static get singleton(): boolean { return true }
}

export class Flag {
  static get flag(): boolean { return true }
}

export class SingleFlag {
  static get singleton(): boolean { return true }
  static get flag(): boolean { return true }
}

export interface IFlag {
  readonly flag : boolean
}
export interface ISingleton {
  readonly singleton : boolean
}

export type ComponentClass<T extends IComponent> = (new (...args : any[]) => T)
export type ComponentType<T extends IComponent> = ComponentClass<T> | IFlag | ISingleton
export type ComponentOrIndex<T extends IComponent> = ComponentType<T> | number