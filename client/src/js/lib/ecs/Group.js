import { Signal, TRIGGERS } from '.'

export default class Group {
  constructor (pool, matcher) {
    this.pool = pool
    this._matcher = matcher
    this._entitiesCache = null
    this._singleEntityCache = null
    this.onEntityAdded = new Signal(this)
    this.onEntityRemoved = new Signal(this)
    this.onEntityUpdated = new Signal(this)
    this._entities = {}
  }

  getEntities () {
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
  getSingleEntity() {
    if (this._singleEntityCache == null) {
      const enumerator = Object.keys(this._entities)
      const c = enumerator.length
      if (c === 1) {
        this._singleEntityCache = this._entities[enumerator[0]]
      } else if (c === 0) {
        return null
      } else {
        throw new Error(ERRORS.GROUP_NOT_SINGLE_ENTITY_GROUP)
      }
    }

    return this._singleEntityCache
  }

  handleEntitySilently(entity) {
    if (this._matcher.matches(entity)) {
      this.addEntitySilently(entity)
    } else {
      this.removeEntitySilently(entity)
    }
  }

  handleEntity(entity, index, component) {
    if (this._matcher.matches(entity)) {
      this.addEntity(entity, index, component)
    } else {
      this.removeEntity(entity, index, component)
    }
  }

  updateEntity(entity, index, previousComponent, nextComponent) {
    if (entity.id in this._entities) {
      // this.onEntityRemoved.dispatch(entity, index, previousComponent)
      // this.onEntityAdded.dispatch(entity, index, nextComponent)
      this.onEntityUpdated.dispatch(entity, index, previousComponent, nextComponent)
    }
  }


  addEntitySilently(entity) {
    if (!this.containsEntity(entity)) {
      this._entities[entity.id] = entity
      this._entitiesCache = null
      this._singleEntityCache = null
      entity.retain(this)
    }
  }

  removeEntitySilently(entity) {
    if (entity.id in this._entities) {
      delete this._entities[entity.id]
      this._entitiesCache = null
      this._singleEntityCache = null
      entity.release(this)
    }
  }

  addEntity(entity, index, component) {
    if (!this.containsEntity(entity)) {
      this._entities[entity.id] = entity
      this._entitiesCache = null
      this._singleEntityCache = null
      entity.retain(this)
      const onEntityAdded = this.onEntityAdded
      if (onEntityAdded.active) {
        onEntityAdded.dispatch(entity, index, component)
      }
    }
  }

  removeEntity(entity, index, component) {
    if (this.containsEntity(entity)) {
      delete this._entities[entity.id]
      this._entitiesCache = null
      this._singleEntityCache = null
      let onEntityRemoved = this.onEntityRemoved
      if (onEntityRemoved.active) onEntityRemoved.dispatch(entity, index, component)
      entity.release(this)
    }
  }

  containsEntity(entity) {
    return entity.id in this._entities
  }

  onUpdate() {
    return new GroupObserver(this, TRIGGERS.ENTITY_UPDATE)
  }
  onAdd() {
    return new GroupObserver(this, TRIGGERS.ENTITY_ADD)
  }
  onRemove() {
    return new GroupObserver(this, TRIGGERS.ENTITY_REMOVE)
  }
  onAddOrRemove() {
    return new GroupObserver(this, TRIGGERS.ENTITY_ADD_OR_REMOVE)
  }
  onAnyChanges() {
    return new GroupObserver(this, TRIGGERS.ENTITY_ANY)
  }

}

export class GroupObserver {
  constructor(group, trigger) {
    this._group = group
    this._trigger = trigger
    this._collected = {}
    this._addEntityCache = this.addEntity

    this.activate()
  }

  get collected () {
    return Object.values(this._collected)
  }

  activate() {
    switch (this._trigger) {
      case TRIGGERS.ENTITY_ADD: 
        this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
        this._group.onEntityAdded.add(this._addEntityCache.bind(this))
      break;
      case TRIGGERS.ENTITY_UPDATE: 
        this._group.onEntityUpdated.remove(this._addEntityCache.bind(this))
        this._group.onEntityUpdated.add(this._addEntityCache.bind(this))
      break;
      case TRIGGERS.ENTITY_REMOVE: 
        this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
        this._group.onEntityRemoved.add(this._addEntityCache.bind(this))
      break;
      case TRIGGERS.ENTITY_ADD_OR_REMOVE: 
        this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
        this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
        this._group.onEntityAdded.add(this._addEntityCache.bind(this))
        this._group.onEntityRemoved.add(this._addEntityCache.bind(this))
      break;
      case TRIGGERS.ENTITY_ANY:
        this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
        this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
        this._group.onEntityUpdated.remove(this._addEntityCache.bind(this))
        this._group.onEntityAdded.add(this._addEntityCache.bind(this))
        this._group.onEntityRemoved.add(this._addEntityCache.bind(this))
        this._group.onEntityUpdated.add(this._addEntityCache.bind(this))
      default:
        throw new Error(ERRORS.GROUP_OBSERVER_INCORRECT_TRIGGER_TYPE, this, this._trigger)
    }
  }

  deactivate () {
    this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
    this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
    this._group.onEntityUpdated.remove(this._addEntityCache.bind(this))

    this.clear()
  }

  clear () {
    for (let e in this._collected) {
      this._collected[e].release(this)
    }
    this._collected = []
  }

  addEntity (group, entity, index, component) {
    if (!(entity.id in this._collected)) {
      this._collected[entity.id] = entity
      entity.retain(this)
    }
  }
}