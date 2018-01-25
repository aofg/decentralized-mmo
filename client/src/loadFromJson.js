import * as PIXI from 'pixi.js'

export default function loadFromJson (
  name,
  pathToImage,
  data,
  resolution = parseInt(data.meta.scale, 10)
) {
  let loader = new PIXI.loaders.Loader()

  PIXI.loaders.spritesheetParser().call(loader, {
    name: name,
    url: pathToImage,
    data: data,
    isJson: true,
    metadata: {}
  }, function () {
    console.log('next', arguments, PIXI.utils.TextureCache)
  })

  return loader
}
