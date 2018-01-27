import { IComponent, ComponentOrIndex } from "./interfaces/IComponent"
import { IPool } from "./interfaces/IPool"
import { IMatcher } from "./interfaces/IMatcher"
import { IEntity } from "./interfaces/IEntity"
import { IGroup } from "./interfaces/IGroup"
import { IGroupObserver } from "./interfaces/IGroupObserver"

export class Matcher<T extends IComponent> implements IMatcher<T> {
    private _pool: IPool<T>
    private _allOf: Set<number>
    private _anyOf: Set<number>
    private _noneOf: Set<number>
    private _indexes: Set<number>
    private _pattern: string
    private _merged: boolean

    constructor(pool) {
        this._pool = pool
        this._allOf = new Set()
        this._anyOf = new Set()
        this._noneOf = new Set()
        this._pattern = ''
        this._indexes = new Set()
        this._merged = true
    }

    allOf(...components: ComponentOrIndex<T>[]) : IMatcher<T> {
        for (let type of components) {
            this._allOf.add(this._pool.getIndex(type))
        }
        this._merged = false

        return this
    }

    anyOf(...components: ComponentOrIndex<T>[]) : IMatcher<T> {
        for (let type of components) {
            this._anyOf.add(this._pool.getIndex(type))
        }
        this._merged = false

        return this
    }

    noneOf(...components : ComponentOrIndex<T>[]) : IMatcher<T> {
        for (let type of components) {
            this._noneOf.add(this._pool.getIndex(type))
        }
        this._merged = false

        return this
    }

    get indexes() : number[] {
        if (!this._merged) {
            this._merge()
        }

        return Array.from(this._indexes.values())
    }

    get pattern() : string {
        if (!this._merged) {
            this._merge()
        }

        return this._pattern
    }

    matches(entity : IEntity<T>) : boolean {
        const matchesAllOf = !this._allOf.size ? true : entity.hasComponents(Array.from(this._allOf.values()))
        const matchesAnyOf = !this._anyOf.size ? true : entity.hasAnyComponent(Array.from(this._anyOf.values()))
        const matchesNoneOf = !this._noneOf.size ? true : !entity.hasAnyComponent(Array.from(this._noneOf.values()))
        return matchesAllOf && matchesAnyOf && matchesNoneOf
    }

    _merge() : void {
        // TODO: simplify and optimize pattern construction
        let patternBuilder = []
        if (this._allOf.size) {
            patternBuilder.push('+')
            patternBuilder.push(Array.from(this._allOf.values()).join(','))
        }

        if (this._anyOf.size) {
            patternBuilder.push('|')
            patternBuilder.push(Array.from(this._anyOf.values()).join(','))
        }

        if (this._noneOf.size) {
            patternBuilder.push('-')
            patternBuilder.push(Array.from(this._noneOf.values()).join(','))
        }

        this._pattern = patternBuilder.join('')
        this._indexes.clear()

        for (let index of this._allOf.values()) {
            this._indexes.add(index)
        }
        for (let index of this._anyOf.values()) {
            this._indexes.add(index)
        }
        for (let index of this._noneOf.values()) {
            this._indexes.add(index)
        }
    }

    getGroup() : IGroup<T> {
        return this._pool._getGroup(this)
    }

    onUpdate() : IGroupObserver<T> {
        return this.getGroup().onUpdate()
    }
    onRemove() : IGroupObserver<T> {
        return this.getGroup().onRemove()
    }
    onAdd() : IGroupObserver<T> {
        return this.getGroup().onAdd()
    }
    onAddOrRemove() : IGroupObserver<T> {
        return this.getGroup().onAddOrRemove()
    }
    onAnyChanges() : IGroupObserver<T> {
        return this.getGroup().onAnyChanges()
    }
}