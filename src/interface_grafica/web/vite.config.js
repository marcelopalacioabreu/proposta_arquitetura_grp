import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Planejador URL can be overridden via environment variable PLANEJADOR_URL
// Default to localhost:6001 where the planner will listen in dev (6000 is blocked by some browsers)
const planejadorUrl = process.env.PLANEJADOR_URL || 'http://localhost:6001'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': 'http://localhost:5000',
      '/auth': 'http://localhost:5000',
      '/meta': 'http://localhost:5000',
      // Elsa API: routed through GRP API (port 5000) reverse proxy
      // This ensures cookies created by GRP are sent with Elsa requests
      '/elsa': {
        target: 'http://localhost:5000',
        changeOrigin: true
      },
      // Auth check endpoint for CookieAuthStateProvider (points to GRP API, NOT PlanejadorFluxo)
      '/identity': {
        target: 'http://localhost:5000',
        changeOrigin: true
      },
      // Blazor WASM runtime and package static assets (absolute paths in _Host.cshtml)
      '/_framework': { target: planejadorUrl, changeOrigin: true },
      '/_content': { target: planejadorUrl, changeOrigin: true },
      '/_blazor': { target: planejadorUrl, changeOrigin: true },
      // Elsa Studio host page loader — changeOrigin:false keeps Host:localhost:5173 so that
      // _Host.cshtml computes apiUrl=http://localhost:5173/elsa/api (proxied by /elsa above)
      '/planejadorDeFluxo': {
        target: planejadorUrl,
        changeOrigin: false,
        rewrite: (path) => path.replace(/^\/planejadorDeFluxo/, ''),
        headers: {
          // Tells _Host.cshtml to set <base href="/planejadorDeFluxo/"> so all Blazor resource
          // fetches (appsettings.json, _framework/*.wasm, etc.) go through this proxy
          'X-Forwarded-Prefix': '/planejadorDeFluxo'
        }
      }
    }
  }
})
