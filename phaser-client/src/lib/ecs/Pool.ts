import { Bag } from "./utils/Bag"
import { IPool } from "./interfaces/IPool"
import { ComponentType, ComponentOrIndex, IComponent } from "./interfaces/IComponent";
import { IEntity, EntityComponentChange, EntityComponentUpdate } from "./interfaces/IEntity";
import { IGroup } from "./interfaces/IGroup";
import { IMatcher } from "./interfaces/IMatcher";
import { Entity } from "./Entity";
import { Matcher } from "./Matcher";
import { UUID } from "./utils/UUID";

export class Pool<T extends IComponent> implements IPool<T> {
    [component: string]: number | any

    private _components: ComponentType<T>[] = []
    private _componentsCache: Bag<Bag<T>>
    private _reusableEntities: Bag<IEntity<T>>

    private _nextIndex: number = 0
    private _entities: { [id: string]: IEntity<T> } = {}
    private _entitiesCache: IEntity<T>[] | null = null
    private _groupPatterns: { [pattern: string]: IGroup<T> | null } = {}
    private _retainedEntities: { [id: string]: IEntity<T> } = {}
    private _groupsForIndex: Bag<Bag<IGroup<T>>> = new Bag()

    public totalCount: number

    private _cachedEntityComponentAddHandler : EntityComponentChange<T>
    private _cachedEntityComponentRemoveHandler : EntityComponentChange<T>
    private _cachedEntityComponentUpdateHandler : EntityComponentUpdate<T>

    constructor(...components: ComponentType<T>[]) {
        this._components = components
        this._componentsCache = new Bag(components.length)
        this._groupsForIndex = new Bag()
        this.totalCount = components.length

        this._cachedEntityComponentAddHandler = this.entityComponentAddHandler.bind(this)
        this._cachedEntityComponentRemoveHandler = this.entityComponentRemoveHandler.bind(this)
        this._cachedEntityComponentUpdateHandler = this.entityComponentUpdateHandler.bind(this)

        components.forEach((componentType, index) => {
            this[componentType.name] = index
        })
    }

    allOf(...components: ComponentType<T>[]): IMatcher<T> {
        return new Matcher<T>(this).allOf(...components)
    }

    noneOf(...components: ComponentType<T>[]): IMatcher<T> {
        return new Matcher<T>(this).noneOf(...components)
    }

    anyOf(...components: ComponentType<T>[]): IMatcher<T> {
        return new Matcher<T>(this).anyOf(...components)
    }

    getEntities(matcher?: IMatcher<T>): IEntity<T>[] {
        if (matcher) {
            return this._getGroup(matcher).getEntities()
        } else {
            if (this._entitiesCache == null) {
                const entities = this._entities
                const keys = Object.keys(entities)
                const length = keys.length
                const entitiesCache = this._entitiesCache = new Array(length)

                for (let i = 0; i < length; i++) {
                    entitiesCache[i] = entities[keys[i]]
                }
            }
            return this._entitiesCache
        }
    }


    getType(typeOrIndex : ComponentOrIndex<T>) : ComponentType<T> {
        const type = typeof typeOrIndex === 'number' ? this._components[typeOrIndex] : typeOrIndex

        if (typeof type !== 'function') {
            // TODO: Typed Exception
            throw new Error("POOL_INCORRECT_COMPONENT_TYPE" + this + typeOrIndex)
        }

        return type
    }
    getIndex(typeOrIndex : ComponentOrIndex<T>) : number {
        const index = typeof typeOrIndex === 'number' ? typeOrIndex : this[typeOrIndex.name]

        if (index === 'undefined') {
            // TODO: Typed Exception
            throw new Error("POOL_INDEX_NOT_FOUND" + this + typeOrIndex)
        }

        return index
    }

    createEntity (name? : string) : IEntity<T> {
      const entity = this._reusableEntities.length > 0 ? this._reusableEntities.pop() : new Entity<T>(this, this._components)
      entity._isEnabled = true
      entity.name = name
      entity._creationIndex = this._creationIndex++
      entity.id = UUID.randomUUID()
      entity.retain(this)
      this._entities[entity.id] = entity
      this._entitiesCache = null
      entity.onComponentAdd.add(this._cachedEntityComponentAddHandler)
      entity.onComponentRemove.add(this._cachedEntityComponentRemoveHandler)
      entity.onComponentUpdate.add(this._cachedEntityComponentUpdateHandler)
      entity.onEntityReleased.add(this._cachedOnEntityReleased)
      return entity
    }
  
    destroyEntity (entity : IEntity<T>) : IPool<T> {
      if (this._entities[entity.id]) {
        delete this._entities[entity.id]
        this._entitiesCache = null
        const onEntityWillBeDestroyed = this.onEntityWillBeDestroyed
        if (onEntityWillBeDestroyed.active) {
          onEntityWillBeDestroyed.dispatch(this, entity)
        }
        entity.destroy()
        this.onEntityDestroyed.dispatch(this, entity)
  
        if (entity._refCount === 1) {
          entity.onEntityReleased.remove(this._cachedOnEntityReleased)
          this._reusableEntities.add(entity)
        } else {
          this._retainedEntities[entity.id] = entity
        }
        entity.release(this)
  
        // enitity.enable = false
        // entity.getComponents().forEach(component => {
        //   this.returnComponent(typeof component, component)
        // })
  
        // entity.reset()
        // this._entitiesCache.push(entity)
      }
  
      return this
    }
  
    createComponent (typeOrIndex : ComponentOrIndex<T>, ...args : any[]) : T {
      const componentIndex = this.getIndex(typeOrIndex)
      const cache = this._componentsCache[componentIndex]
  
      if (cache.length > 0) {
        const instance = cache.pop()
        instance.create(...args)
        return instance
      } else {
        const ComponentType = this.getType(typeOrIndex)
        const instance = new ComponentType()
        instance.create(...args)
        return instance
      }
    }
  
    returnComponent (typeOrIndex : ComponentOrIndex<T>, instance : T) : void {
      const componentIndex = this.getIndex(typeOrIndex)
      this._componentsCache[componentIndex].push(instance)
    }

    private entityComponentAddHandler(entity: IEntity<T>, index: number, component: T) {
        const groups = this._groupsForIndex[index]
        if (groups != null) {
            for (let i = 0, groupsCount = groups.size; i < groupsCount; i++) {
                groups[i].handleEntity(entity, index, component)
            }
        }
    }
    private entityComponentRemoveHandler(entity: IEntity<T>, index: number, component: T) {
        const groups = this._groupsForIndex[index]
        if (groups != null) {
            for (let i = 0, groupsCount = groups.size; i < groupsCount; i++) {
                groups[i].handleEntity(entity, index, component)
            }
        }
    }
    private entityComponentUpdateHandler(entity: IEntity<T>, index: number, previousComponent: T, nextComponent: T) {
        const groups = this._groupsForIndex[index]
        if (groups != null) {
            for (let i = 0, groupsCount = groups.size; i < groupsCount; i++) {
                groups[i].updateEntity(entity, index, previousComponent, nextComponent)
            }
        }
    }
}