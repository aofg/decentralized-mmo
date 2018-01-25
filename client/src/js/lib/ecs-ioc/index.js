export function scenarioFactory (scenario) {
  return (context) => scenario(context)
}

export function kernelFactory (Type, context) {
  if (Type.pools && Type.pools.length) {
    Type.pools.forEach(pool => {
      context.bind(pool.name).class(pool)
    })
  }

  if (Type.scenario) {
    console.log(context.bind('test'))
    context.bind(Type.name + 'Scenario').factory(scenarioFactory(Type.scenario))
  }

  return (ioc) => new Type()
}
