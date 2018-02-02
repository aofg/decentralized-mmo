# Decentralized MMORPG

### Abstract

This paper as well as appendix repository discusses the main architectural considerations for a Massively Multiplayer Online Role Playing Game (MMORPG) without aspect of centralization. The traditional way to implement MMORPG is the client/server model with full authority on central server. We will test and prove alternative approach to separate authority around community (in other words – players).

### Motivation

25 years ago Massively Multiplayer Online Games _(MMOGs)_ appeared in the world of games and consistently creates huge cash flows for their operators. 

Creating and maintaining a workable state is a difficult task, but at the end of the path the reward is waiting and corresponds to the nested forces. 

Worst point is only hugest and powerful companies at market could pass through that way and don't want to share rewards as well as market with someone else. Idea of AOFG is making fully decentralized MMORPG with a power of open-source and crypto community. 

### Introduction

Almost any recent multiplayer games based on model with __distributed__ server architecture. That model allows to cut operational costs as well as provide better service for a customers (players). But idea of AOFG not just distribute servers and parallel computation, we want to __decentralize__ computation and validate all game state changes inside peer-to-peer (p2p) network.

Core challenge is providing mathematical proof of valid state for anyone in game without impact on player experience. 

## Simulation of the Worlds

MMORPG is very similar to simulation of the life inside sandbox with choicen rules. In that point of view we could setup base idea of simulation:

Let snapshot of __the State of the moment__ is $$S$$.

And $$S$$ a snapshot of the states of all _entities_ inside a simulation. So, in a case of MMORPG our mission is a calculation of the next $$S'$$ (the State of next moment) based on current $$S$$. Simple is that.

But what is $$S$$ or most correct what is set of all entities is? Let call entity as $$e$$ and set of entities as $$E_S$$. Easiest way is imagine entity as an abstract box\container. If $$S$$ could be described with any amount of $$e$$ (from $$0$$ to $$\infty$$), but each **$$e$$ is limited and pre-known set of components**. 

Component is a strict data structure without any logic and solve only one task — storing a data. Lets call components as $$c$$ add $$C$$ as a set of the components. 

So, if $$C_e$$ is a set of the corresponding $$e$$, then $$S$$ will be:

$$
\begin{equation}
\begin{aligned}
S &= \sum^{||E_S||}_{i=1} e_i, & e \in E_S \\
e &= \sum^{||C_e||}_{i=1} c_i, & c \in C_e
\end{aligned}
\end{equation}
$$

Now then we know what is $$S$$, let move to calculation of the $$S'$$. Let's assume that our simulation is about next features:

* **Shape** is a component of the shape and will be $$c_{shape}$$.
* **Color** is a component of the colorand will be $$c_{color}$$. Could be one of next: 

$$
\begin{equation}
\begin{aligned}
e &= [c_{shape}, c_{color}], \\ 
c_{shape} &\in [\mathbb{CUBE},\ \mathbb{SPHERE}] \ \wedge \\
c_{color} &\in [\mathbb{RED},\ \mathbb{YELLOW},\ \mathbb{GREEN}] \\
f(v, x) =&\ \{i \ | \ i \in \mathbb{N},  i > |v|, v_i = x \}
\end{aligned}
\end{equation}
$$

Rules of our simulation is a pretty simple. If shape component of the entity is a sphere, then color component of the entity will be _next_ color at the next moment of the Time.

$$
\begin{equation}
\begin{aligned}
f(e) &= e' \\
\\
f_{simulation}(e) &= \begin{cases}
[e_{shape}, f_{nextColor}(e)], & \text{if } e_{shape} = \mathbb{SPHERE} \\
    [e_{shape}, e_{color}], & \text{overwise }
\end{cases} \\
\\
f_{nextColor}(e) &= \begin{cases}
    \mathbb{RED}, & \text{if } e_{color} = \mathbb{GREEN} \\
    \mathbb{YELLOW}, &\text{if } e_{color} = \mathbb{RED} \\
    \mathbb{GREEN}, &\text{if } e_{color} = \mathbb{YELLOW}
  \end{cases}
\end{aligned}
\end{equation}
$$


I I# Proof of Concent (PoC) Implementation (Prototype)

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