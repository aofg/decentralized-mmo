import { Context } from '../lib/ioc/Context'
import { kernelFactory } from '../lib/ecs-ioc/EcsFactories'
import { WorldKernel } from './kernels/world/WorldKernel'
import { Scenario } from '../lib/ecs/Scenario'

export interface IGameOptions {
  resources: Phaser.Cache,
  game: Phaser.Game
}
export class Game {
  public context: Context
  private _options: IGameOptions
  private _scenario: Scenario

  constructor(options) {
    this._options = Object.assign({
    }, options)

    this.context = new Context()
    this._scenario = new Scenario()
    this.create()
  }

  create() {
    this.context.bind('WorldKernel').factory(kernelFactory(WorldKernel, this.context.ioc))
    this.context.bind('Cache').singleton(this._options.resources)
    this.context.bind('Game').singleton(this._options.game)
    // this.context.bind('worldKernel').factory(kernelFactory(WorldKernel, this.context))
    // this.context.bind('entitiesKernel').factory(kernelFactory(EntitiesKernel, this.context))
    // this.context.bind('mobilesKernel').factory(kernelFactory(MobilesKernel, this.context))
    // this.context.bind('playerKernel').factory(kernelFactory(PlayerKernel, this.context))

    // const worldKernel = this.context.resolve('worldKernel')
    // console.log(worldKernel)

    this._scenario.add(
      this.context.resolve<Scenario>('WorldKernelScenario')
    )

    this._scenario.activate()
    this._scenario.initialize()
  }

  update(dt) {
    this._scenario.execute()
    this._scenario.cleanup()
  }
}
