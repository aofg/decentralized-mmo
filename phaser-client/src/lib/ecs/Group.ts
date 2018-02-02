import { IGroup, GroupEntityRemoved, GroupEntityAdded, GroupEntityUpdated } from './interfaces/IGroup'
import { IPool } from './interfaces/IPool'
import { IComponent } from './interfaces/IComponent'
import { ISignal, Signal } from './utils/Signal'
import { IEntity } from './interfaces/IEntity'
import { IMatcher } from './interfaces/IMatcher'
import { IGroupObserver, GroupObserverTrigger } from './interfaces/IGroupObserver'
import { GroupObserver } from './GroupObserver'

export class Group<T extends IComponent> implements IGroup<T> {
  public pool: IPool<T>
  public onEntityAdded: ISignal<GroupEntityAdded<T>> = new Signal(this)
  public onEntityRemoved: ISignal<GroupEntityRemoved<T>> = new Signal(this)
  public onEntityUpdated: ISignal<GroupEntityUpdated<T>> = new Signal(this)

  private _entities: { [id: string]: IEntity<T> } = {}
  private _matcher: IMatcher<T>
  private _entitiesCache: IEntity<T>[] | null = null
  private _singleEntityCache: IEntity<T> | null = null

  constructor(pool: IPool<T>, matcher: IMatcher<T>) {
    this.pool = pool
    this._matcher = matcher
  }

  getEntities(): IEntity<T>[] {
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

  getSingleEntity(): IEntity<T> {
    if (this._singleEntityCache == null) {
      const enumerator = Object.keys(this._entities)
      const c = enumerator.length
      if (c === 1) {
        this._singleEntityCache = this._entities[enumerator[0]]
      } else if (c === 0) {
        return null
      } else {
        // TODO: Typed exception
        throw new Error("GROUP_NOT_SINGLE_ENTITY_GROUP")
      }
    }

    return this._singleEntityCache
  }

  handleEntitySilently(entity: IEntity<T>): void {
    if (this._matcher.matches(entity)) {
      this.addEntitySilently(entity)
    } else {
      this.removeEntitySilently(entity)
    }
  }

  handleEntity(entity: IEntity<T>, index: number, component: T): void {
    if (this._matcher.matches(entity)) {
      this.addEntity(entity, index, component)
    } else {
      this.removeEntity(entity, index, component)
    }
  }

  updateEntity(entity: IEntity<T>, index: number, previousComponent: T, nextComponent: T): void {
    if (entity.id in this._entities) {
      // this.onEntityRemoved.dispatch(entity, index, previousComponent)
      // this.onEntityAdded.dispatch(entity, index, nextComponent)
      this.onEntityUpdated.dispatch(entity, index, previousComponent, nextComponent)
    }
  }


  addEntitySilently(entity: IEntity<T>): void {
    if (!this.containsEntity(entity)) {
      this._entities[entity.id] = entity
      this._entitiesCache = null
      this._singleEntityCache = null
      entity.retain(this)
    }
  }

  removeEntitySilently(entity: IEntity<T>): void {
    if (entity.id in this._entities) {
      delete this._entities[entity.id]
      this._entitiesCache = null
      this._singleEntityCache = null
      entity.release(this)
    }
  }

  addEntity(entity: IEntity<T>, index: number, component: T): void {
    if (!this.containsEntity(entity)) {
      this._entities[entity.id] = entity
      this._entitiesCache = null
      this._singleEntityCache = null
      entity.retain(this)
      this.onEntityAdded.dispatch(entity, index, component)
    }
  }

  removeEntity(entity: IEntity<T>, index: number, component: T): void {
    if (this.containsEntity(entity)) {
      delete this._entities[entity.id]
      this._entitiesCache = null
      this._singleEntityCache = null
      this.onEntityRemoved.dispatch(entity, index, component)
      entity.release(this)
    }
  }

  containsEntity(entity) {
    return entity.id in this._entities
  }

  onUpdate(): IGroupObserver<T> {
    return new GroupObserver(this, GroupObserverTrigger.ENTITY_UPDATE)
  }
  onAdd(): IGroupObserver<T> {
    return new GroupObserver(this, GroupObserverTrigger.ENTITY_ADD)
  }
  onRemove(): IGroupObserver<T> {
    return new GroupObserver(this, GroupObserverTrigger.ENTITY_REMOVE)
  }
  onAddOrRemove(): IGroupObserver<T> {
    return new GroupObserver(this, GroupObserverTrigger.ENTITY_ADD_OR_REMOVE)
  }
  onAnyChanges(): IGroupObserver<T> {
    return new GroupObserver(this, GroupObserverTrigger.ENTITY_ANY)
  }
}
