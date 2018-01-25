export const ERRORS = {
  POOL_INDEX_NOT_FOUND: 0,
  POOL_INCORRECT_COMPONENT_TYPE: 1,

  ENTITY_HAS_NO_COMPONENT: 100,
  ENTITY_ALREADY_HAS_COMPONENT: 101,
  ENTITY_IS_ALREADY_RELEASED: 102,

  GROUP_NOT_SINGLE_ENTITY_GROUP: 200,
  GROUP_OBSERVER_INCORRECT_TRIGGER_TYPE: 201,

  BAG_ARGUMENT_OUT_OF_BOUNDS: 1000
}

export const TRIGGERS = {
  ENTITY_ADD: 0,
  ENTITY_UPDATE: 1,
  ENTITY_REMOVE: 2,
  ENTITY_ADD_OR_REMOVE: 3,
  ENTITY_ANY: 4
}

export { default as Component, Flag, Singleton, SingleFlag } from './Component'
export { default as Entity } from './Entity'
export { default as Group } from './Group'
export { default as HashSet } from './HashSet'
export { default as Matcher } from './Matcher'
export { default as Pool } from './Pool'
export { default as System, ReactiveSystem } from './System'
export { default as Signal } from './Signal'
export { default as Bag } from './Bag'
export { default as UUID } from './UUID'
