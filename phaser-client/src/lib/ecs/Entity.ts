import { IComponent, IFlag, ComponentType, ComponentOrIndex } from './interfaces/IComponent'
import { IEntity, EntityComponentChange, EntityRelease, EntityComponentUpdate } from "./interfaces/IEntity";
import { IPool } from "./interfaces/IPool";
import { Bag } from "./utils/Bag";
import { Signal } from './utils/Signal';

export class Entity<T extends IComponent> implements IEntity<T> {
    private _pool: IPool<T>
    private _components: Bag<T>

    public id : string
    public creationIndex : number

    public owners: Bag<Object> = new Bag<Object>()
    public ownersCount: number = 0

    public onComponentAdd = new Signal<EntityComponentChange<T>>(this)
    public onComponentUpdate = new Signal<EntityComponentUpdate<T>>(this)
    public onComponentRemove = new Signal<EntityComponentChange<T>>(this)
    public onEntityRelease = new Signal<EntityRelease<T>>(this)


    constructor(pool: IPool<T>, components: Array<ComponentType<T>>) {
        this._pool = pool
        this._components = new Bag<T>(components.length)
        
        // TODO: Add shortcut
        // Object.defineProperty(this, snakeName, {
        //     get: () => this._get(type)
        // })

        // components.forEach(type => {
        //     console.log(type.name)

        //     const snakeName = type.name.charAt(0).toLowerCase() + type.name.slice(1)

        //     //* addPosition(10, 5)
        //     this[`add${type.name}`] = (...args) => {
        //         return this._add(type, ...args)
        //     }
        //     //* removePosition()
        //     this[`remove${type.name}`] = (...args) => {
        //         return this._remove(type, ...args)
        //     }
        //     //* updatePosition(15, 2)
        //     this[`update${type.name}`] = (...args) => {
        //         return this._update(type, ...args)
        //     }
        //     //* getPosition()
        //     this[`get${type.name}`] = () => {
        //         return this._get(type)
        //     }
        //     //* hasWeapon()
        //     this[`has${type.name}`] = () => {
        //         return this._has(type)
        //     }

        //     Object.defineProperty(this, snakeName, {
        //         get: () => this._get(type)
        //     })

        //     if (type instanceof IFlag) {
        //         Object.defineProperty(this, `is${type.name}`, {
        //             get: () => this._has(type),
        //             set: (flag) => this._toggle(type, flag)
        //         })
        //     }
        // })
    }

    retain(owner: Object): void {
        this.owners.add(owner)
        this.ownersCount++
    }

    release(owner: Object): void {
        this.owners.remove(owner)
        this.ownersCount--

        if (this.ownersCount === 0) {
            if (this.onEntityRelease.active) {
                this.onEntityRelease.dispatch()
            }
        } else if (this.ownersCount < 0) {
            // TODO: Typed Exception
            throw new Error("ENTITY_IS_ALREADY_RELEASED")
        }
    }

    //! Matcher methods
    hasComponents(indexes: ComponentType<T>[]): boolean {
        const _components = this._components
        for (let i = 0, indexesLength = indexes.length; i < indexesLength; i++) {
            if (!_components[this._getComponentIndex(indexes[i])]) {
                return false
            }
        }

        return true
    }

    hasAnyComponent(indexes: ComponentType<T>[]): boolean {
        const _components = this._components
        for (let i = 0, indexesLength = indexes.length; i < indexesLength; i++) {
            if (!!_components[this._getComponentIndex(indexes[i])]) {
                return true
            }
        }

        return false
    }

    get(component : ComponentType<T>) : T {
        return this._get(component)
    }

    add(component : ComponentType<T>, ...args : any[]) : IEntity<T> {
        return this._add(component, args)
    }

    update(component : ComponentType<T>, ...args : any[]) : IEntity<T> {
        return this._update(component, args)
    }

    remove(component : ComponentType<T>) : IEntity<T> {
        return this._remove(component)
    }

    toggle(component : ComponentType<T>, flag? : boolean) : IEntity<T> { 
        return this._toggle(component, flag)
    }
    has(component : ComponentType<T>) : boolean {
        return this._has(component)
    }

    private _createComponent(typeOrIndex: ComponentOrIndex<T>, ...args : any[]) : T {
        const index = this._getComponentIndex(typeOrIndex)
        return this._pool.createComponent(typeOrIndex, ...args)
    }

    private _getComponentIndex(typeOrIndex : ComponentOrIndex<T>) : number {
        return <number>typeOrIndex ? <number>typeOrIndex : this._pool.getIndex(typeOrIndex)
    }

    private _has(typeOrIndex) {
        const index = this._getComponentIndex(typeOrIndex)
        return !!this._components[index]
    }

    private _add(typeOrIndex : ComponentOrIndex<T>, ...args : any[]) {
        const index = this._getComponentIndex(typeOrIndex)

        if (this._has(index)) {
            // TODO: Typed Exception
            throw new Error("ENTITY_ALREADY_HAS_COMPONENT " + typeOrIndex)
        }

        const instance = this._createComponent(index, ...args)
        this._components[index] = instance

        // !Fire event
        this.onComponentAdd.dispatch(index, instance)
        return this
    }

    private _get(typeOrIndex : ComponentOrIndex<T>) : T {
        const index = this._getComponentIndex(typeOrIndex)
        if (!this._has(index)) {
            // TODO: Typed Exception
            throw new Error("ENTITY_HAS_NOT_COMPONENT " + typeOrIndex)
        }
        return this._components[index]
    }

    private _remove(typeOrIndex : ComponentOrIndex<T>) : IEntity<T> {
        const index = this._getComponentIndex(typeOrIndex)
        if (!this._has(index)) {
            // TODO: Typed Exception
            throw new Error("ENTITY_HAS_NO_COMPONENT " + typeOrIndex)
        }

        const instance = this._components[index]
        this._pool.returnComponent(index, instance)
        delete this._components[index]

        // !Fire event
        this.onComponentRemove.dispatch(index, instance)
        return this
    }

    private _update(typeOrIndex : ComponentOrIndex<T>, ...args : any[]) : IEntity<T> {
        const index = this._getComponentIndex(typeOrIndex)
        const newInstance = this._pool.createComponent(index, ...args)
        const previousInstance = this._components[index]
        this._pool.returnComponent(typeOrIndex, previousInstance)
        this._components[index] = newInstance

        // console.log('update', this, index, newInstance.toString(), previousInstance.toString())
        // !Fire event
        this.onComponentUpdate.dispatch(index, newInstance, previousInstance)
        return this
    }

    private _toggle(typeOrIndex : ComponentOrIndex<T>, flag? : boolean) : IEntity<T> {
        const index = this._getComponentIndex(typeOrIndex)
        const has = this._has(typeOrIndex)
        if (flag && !has) {
            this._add(typeOrIndex)
        }

        if (!flag && has) {
            this._remove(typeOrIndex)
        }

        return this
    }
}