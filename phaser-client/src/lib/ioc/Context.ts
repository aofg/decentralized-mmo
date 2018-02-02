import { IoC, BindingPipe, ClassRef, DependencyRequest } from './IoC'

export class Context {
  ioc: IoC
  constructor() {
    this.ioc = new IoC()
  }

  bind(identity): BindingPipe {
    return this.ioc.bind(identity)
  }

  create<T>(ref: ClassRef): T {
    return this.ioc.create(ref)
  }

  resolve<T>(request: DependencyRequest): T {
    return this.ioc.resolve<T>(request)
  }

  resolveAny(request: DependencyRequest): any {
    return this.ioc.resolveAny(request)
  }
}
