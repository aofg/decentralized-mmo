# Decentralized MMORPG


[![in Progress](https://badge.waffle.io/waffleio/waffle.io.svg?label=waffle%3Ain%20progress&title=In%20Progress)](http://waffle.io/waffleio/waffle.io)

### Abstract

This paper as well as appendix repository discusses the main architectural considerations for a Massively Multiplayer Online Role Playing Game (MMORPG) without aspect of centralization. The traditional way to implement MMORPG is the client/server model with full authority on central server. We will test and prove alternative approach to separate authority around community (in other words – players).

### Motivation

25 years ago Massively Multiplayer Online Games _(MMOGs)_ appeared in the world of games and consistently creates huge cash flows for their operators. 

Creating and maintaining a workable state is a difficult task, but at the end of the path the reward is waiting and corresponds to the nested forces. 

Worst point is only hugest and powerful companies at market could pass through that way and don't want to share rewards as well as market with someone else. Idea of AOFG is making fully decentralized MMORPG with a power of open-source and crypto community. 

### Introduction

Almost any recent multiplayer games based on model with __distributed__ server architecture. That model allows to cut operational costs as well as provide better service for a customers (players). But idea of AOFG not just distribute servers and parallel computation, we want to __decentralize__ computation and validate all game state changes inside peer-to-peer (p2p) network.

Core challenge is providing mathematical proof of valid state for anyone in game without impact on player experience. 

##  Massively Multiplayer Online Games

MMORPG is very similar to simulation of the life inside sandbox with choicen rules. In that point of view we could setup base idea of simulation:

Let snapshot of __the State of the moment__ is ***S*** <-- latex is coming

And ***S*** is a set of all entities inside a simulation. Let set of entities will be an ***E***. 

## Proof of Concent (PoC) Implementation (Prototype)

With a reason of rapidly iteration and early achievement of results (or failure), we chose JavaScript as primary language for the MVC. 

* Client is based on [PixiJS v4](https://github.com/pixijs/pixi.js) and open-source arts [32x32 tilesets](https://opengameart.org/content/lpc-compatible-terraintiles)
* P2P networking is custom implementation of [Raft Consensus](https://raft.github.io/)
* Master Nodes of the core chains is the [Tendermint](https://tendermint.com) chains based on [Lotion](https://github.com/keppel/lotion) implementation


## Contribution

## Team
* **[Aler Denisov](https://github.com/alerdenisov)** – core developer and guy who was pretty craze to start it...

## Donate

BTC: **16wPwU9TFFEjxMRUNQ1rf9pN7Nf1kjaroV**

ETH: **0xc569011652c8206daf01775a01e4ba0ddb25dddf**