import React from 'react'

// Simple wrapper that loads the Elsa designer via an iframe proxied through /planejadorDeFluxo
export default function PainelPlanejadorFluxo(){
  const src = '/planejadorDeFluxo/studio' // proxy path; target configured in PlanejadorFluxo service
  return (
    <div style={{height: '100%', width: '100%', margin: '-16px -12px -16px 20px'}}>
      <iframe title="Planejador de Fluxo" src={src} style={{border:0, width:'100%', height:'90vh'}} />
    </div>
  )
}
