import * as PIXI from 'pixi.js'
import Game from '@/js/game'
// import 'pixi-tiledmap'
import '@/scss/main.scss'
// import path from 'path'
require('babel-core/register')
require('babel-polyfill')

PIXI.utils.sayHello()

let ww = window.innerWidth
let wh = window.innerHeight
const app = new PIXI.Application(ww, wh, {
  transparent: true,
  resolution: 1,
  autoResize: true
})
const { stage, renderer } = app

document.body.appendChild(app.view)

//! Load resource and run game 
PIXI.loader
  .add('data/terrain_atlas.png')
  .load(() => {
    stage.interactive = true

    const game = new Game({
      stage,
      renderer,
      app,
      resources: PIXI.loader.resources
    })

    let time = 0
    app.ticker.add(delta => {
      time += delta
      game.update(delta, time)
      app.render()
    })
  })
