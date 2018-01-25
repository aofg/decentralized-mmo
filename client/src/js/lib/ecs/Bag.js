export default class Bag extends Array {
  constructor(capacity = 64) {
    super()
    this.size_ = 0
    this.length = capacity
  }

  removeAt(index) {
    const e = this[index]; // make copy of element to remove so it can be returned
    this[index] = this[--this.size_]; // overwrite item to remove with last element
    this[this.size_] = null; // null last element, so gc can do its work
    return e
  }

  remove(element) {
    let i
    let e2
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

  contains(e) {
    let i
    let size

    for (i = 0, size = this.size_; size > i; i++) {
      if (e === this[i]) {
        return true
      }
    }
    return false
  }

  removeLast() {
    if (this.size_ > 0) {
      return removeAt(this.size_ - 1)
    }

    return null
  }

  removeAll(bag) {
    let modified = false
    let i
    let j
    let l
    let e1
    let e2

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

  get(index) {
    if (index >= this.length || index < 0) {
      throw new Error(ERRORS.BAG_ARGUMENT_OUT_OF_BOUNDS)
    }

    return this[index]
  }

  get size() {
    return this.size_
  }

  get capacity() {
    return this.length
  }

  isIndexWithinBounds(index) {
    return index < this.length
  }

  get empty() {
    return this.size_ == 0
  }

  add(element) {
    if (this.size_ === this.length) {
      this.grow()
    }

    this[this.size_++] = element
  }

  grow(newCapacity = ~~((this.length * 3) / 2) + 1) {
    this.length = ~~newCapacity
  }

  set(index, element) {
    if (index >= this.length) {
      this.grow(index * 2)
    }

    this.size_ = index + 1
    this[index] = element
  }

  ensureCapacity(index) {
    if (index >= this.length) {
      this.grow(index * 2)
    }
  }

  clear() {
    let i
    let size
    // null all elements so gc can clean up
    for (i = 0, size = this.size_; i < size; i++) {
      this[i] = null
    }

    this.size_ = 0
  }

  addAll(items) {
    let i

    for (i = 0; items.size > i; i++) {
      this.add(items.get(i))
    }
  }
}