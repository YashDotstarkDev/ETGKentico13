# ETG Cut

This project contains new astro components for the etg website. All the legacy components are also here.

Use 
```sh
webpack --mode production --config ./webpack.config.cjs
or
npm run build-legacy
```
To build the legacy components. These will be built to the /public/legacy folder.

Legacy widgets can use the same markup, but need a data-widget="widget-name" added to the parent element. 

This will be used to identify the widget and load the correct script.

Examples of this can be found in the src/components/legacy folder.

