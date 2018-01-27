export class Bag<T> extends Array<T> {
    [ index : number ] : T
    
    private size_ : number

    constructor(capacity : number = 64) {
      super()
      this.size_ = 0
      this.length = capacity
    }
  
    removeAt(index : number) : T {
      const e = this[index]; // make copy of element to remove so it can be returned
      this[index] = this[--this.size_]; // overwrite item to remove with last element
      this[this.size_] = null; // null last element, so gc can do its work
      return e
    }
  
    remove(e : T) : boolean {
      let i : number
      let e2 : T
      const size = this.size_
  
      for (i = 0; i < size; i++) {
        e2 = this[i]
  
        if (e == e2) {
          this.removeAt(i)
          return true
        }
      }
  
      return false
    }
  
    contains(e : T) : boolean {
      let i : number
      let size : number
  
      for (i = 0, size = this.size_; size > i; i++) {
        if (e === this[i]) {
          return true
        }
      }
      return false
    }
  
    removeLast() : T | null {
      if (this.size_ > 0) {
        return this.removeAt(this.size_ - 1)
      }
  
      return null
    }
  
    removeAll(bag : Bag<T>) : boolean {
      let modified = false
      let i : number
      let j : number
      let l : number
      let e1 : T
      let e2 : T
  
      for (i = 0, l = bag.size; i < l; i++) {
        e1 = bag.get(i)
  
        for (j = 0; j < this.size_; j++) {
          e2 = this[j]
  
          if (e1 === e2) {
            this.removeAt(j)
            j--
            modified = true
            break
          }
        }
      }
  
      return modified
    }
  
    get(index) : T | undefined {
      if (index >= this.length || index < 0) {
        // TODO: Typed Exception
        throw new Error("BAG_ARGUMENT_OUT_OF_BOUNDS")
      }
  
      return this[index]
    }
  
    get size() : number {
      return this.size_
    }
  
    get capacity() : number {
      return this.length
    }
  
    isIndexWithinBounds(index) : boolean {
      return index < this.length
    }
  
    get empty() : boolean {
      return this.size_ == 0
    }
  
    add(element : T) : void {
      if (this.size_ === this.length) {
        this.grow()
      }
  
      this[this.size_++] = element
    }
  
    grow(newCapacity = ~~((this.length * 3) / 2) + 1) : void {
      this.length = ~~newCapacity
    }
  
    set(index : number, element : T) : void {
      if (index >= this.length) {
        this.grow(index * 2)
      }
  
      this.size_ = index + 1
      this[index] = element
    }
  
    ensureCapacity(index : number) : void {
      if (index >= this.length) {
        this.grow(index * 2)
      }
    }
  
    clear() : void{
      let i : number
      let size : number
      // null all elements so gc can clean up
      for (i = 0, size = this.size_; i < size; i++) {
        this[i] = null
      }
  
      this.size_ = 0
    }
  
    addAll(items : Bag<T>) : void {
      let i
  
      for (i = 0; items.size > i; i++) {
        this.add(items.get(i))
      }
    }
  }