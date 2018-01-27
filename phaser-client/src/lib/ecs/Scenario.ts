import { ISystem } from "./interfaces/systems/ISystem";
import { IExecuteSystem } from "./interfaces/systems/IExecuteSystem";
import { IActivateSystem } from "./interfaces/systems/IActivateSystem";
import { IDeactivateSystem } from "./interfaces/systems/IDeactivateSystem";
import { IInitializeSystem } from "./interfaces/systems/IInitializeSystem";
import { ICleanupSystem } from "./interfaces/systems/ICleanupSystem";

export class Scenario implements ISystem, IExecuteSystem, IActivateSystem, IDeactivateSystem, IInitializeSystem, ICleanupSystem {
    private _executableSystems : IExecuteSystem[]
    private _initializableSystems : IInitializeSystem[]
    private _activatableSystems : IActivateSystem[]
    private _deactivatableSystems : IDeactivateSystem[]
    private _cleanupSystems : ICleanupSystem[]

    add(system : ISystem | IExecuteSystem | IActivateSystem | IDeactivateSystem | IInitializeSystem | ICleanupSystem) : Scenario {
        if (<IExecuteSystem> system) {
            this._executableSystems.push(<IExecuteSystem> system)
        }

        if (<ICleanupSystem> system) {
            this._cleanupSystems.push(<ICleanupSystem> system)
        }

        if (<IActivateSystem> system) {
            this._activatableSystems.push(<IActivateSystem> system)
        }

        if (<IDeactivateSystem> system) {
            this._deactivatableSystems.push(<IDeactivateSystem> system)
        }

        if (<IInitializeSystem> system) {
            this._initializableSystems.push(<IInitializeSystem> system)
        }

        if (<ICleanupSystem> system) {
            this._cleanupSystems.push(<ICleanupSystem> system)
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