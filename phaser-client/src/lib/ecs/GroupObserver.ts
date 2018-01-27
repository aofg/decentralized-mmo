import { IGroup } from "./interfaces/IGroup";
import { IComponent } from "./interfaces/IComponent";
import { IGroupObserver, GroupObserverTrigger } from "./interfaces/IGroupObserver";
import { IEntity } from "./interfaces/IEntity";


export class GroupObserver<T extends IComponent> implements IGroupObserver<T> {
    private _group : IGroup<T>
    private _trigger : GroupObserverTrigger
    private _collected : { [id : string] : IEntity<T> } = {}
    private _addEntityCache : any

    constructor(group : IGroup<T>, trigger : GroupObserverTrigger) {
        this._group = group
        this._trigger = trigger
        this._collected = {}
        this._addEntityCache = this.addEntity

        this.activate()
    }

    get collected() : IEntity<T>[] {
        return Object.keys(this._collected).map(key => this._collected[key])
    }

    activate() : void {
        switch (this._trigger) {
            case GroupObserverTrigger.ENTITY_ADD:
                this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
                this._group.onEntityAdded.add(this._addEntityCache.bind(this))
                break;
            case GroupObserverTrigger.ENTITY_UPDATE:
                this._group.onEntityUpdated.remove(this._addEntityCache.bind(this))
                this._group.onEntityUpdated.add(this._addEntityCache.bind(this))
                break;
            case GroupObserverTrigger.ENTITY_REMOVE:
                this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
                this._group.onEntityRemoved.add(this._addEntityCache.bind(this))
                break;
            case GroupObserverTrigger.ENTITY_ADD_OR_REMOVE:
                this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
                this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
                this._group.onEntityAdded.add(this._addEntityCache.bind(this))
                this._group.onEntityRemoved.add(this._addEntityCache.bind(this))
                break;
            case GroupObserverTrigger.ENTITY_ANY:
                this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
                this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
                this._group.onEntityUpdated.remove(this._addEntityCache.bind(this))
                this._group.onEntityAdded.add(this._addEntityCache.bind(this))
                this._group.onEntityRemoved.add(this._addEntityCache.bind(this))
                this._group.onEntityUpdated.add(this._addEntityCache.bind(this))
            default:
                // TODO: Typed exceptions
                throw new Error("GROUP_OBSERVER_INCORRECT_TRIGGER_TYPE " + this + this._trigger)
        }
    }

    deactivate() {
        this._group.onEntityAdded.remove(this._addEntityCache.bind(this))
        this._group.onEntityRemoved.remove(this._addEntityCache.bind(this))
        this._group.onEntityUpdated.remove(this._addEntityCache.bind(this))

        this.clear()
    }

    clear() {
        for (let e in this._collected) {
            this._collected[e].release(this)
        }
        this._collected = {}
    }

    addEntity(group, entity, index, component) {
        if (!(entity.id in this._collected)) {
            this._collected[entity.id] = entity
            entity.retain(this)
        }
    }
}