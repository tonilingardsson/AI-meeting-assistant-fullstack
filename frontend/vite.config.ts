import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

export default defineConfig({
    plugins: [react()],
    server: {
        watch: {
            ignored: ['**/.vs/**'],
        },
        proxy: {
            '/api': {
                target: 'http://localhost:5194',
                changeOrigin: true,
            },
        },
    },
})