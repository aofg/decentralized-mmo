import { Bag } from "./Bag";

export interface ISignal<T> {
    dispatch(...args: any[]): void
    add(listener: T): void
    clear(): void
    remove(listener: T): void
}

export class Signal<T extends (...any) => void> implements ISignal<T> {
    public _listeners: Bag<T>
    private _context
    private _alloc: number
    public active: boolean

    constructor(context, alloc: number = 16) {
        this._listeners = new Bag<T>()
        this._context = context
        this._alloc = alloc
        this.active = false
    }
    dispatch(...args: any[]): void {
        const listeners: Bag<T> = this._listeners
        const size = listeners.size
        if (size <= 0) return; // bail early
        const context = this._context
        for (let i = 0; i < size; i++) {
            listeners[i].apply(context, args)
        }
    }
    add(listener: T): void {
        this._listeners.add(listener)
        this.active = true
    }
    remove(listener: T): void {
        const listeners = this._listeners
        listeners.remove(listener)
        this.active = listeners.size > 0
    }
    clear(): void {
        this._listeners.clear()
        this.active = false
    }
}