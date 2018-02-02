export type Factory<T> = (context: IoC) => T
export type ClassRef = new (...args: any[]) => any
export interface IDependencyRequest {
  to: string,
  identity: string
}
export type DependencyRequest = string | IDependencyRequest | ClassRef

function isClassRef(request: any): boolean {
  return typeof request === 'function' && typeof request.name === 'string'
}

function isDependencyRequest(request: any): boolean {
  return typeof request === 'object' && typeof request.to === 'string' && typeof request.identity === 'string'
}

function isString(request: any): boolean {
  return typeof request === 'string'
}

export class IoC {
  private _pipes: { [identity: string]: BindingPipe } = {}

  bind(identity: string): BindingPipe {
    const pipe = new BindingPipe(identity)
    this._pipes[identity] = pipe
    return pipe
  }

  create(ref: ClassRef, ...args: any[]) {
    const instance = new ref(...args)
    this.resolveDependencies(instance)
    return instance
  }

  resolveDependencies(instance: any): any {
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

  resolve<T>(request: DependencyRequest): T {
    return this.resolveAny(request) as T
  }

  resolveAny(request: DependencyRequest): any {
    const pipe = this._pipes[this.getIdentity(request)]
    if (pipe) {
      return pipe.resolve(this)
    }

    if (isClassRef(request)) {
      console.log('Resolving class ref withount pipe. Use ioc.create instead of resolve')
      return this.create(request as ClassRef)
    }

    console.error('Couldn\'t resolve dependency:', request)
    return null
  }

  getIdentity(request: DependencyRequest): string {
    if (isClassRef(request)) {
      return (request as ClassRef).name
    }
    if (isDependencyRequest(request)) {
      return (request as IDependencyRequest).identity
    }
    if (isString(request)) {
      return request as string
    }

    // TODO: Typed exceptions
    throw new Error('Incorrect request dependency ' + request)
  }

  getField(request: DependencyRequest): string {
    if (isClassRef(request)) {
      return (request as ClassRef).name
    }
    if (isDependencyRequest(request)) {
      return (request as IDependencyRequest).to
    }
    if (isString(request)) {
      return request as string
    }

    // TODO: Typed exceptions
    throw new Error('Incorrect request dependency ' + request)
  }
}

export class BindingPipe {
  private _id: string
  private _factory?: (ioc: IoC) => any = null
  private _singleton?: any = null
  private _ref?: ClassRef = null

  constructor(identity: string) {
    this._id = identity
  }

  singleton(instance) {
    if (this._factory) {
      throw new Error('Factory already injected')
    }
    if (this._ref) {
      throw new Error('Constructor already injected')
    }

    this._singleton = instance
    return this
  }

  factory(factory) {
    if (this._singleton) {
      throw new Error('Instance already injected')
    }
    if (this._ref) {
      throw new Error('Constructor already injected')
    }
    this._factory = factory
    return this
  }

  class(type: ClassRef): BindingPipe {
    if (this._factory) {
      throw new Error('Factory already injected')
    }
    if (this._singleton) {
      throw new Error('Instance already injected')
    }

    this._ref = type
    return this
  }

  resolve(ioc) {
    let instance

    if (this._singleton) {
      instance = this._singleton
    } else if (this._factory) {
      instance = this._factory(ioc)
    } else if (this._ref) {
      instance = new this._ref()
    }

    if (instance) {
      ioc.resolveDependencies(instance)
    } else {
      console.error('Couldn\'t resolve dependency:', this._id)
    }

    return instance
  }
}
