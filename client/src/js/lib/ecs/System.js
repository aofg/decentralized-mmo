export default class System {
  constructor () {
    this._executionSystems = []
    this._initializeSystems = []
    this._deinitializeSystems = []
    this._cleanupSystems = []
  }
  add (system) {
    return this.addSystem(system)
  }
  addSystem (system) {
    if (typeof system.execute === 'function') {
      this._executionSystems.push(system)
    }

    if (typeof system.initialize === 'function') {
      this._initializeSystems.push(system)
    }

    if (typeof system.deinitialize === 'function') {
      this._deinitializeSystems.push(system)
    }

    if (typeof system.cleanup === 'function') {
      this._cleanupSystems.push(system)
    }

    return this
  }

  initialize () {
    this._initializeSystems.forEach(system => system.initialize())
  }

  deinitialize () {
    this._deinitializeSystems.forEach(system => system.deinitialize())
  }

  execute () {
    this._executionSystems.forEach(system => system.execute())
  }
}

export class ReactiveSystem {
  constructor() {
    console.log('test')
  }

  execute() {
    // console.log(this.triggerOnEvent)
    const collected = this.triggerOnEvent.collected
    if (collected.length > 0) {
      this.executeTrigger(collected)
    }

    this.triggerOnEvent.clear()
    
    // const ensureComponents = this.ensureComponents
    // const excludeComponents = this.excludeComponents
    // const buffer = this._buffer
    // let j = buffer.length
    // if (Object.keys(collected).length != 0) {
    //   if (ensureComponents) {
    //     if (excludeComponents) {
    //       for (let k in collected) {
    //         const e = collected[k]
    //         if (ensureComponents.matches(e) && !excludeComponents.matches(e)) {
    //           buffer[j++] = e.addRef()
    //         }
    //       }
    //     } else {
    //       for (let k in collected) {
    //         const e = collected[k]
    //         if (ensureComponents.matches(e)) {
    //           buffer[j++] = e.addRef()
    //         }
    //       }
    //     }
    //   } else if (excludeComponents) {
    //     for (let k in collected) {
    //       const e = collected[k]
    //       if (!excludeComponents.matches(e)) {
    //         buffer[j++] = e.addRef()
    //       }
    //     }
    //   } else {
    //     for (let k in collected) {
    //       const e = collected[k]
    //       buffer[j++] = e.addRef()
    //     }
    //   }

    //   this._observer.clear()
    //   if (buffer.length != 0) {
    //     this._subsystem.execute(buffer)
    //     for (let i = 0, bufferCount = buffer.length; i < bufferCount; i++) {
    //       buffer[i].release()
    //     }
    //     buffer.length = 0
    //     if (this._clearAfterExecute) {
    //       this._observer.clear()
    //     }
    //   }
    // }

  }

  activate() {
    this.triggerOnEvent.activate()
  }

  deactivate() {
    this.triggerOnEvent.deactivate()
  }
  clear() {
    this.triggerOnEvent.clearCollectedEntities()
  }
}