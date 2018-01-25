export default class IoC {
  constructor () {
    this._pipes = {}
  }

  bind (identity) {
    const pipe = new BindingPipe(identity)
    this._pipes[identity] = pipe
    return pipe
  }

  create (Class) {
    const instance = new Class()
    this.resolveDependencies(instance)
    return instance
  }

  resolveDependencies (instance) {
    if (Array.isArray(instance.$dependencies)) {
      instance.$dependencies.forEach(request => {
        const field = this.getField(request)
        const dependency = this.resolve(request)
        instance[field] = dependency
      })
    }

    if (typeof instance.$injected === 'function') {
      instance.$injected(this)
    }

    return instance
  }

  resolve (request) {
    const pipe = this._pipes[this.getIdentity(request)]
    if (pipe) {
      return pipe.resolve(this)
    }

    console.error('Couldn\'t resolve dependency:', request)
    return null
  }

  getIdentity (request) {
    if (typeof request === 'string') {
      return request
    } else if (typeof request.identity !== 'undefined') {
      return request.identity
    }

    throw new Error('Incorrect request dependency', request)
  }

  getField (request) {
    if (typeof request === 'string') {
      return request
    } else if (typeof request.to !== 'undefined') {
      return request.to
    }

    throw new Error('Incorrect request dependency', request)
  }
}

export class BindingPipe {
  constructor (identity) {
    this._id = identity
    this._factory = this._singleton = this._Type = null
  }

  singleton (instance) {
    if (this._factory) {
      throw new Error('Factory already injected')
    }
    if (this._Type) {
      throw new Error('Constructor already injected')
    }

    this._singleton = instance
    return this
  }

  factory (factory) {
    if (this._singleton) {
      throw new Error('Instance already injected')
    }
    if (this._Type) {
      throw new Error('Constructor already injected')
    }
    this._factory = factory
    return this
  }

  class (type) {
    if (this._factory) {
      throw new Error('Factory already injected')
    }
    if (this._singleton) {
      throw new Error('Instance already injected')
    }

    this._Type = type
    return this
  }

  resolve (ioc) {
    let instance

    if (this._singleton) {
      instance = this._singleton
    } else if (this._factory) {
      instance = this._factory(ioc)
    } else if (this._Type) {
      instance = new this._Type()
    }

    if (instance) {
      ioc.resolveDependencies(instance)
    } else {
      console.error('Couldn\'t resolve dependency:', this._id)
    }

    return instance
  }
}
