import { System } from '@/js/lib/ecs'
import { Context } from '@/js/lib/ioc'
import { kernelFactory } from '@/js/lib/ecs-ioc'
import { WorldKernel } from './kernels/world'
import { EntitiesKernel } from './kernels/entities'
import { MobilesKernel } from './kernels/mobiles'
import { PlayerKernel } from './kernels/player'

export default class Game {
  constructor (options) {
    this.options = Object.assign({
      // TODO: Default options
    }, options)

    this.context = new Context()
    this.create()
  }

  create () {
    this.context.bind('WorldKernel').factory(kernelFactory(WorldKernel, this.context))
    this.context.bind('EntitiesKernel').factory(kernelFactory(EntitiesKernel, this.context))
    this.context.bind('MobilesKernel').factory(kernelFactory(MobilesKernel, this.context))
    this.context.bind('PlayerKernel').factory(kernelFactory(PlayerKernel, this.context))

    const worldKernel = this.context.resolve('WorldKernel')
    const worldScenario = this.context.resolve('WorldKernelScenario')
    console.log(System)
    this.scenario = new System()
      .add(worldScenario)

    this.scenario.initialize()
  }

  update (dt) {
    this.scenario.execute()
  }
}
