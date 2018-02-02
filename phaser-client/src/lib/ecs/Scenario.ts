import { ISystem } from './interfaces/systems/ISystem'
import { IExecuteSystem } from './interfaces/systems/IExecuteSystem'
import { IActivateSystem } from './interfaces/systems/IActivateSystem'
import { IDeactivateSystem } from './interfaces/systems/IDeactivateSystem'
import { IInitializeSystem } from './interfaces/systems/IInitializeSystem'
import { ICleanupSystem } from './interfaces/systems/ICleanupSystem'

function isExecuteSystem(system: any): boolean {
  return typeof system === 'object' && typeof system.execute === 'function'
}

function isCleanupSystem(system: any): boolean {
  return typeof system === 'object' && typeof system.cleanup === 'function'
}

function isActivateSystem(system: any): boolean {
  return typeof system === 'object' && typeof system.activate === 'function'
}

function isDeactivateSystem(system: any): boolean {
  return typeof system === 'object' && typeof system.deinitialize === 'function'
}

function isInitializeSystem(system: any): boolean {
  return typeof system === 'object' && typeof system.initialize === 'function'
}

export class Scenario implements ISystem, IExecuteSystem, IActivateSystem, IDeactivateSystem, IInitializeSystem, ICleanupSystem {
  private _executableSystems: IExecuteSystem[] = []
  private _initializableSystems: IInitializeSystem[] = []
  private _activatableSystems: IActivateSystem[] = []
  private _deactivatableSystems: IDeactivateSystem[] = []
  private _cleanupSystems: ICleanupSystem[] = []

  add(system: ISystem | IExecuteSystem | IActivateSystem | IDeactivateSystem | IInitializeSystem | ICleanupSystem): Scenario {
    if (isExecuteSystem(system)) {
      this._executableSystems.push(system as IExecuteSystem)
    }

    if (isCleanupSystem(system)) {
      this._cleanupSystems.push(system as ICleanupSystem)
    }

    if (isActivateSystem(system)) {
      this._activatableSystems.push(system as IActivateSystem)
    }

    if (isDeactivateSystem(system)) {
      this._deactivatableSystems.push(system as IDeactivateSystem)
    }

    if (isInitializeSystem(system)) {
      this._initializableSystems.push(system as IInitializeSystem)
    }
    return this
  }

  cleanup(): void {
    this._cleanupSystems.forEach(system => system.cleanup())
  }
  initialize(): void {
    this._initializableSystems.forEach(system => system.initialize())
  }
  deactivate(): void {
    this._deactivatableSystems.forEach(system => system.deactivate())
  }
  activate(): void {
    this._activatableSystems.forEach(system => system.activate())
  }
  execute(): void {
    this._executableSystems.forEach(system => system.execute())
  }
}
