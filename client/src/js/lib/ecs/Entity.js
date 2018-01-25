import { ERRORS, Signal, Flag, Singleton, SingleFlag } from '.'

export default class Entity {
  constructor(pool, components) {
    this._pool = pool
    this._getters = []
    this.owners = {}
    this.ownersCount = 0;
    this._components = Array(components.length)

    //! Shortcuts
    components.forEach(type => {
      console.log(type.name)

      const snakeName = type.name.charAt(0).toLowerCase() + type.name.slice(1)

      //* addPosition(10, 5)
      this[`add${type.name}`] = (...args) => {
        return this._add(type, ...args)
      }
      //* removePosition()
      this[`remove${type.name}`] = (...args) => {
        return this._remove(type, ...args)
      }
      //* updatePosition(15, 2)
      this[`update${type.name}`] = (...args) => {
        return this._update(type, ...args)
      }
      //* getPosition()
      this[`get${type.name}`] = () => {
        return this._get(type)
      }
      //* hasWeapon()
      this[`has${type.name}`] = () => {
        return this._has(type)
      }

      Object.defineProperty(this, snakeName, {
        get: () => this._get(type)
      })

      if (type.prototype instanceof Flag || type.prototype instanceof SingleFlag) {
        Object.defineProperty(this, `is${type.name}`, {
          get: () => this._has(type),
          set: (flag) => this._toggle(type, flag)
        })
      }
    })

    // !Events
    this.onComponentAdd = new Signal(this)
    this.onComponentUpdate = new Signal(this)
    this.onComponentRemove = new Signal(this)
    this.onEntityReleased = new Signal(this)
  }

  retain (owner) {
    this.owners[owner] = true
    this.ownersCount++
  }

  release (owner) {
    delete this.owners[owner]
    this.ownersCount--

    if (this.ownersCount === 0) {
      if (this.onEntityReleased.onEntityReleased.active) {
        this.onEntityReleased.dispatch()
      }
    } else if (this._refCount < 0) {
      throw new Error(ERRORS.ENTITY_IS_ALREADY_RELEASED, this)
    }

  }

  hasComponents(indexes) {
    const _components = this._components
    for (let i = 0, indexesLength = indexes.length; i < indexesLength; i++) {
      if (!_components[indexes[i]]) {
        return false
      }
    }

    return true
  }
  
  hasAnyComponent(indexes) {
    const _components = this._components
    for (let i = 0, indexesLength = indexes.length; i < indexesLength; i++) {
      if (!!_components[indexes[i]]) {
        return true
      }
    }

    return false
  }

  _createComponent(typeOrIndex, ...args) {
    const index = this._getComponentIndex(typeOrIndex)
    return this._pool.createComponent(typeOrIndex, ...args)
  }
  
  _getComponentIndex (typeOrIndex) {
    return typeof typeOrIndex === 'number' ? typeOrIndex : this._pool.getIndex(typeOrIndex)
  }

  _has (typeOrIndex) {
    const index = this._getComponentIndex(typeOrIndex)
    return !!this._components[index]
  }

  _add (typeOrIndex, ...args) {
    const index = this._getComponentIndex(typeOrIndex)

    if (this._has(index)) {
      throw new Error(ERRORS.ENTITY_ALREADY_HAS_COMPONENT, typeOrIndex)
    }
    
    const instance = this._createComponent(index, ...args)
    this._components[index] = instance

    // !Fire event
    this.onComponentAdd.dispatch(index, instance)
    return this
  }

  _get (typeOrIndex) {
    const index = this._getComponentIndex(typeOrIndex)
    return this._components[index]
  }

  _remove (typeOrIndex) {
    const index = this._getComponentIndex(typeOrIndex)
    if (!this._has(index)) {
      throw new Error(ERRORS.ENTITY_HAS_NO_COMPONENT, typeOrIndex)
    }

    const instance = this._components[index]
    this._pool.returnComponent(type, instance)
    delete this._components[index]

    // !Fire event
    this.onComponentRemove.dispatch(index, instance)
    return this
  }

  _update (typeOrIndex, ...args) {
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

  _toggle (typeOrIndex, flag) {
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