import { IoC } from './IoC'
export class Context {
    ioc: IoC
    constructor() {
        this.ioc = new IoC()
    }

    bind(identity) {
        return this.ioc.bind(identity)
    }

    create(Class) {
        return this.ioc.create(Class)
    }

    resolve(identity) {
        return this.ioc.resolve(identity)
    }
}
