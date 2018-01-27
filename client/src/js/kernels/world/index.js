import { System, Pool } from '@/js/lib/ecs'
import { Map, Center } from './components'
import { LoadMapSystem, SlideMapToCenterSystem } from './systems'

export class WorldPool extends Pool {
  constructor () {
    super(Map, Center)
  }
}

export class WorldKernel {
  constructor () {
    this.$dependencies = [
      'resources',
      'WorldPool'
    ]
  }

  static scenario (ioc) {
    return new System()
      .add(ioc.create(LoadMapSystem))
      .add(ioc.create(SlideMapToCenterSystem))
  }

  static get pools () {
    return [WorldPool]
  }
}
