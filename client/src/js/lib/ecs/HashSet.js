export default class HashSet {
  constructor () {
    this.__hashSet__ = {}
    this.__keys = []
    this.__dirty = false
  }

  [Symbol.iterator] () {
    return Object.keys(this.__hashSet__)
  }

  add (value) {
    this.__hashSet__[value] = true
    this.__dirty = true
    return this
  }

  has (value) {
    return this.__hashSet__[value] === true
  }

  remove (value) {
    delete this.__hashSet__[value]
    this.__dirty = true

    return this
  }

  clear () {
    this.__hashSet__ = {}
    this.__dirty = true
    return this
  }

  concat (range) {
    if (range instanceof HashSet) {
      for (let value of range.values) {
        this.add(value)
      }
    } else {
      for (let value of range) {
        this.add(value)
      }
    }

    return this
  }

  __cleanup () {
    this.__keys = Object.keys(this.__hashSet__).sort()
    this.__dirty = false
  }

  get length () {
    if (this.__dirty) {
      this.__cleanup()
    }

    return this.__keys.length
  }

  get values () {
    if (this.__dirty) {
      this.__cleanup()
    }

    return this.__keys
  }
}
