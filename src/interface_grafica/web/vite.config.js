import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Planejador URL can be overridden via environment variable PLANEJADOR_URL
const planejadorUrl = process.env.PLANEJADOR_URL || 'http://localhost:54263'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': 'http://localhost:5000',
      '/auth': 'http://localhost:5000',
      '/meta': 'http://localhost:5000',
      // Development proxy for PlanejadorFluxo (Elsa Studio or proxy service)
      // Strip the /planejadorDeFluxo prefix when forwarding to the planner host
      '/planejadorDeFluxo': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/planejadorDeFluxo/, '')
      }
    }
  }
})
