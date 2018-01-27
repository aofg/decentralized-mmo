import { System } from '@/js/lib/ecs'

export class LoadMapSystem extends System {
  constructor () {
    super()
    this.$dependencies = [ 'WorldPool' ]
  }

  initialize () {
  }
}

export class SlideMapToCenterSystem extends System {
  initialize () {
    console.log('would you like center your camera?')
  }
}
