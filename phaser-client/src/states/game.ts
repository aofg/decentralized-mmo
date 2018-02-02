import * as Assets from '../assets'
import { Game } from '../game/Game'

export default class GameState extends Phaser.State {
  private _game: Game
  create(phaser: Phaser.Game) {
    this._game = new Game({
      resources: phaser.cache,
      game: phaser
    })
  }

  update(game: Phaser.Game) {
    this._game.update(game.time.elapsed / 1000)
  }
}
