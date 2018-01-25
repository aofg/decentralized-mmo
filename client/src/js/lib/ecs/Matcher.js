import { HashSet } from '.'
export default class Matcher {
  constructor (pool) {
    this._pool = pool
    this._allOf = new HashSet()
    this._anyOf = new HashSet()
    this._noneOf = new HashSet()
    this._pattern = ''
    this._indexes = new HashSet()
    this._merged = true
  }

  allOf(...components) {
    for (let type of components) {
      this._allOf.add(this._pool.getIndex(type))
    }
    this._merged = false

    return this
  }
  
  anyOf(...components) {
    for (let type of components) {
      this._anyOf.add(this._pool.getIndex(type))
    }
    this._merged = false
    
    return this
  }
  
  noneOf(...components) {
    for (let type of components) {
      this._noneOf.add(this._pool.getIndex(type))
    }
    this._merged = false
    
    return this
  }

  get indexes () {
    if (!this._merged) {
      this._merge()
    }

    return this._indexes.values
  }

  get pattern () {
    if (!this._merged) {
      this._merge()
    }

    return this._pattern
  }

  matches (entity) {
    const matchesAllOf = !this._allOf.length ? true : entity.hasComponents(this._allOf.values)
    const matchesAnyOf = !this._anyOf.length ? true : entity.hasAnyComponent(this._anyOf.values)
    const matchesNoneOf = !this._noneOf.length ? true : !entity.hasAnyComponent(this._noneOf.values)
    return matchesAllOf && matchesAnyOf && matchesNoneOf
  }

  _merge() {
    // TODO: simplify and optimize pattern construction
    let patternBuilder = []
    if (this._allOf.length) {
      patternBuilder.push('+')
      patternBuilder.push(this._allOf.values.join(','))
    }

    if (this._anyOf.length) {
      patternBuilder.push('|')
      patternBuilder.push(this._anyOf.values.join(','))
    }

    if (this._noneOf.length) {
      patternBuilder.push('-')
      patternBuilder.push(this._noneOf.values.join(','))
    }

    this._pattern = patternBuilder.join('')
    this._indexes.clear().concat(this._allOf).concat(this._anyOf).concat(this._noneOf)

    return this._pattern
  }

  getGroup() {
    return this._pool._getGroup(this)
  }

  onUpdate () {
    return this.getGroup().onUpdate()
  }
  onRemove () {
    return this.getGroup().onRemove()
  }
  onAdd () {
    return this.getGroup().onAdd()
  }
  onAddOrRemove() {
    return this.getGroup().onAddOrRemove()
  }
  onAnyChanges() {
    return this.getGroup().onAnyChanges()
  }
}