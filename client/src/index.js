import * as PIXI from 'pixi.js'
import '@/scss/main.scss'
require('babel-core/register')
require('babel-polyfill')

let ww = window.innerWidth
let wh = window.innerHeight
const a = new PIXI.Application(ww, wh, {
  antialias: false,
  transparent: false,
  resolution: 1,
  backgroundColor: 0x061639,
  autoResize: true
})
console.log(a)