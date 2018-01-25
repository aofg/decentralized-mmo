export class MobilesKernel {
  constructor () {
    this.$dependencies = [ 'WorldPool', 'EntitiesKernel' ]
  }

  $injected () {
    console.log(this.WorldPool, this.EntitiesKernel)
  }
}
