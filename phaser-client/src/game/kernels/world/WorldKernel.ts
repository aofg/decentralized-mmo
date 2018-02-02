import { Pool } from '../../../lib/ecs/Pool'
import { IComponent, Singleton } from '../../../lib/ecs/interfaces/IComponent'
import * as Phaser from 'phaser-ce'
import { inject } from 'inversify'
import { Scenario } from '../../../lib/ecs/Scenario'
import { IoC, DependencyRequest } from '../../../lib/ioc/IoC'
import { IPool } from '../../../lib/ecs/interfaces/IPool'
import { IExecuteSystem } from '../../../lib/ecs/interfaces/systems/IExecuteSystem'
import { ISystem } from '../../../lib/ecs/interfaces/systems/ISystem'
import { IInitializeSystem } from 'lib/ecs/interfaces/systems/IInitializeSystem'

import * as Assets from '../../../assets'

export interface IWorldComponent extends IComponent { }

export interface TileData {
  set: number,
  index: number
}

export interface MapChunkData {
  tiles: TileData[]
}

export class MapChunk implements IWorldComponent {
  public x: number
  public y: number
  public data: MapChunkData
  create(x: number, y: number, data: MapChunkData) {
    this.x = x
    this.y = y

    // ? Should I copy data?
    this.data = data
  }
}

export class Tileset implements IWorldComponent {
  public index: number
  public texture: PIXI.BaseTexture

  create(index: number, texture: PIXI.BaseTexture) {
    this.index = index
    this.texture = texture

    console.log(texture)
  }
}

export class Tilemap extends Singleton implements IWorldComponent {
  public map: Phaser.Tilemap

  create(map: Phaser.Tilemap) {
    this.map = map
  }
}

export class WorldPool extends Pool<IWorldComponent> {
  constructor() {
    super(MapChunk, Tileset, Tilemap)
  }

  get map(): Tilemap {
    return this.getSingle<Tilemap>(Tilemap)
  }
}

export class DebugExecuteSystem implements ISystem, IInitializeSystem {
  public WorldPool: WorldPool
  public WorldKernel: WorldKernel
  public Cache: Phaser.Cache
  public Game: Phaser.Game

  public $dependencies: DependencyRequest[] = [
    'WorldPool',
    'WorldKernel',
    'Cache',
    'Game'
  ]

  initialize(): void {
    // console.log(Assets.Spritesheets.SpritesheetsTerrain3232483.getPNG())
    // console.log(this.Cache.getAssets.Spritesheets.SpritesheetsTerrain3232483.getPNG())
    // this.WorldPool.createEntity().add(Tileset, 0, this.Cache.getBaseTexture(Assets.Spritesheets.SpritesheetsCastle3232152.getName()))
    const tilemap = this.Game.add.tilemap()
    this.WorldPool.createEntity().add(Tilemap, tilemap)

    this.WorldPool.map.create()
  }
}

export class WorldKernel {
  static scenario(ioc: IoC): Scenario {
    return new Scenario()
      .add(ioc.create(DebugExecuteSystem))
    // .add(ioc.create(LoadMapSystem))
    // .add(ioc.create(SlideMapToCenterSystem))
  }

  static get pools(): any[] {
    return [WorldPool]
  }
}
