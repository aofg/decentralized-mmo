import { Group, Matcher, Entity, Bag, UUID, ERRORS } from '.'

export default class Pool {
  constructor (...components) {
    this._components = components
    this._componentsCache = components.map(_ => [])
    this._entities = {}
    this._nextIndex = 0
    this._entitiesCache = null
    this._reusableEntities = []
    this._groupPatterns = {}
    this._retainedEntities = {}
    this._groupsForIndex = new Bag()
    this.totalCount = components.length

    this._cachedEntityComponentAddHandler = this.entityComponentAddHandler.bind(this)
    this._cachedEntityComponentRemoveHandler = this.entityComponentRemoveHandler.bind(this)
    this._cachedEntityComponentUpdateHandler = this.entityComponentUpdateHandler.bind(this)

    components.forEach((componentType, index) => {
      this[componentType.name] = index
    })
  }

  entityComponentAddHandler (entity, index, component) {
    const groups = this._groupsForIndex[index]
    if (groups != null) {
      for (let i = 0, groupsCount = groups.size; i < groupsCount; i++) {
        groups[i].handleEntity(entity, index, component)
      }
    }
  }
  entityComponentRemoveHandler (entity, index, component) {
    const groups = this._groupsForIndex[index]
    if (groups != null) {
      for (let i = 0, groupsCount = groups.size; i < groupsCount; i++) {
        groups[i].handleEntity(entity, index, component)
      }
    }
  }
  entityComponentUpdateHandler (entity, index, previousComponent, nextComponent) {
    const groups = this._groupsForIndex[index]
    if (groups != null) {
      for (let i = 0, groupsCount = groups.size; i < groupsCount; i++) {
        groups[i].updateEntity(entity, index, previousComponent, nextComponent)
      }
    }
  }

  allOf (...components) {
    return new Matcher(this).allOf(...components)
  }

  noneOf (...components) {
    return new Matcher(this).noneOf(...components)
  }

  anyOf (...components) {
    return new Matcher(this).anyOf(...components)
  }

  getEntities (matcher) {
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

  _getGroup (matcher) {
    if (typeof this._groupPatterns[matcher.pattern] === 'undefined') {
      const group = new Group(this, matcher)

      const entities = this.getEntities()
      for (let i = 0, entitiesLength = entities.length; i < entitiesLength; i++) {
        group.handleEntitySilently(entities[i])
      }

      for (let i = 0, indexesLength = matcher.indexes.length; i < indexesLength; i++) {
        const index = matcher.indexes[i]
        if (typeof this._groupsForIndex[index] === 'undefined') {
          this._groupsForIndex[index] = new Bag()
        }
        this._groupsForIndex[index].add(group)
      }

      this._groupPatterns[matcher.pattern] = group
    }

    return this._groupPatterns[matcher.pattern]
  }

  getType (typeOrIndex) {
    const type = typeof typeOrIndex === 'number' ? this._components[typeOrIndex] : typeOrIndex

    if (typeof type !== 'function') {
      throw new Error(ERRORS.POOL_INCORRECT_COMPONENT_TYPE, this, typeOrIndex)
    }

    return type
  }
  getIndex (typeOrIndex) {
    const index = typeof typeOrIndex === 'number' ? typeOrIndex : this[typeOrIndex.name]

    if (index === 'undefined') {
      throw new Error(ERRORS.POOL_INDEX_NOT_FOUND, this, typeOrIndex)
    }

    return index
  }

  createEntity (name) {
    const entity = this._reusableEntities.length > 0 ? this._reusableEntities.pop() : new Entity(this, this._components)
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

  destroyEntity (entity) {
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
      entity.release()

      // enitity.enable = false
      // entity.getComponents().forEach(component => {
      //   this.returnComponent(typeof component, component)
      // })

      // entity.reset()
      // this._entitiesCache.push(entity)
    }

    return this
  }

  createComponent (typeOrIndex, ...args) {
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

  returnComponent (type, instance) {
    const componentIndex = this.getIndex(type)
    this._componentsCache[componentIndex].push(instance)
  }

  createSystem (Type) {
    const system = new Type()
    if (typeof system.inject === 'function') {
      system.inject({
        pool: this
      })
    }

    return system
  }
}
