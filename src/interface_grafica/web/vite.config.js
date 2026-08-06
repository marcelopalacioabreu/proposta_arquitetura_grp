import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': 'http://localhost:5000',
      '/auth': 'http://localhost:5000',
      '/meta': 'http://localhost:5000',
      // Development proxy for PlanejadorFluxo (Elsa Studio or proxy service)
      '/planejadorDeFluxo': 'http://localhost:6000'
    }
  }
})
