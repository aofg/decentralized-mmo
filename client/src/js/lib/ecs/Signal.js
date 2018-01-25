// fast remove and add array
import { Bag } from '.'

export default class Signal {
  constructor (context, alloc=16) {
    this._listeners = new Bag()
    this._context = context
    this._alloc = alloc
    this.active = false
  }

  dispatch (...args) {
    if (!this.active) {
      return //? Should throw exception?
    }

    const listeners = this._listeners
    const size = listeners.size
    if (size <= 0) {
      return //* bail early
    }
    const context = this._context

    for (let i = 0; i < size; i++) {
      listeners[i](context, ...args)
    }
  }

  add (listener) {
    this._listeners.add(listener)
    this.active = true
  }

  remove (listener) {
    const listeners = this._listeners
    listeners.remove(listener)
    this.active = listeners.size > 0
  }

  clear() {
    this._listeners.clear()
    this.active = false
  }
}