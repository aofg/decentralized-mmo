import * as PIXI from 'pixi.js'
import Game from '@/js/game'
import '@/scss/main.scss'
require('babel-core/register')
require('babel-polyfill')

let ww = window.innerWidth
let wh = window.innerHeight
const app = new PIXI.Application(ww, wh, {
  antialias: false,
  transparent: false,
  resolution: 1,
  backgroundColor: 0x061639,
  autoResize: true
})

const game = new Game({})

app.ticker.add(delta => game.update(delta))
