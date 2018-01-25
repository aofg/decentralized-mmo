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
    this.context.bind('worldKernel').factory(kernelFactory(WorldKernel, this.context))
    this.context.bind('entitiesKernel').factory(kernelFactory(EntitiesKernel, this.context))
    this.context.bind('mobilesKernel').factory(kernelFactory(MobilesKernel, this.context))
    this.context.bind('playerKernel').factory(kernelFactory(PlayerKernel, this.context))

    const worldKernel = this.context.resolve('worldKernel')
    console.log(worldKernel)
  }

  update (dt) {

  }
}
