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
      // Development proxy for PlanejadorFluxo (Elsa Studio or proxy service)
      // Strip the /planejadorDeFluxo prefix when forwarding to the planner host
      '/planejadorDeFluxo': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/planejadorDeFluxo/, ''),
        headers: {
          'X-Forwarded-Prefix': '/planejadorDeFluxo'
        }
      }
      ,
      // Proxy the embedded painel route used by the app so requests like
      // /painel/planejadorFluxo/_framework/* are forwarded to the planner's /_framework/*
      '/painel/planejadorDeFluxo': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/painel\/planejadorDeFluxo/, ''),
        headers: {
          'X-Forwarded-Prefix': '/painel/planejadorDeFluxo'
        }
      }
      ,
      // Proxy Blazor WASM static assets and runtime requests to the planner host
      // so absolute paths like /_framework/* and /_content/* are served correctly.
      '/_framework': {
        target: planejadorUrl,
        changeOrigin: true
      },
      '/_content': {
        target: planejadorUrl,
        changeOrigin: true
      },
      '/_blazor': {
        target: planejadorUrl,
        changeOrigin: true
      }
      ,
      // Explicit proxies for prefixed requests (embedded paths)
      '/planejadorDeFluxo/_framework': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/planejadorDeFluxo/, '')
      },
      '/planejadorDeFluxo/_content': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/planejadorDeFluxo/, '')
      },
      '/painel/planejadorDeFluxo/_framework': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/painel\/planejadorDeFluxo/, '')
      },
      '/painel/planejadorDeFluxo/_content': {
        target: planejadorUrl,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/painel\/planejadorDeFluxo/, '')
      }
    }
  }
})
