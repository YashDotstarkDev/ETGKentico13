import {defineConfig} from 'astro/config';

import tailwind from '@astrojs/tailwind';

import vue from '@astrojs/vue';

// https://astro.build/config
export default defineConfig({
    integrations: [tailwind(), vue()],
    vite: {
        css: {
            preprocessorOptions: {
                scss: {
                    api: "modern-compiler"
                }
            }
        },
        build: {
            inlineStylesheets: "never",
            assetsInlineLimit: 0,
            rollupOptions: {
                output: {
                    entryFileNames: "scripts/[name].js",
                    chunkFileNames: "chunks/[name]-[hash].js",
                    assetFileNames: "assets/[name]-[hash][extname]",
                },
            },
        }
    }
});
