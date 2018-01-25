import { System } from '@/js/lib/ecs'
import WorldMap from '@/js/lib/map/worldMap'

export class LoadMapSystem extends System {
  inject ({ pool }) {
    this.pool = pool
  }

  initialize () {
    const map = new WorldMap()
    map.prepare()
    this.pool.createEntity().addMap(map)
  }
}
