import React from 'react'

export default function PainelPlanejadorFluxo(){
  // Load Elsa Studio at root path so Blazor routes to '/' then redirects to /workflows
  // Negative margin cancels the p-3 padding from LayoutPrivado to give the iframe full bleed
  return (
    <div style={{ margin:'-16px', height:'calc(100vh - var(--menu-top, 56px))', overflow:'hidden' }}>
      <iframe
        title="Planejador de Fluxo"
        src="/planejadorDeFluxo/"
        style={{ border:0, width:'100%', height:'100%', display:'block' }}
      />
    </div>
  )
}
