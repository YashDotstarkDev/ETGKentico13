/** @type {import('tailwindcss').Config} */

import plugin from 'tailwindcss/plugin'

export default {
	content: ['./src/**/*.{astro,html,js,jsx,md,mdx,svelte,ts,tsx,vue}'],
	safelist: ['right-[-5vw]','left-[-5vw]','right-[-2vw]','left-[-2vw]','right-[-7vw]','left-[-7vw]', 'opacity-10', 'opacity-20', 'opacity-30', 'opacity-40', 'opacity-50', 'opacity-60', 'opacity-70', 'opacity-80', 'opacity-90'],
	theme: {
		extend: {
			gridTemplateColumns: {
				'20': 'repeat(20, minmax(0, 1fr))',
				'40': 'repeat(40, minmax(0, 1fr))',
			},
			typography: {
				DEFAULT: {
					css: {
						color: '#000',
						a: {
							color: '#091D3A',
							'&:hover': {
								color: '#1A6F95',
							},
						},
					},
				},
			},
			height: {
				screen: ['100vh /* fallback for Opera, IE and etc. */', '100svh'],
			},
			minHeight: {
				screen: ['100vh /* fallback for Opera, IE and etc. */', '100svh'],
			},
			maxHeight: {
				"80vh": ['80vh /* fallback for Opera, IE and etc. */', '80svh'],
			},
			colors: {
				primary: "rgb(var(--colour-primary) / <alpha-value>)",
				body: "rgb(var(--colour-body) / <alpha-value>)",
				secondary: "rgb(var(--colour-secondary) / <alpha-value>)",
				purple: "rgb(var(--colour-purple) / <alpha-value>)",
				green: "rgb(var(--colour-green) / <alpha-value>)",
				grey: "rgb(var(--colour-grey) / <alpha-value>)",
			},
			screens: {
				xs: "375px",
			},
			fontFamily: {
				headings: ['Raleway', 'sans-serif'],
				body: ['Lato', 'sans-serif'],
				serif: ['serif'],
			},
			backgroundImage: {
				"select-arrow": "url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTUiIGhlaWdodD0iOSIgdmlld0JveD0iMCAwIDE1IDkiIGZpbGw9Im5vbmUiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+CjxwYXRoIGQ9Ik02Ljg4MTM1IDguMjE4NzVMMC44ODEzNDggMi4yMTg3NUMwLjQ3NTA5OCAxLjg0Mzc1IDAuNDc1MDk4IDEuMTg3NSAwLjg4MTM0OCAwLjgxMjVDMS4yNTYzNSAwLjQwNjI1IDEuOTEyNiAwLjQwNjI1IDIuMjg3NiAwLjgxMjVMNy42MDAxIDYuMDkzNzVMMTIuODgxMyAwLjgxMjVDMTMuMjU2MyAwLjQwNjI1IDEzLjkxMjYgMC40MDYyNSAxNC4yODc2IDAuODEyNUMxNC42OTM4IDEuMTg3NSAxNC42OTM4IDEuODQzNzUgMTQuMjg3NiAyLjIxODc1TDguMjg3NiA4LjIxODc1QzcuOTEyNiA4LjYyNSA3LjI1NjM1IDguNjI1IDYuODgxMzUgOC4yMTg3NVoiIGZpbGw9IiNjYTU2OGUiLz4KPC9zdmc+')",
			},
		}
	},
	plugins: [
		plugin(function ({ addVariant }) {
			addVariant("pointer-coarse", "@media (pointer: coarse)");
			addVariant("pointer-fine", "@media (pointer: fine)");
		}),
		require('@tailwindcss/typography'),
		require('@tailwindcss/container-queries'),
	],
}
