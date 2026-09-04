const path = require('path')
const glob = require('glob')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const BeautifyHtmlWebpackPlugin = require('beautify-html-webpack-plugin')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const CopyWebpackPlugin = require('copy-webpack-plugin')

const reFileWithExtension = /([\w_-]*)\.?[^\\\/]*$/i


// widgets
const widgetsGlob = glob.sync('./src/_old-site/widgets/**/*.js')
const widgetEntries = widgetsGlob.reduce((entries, entry) => {
    const entryFileName = entry.match(reFileWithExtension)[1] // 'filename'
    // See: https://webpack.js.org/concepts/entry-points/#entrydescription-object
    entries[entryFileName] = entry
    return entries
}, {})

// pages
const pagesGlob = glob.sync('./src/_old-site/pages/**/*.js')
const pageEntries = pagesGlob.reduce((entries, entry) => {
    const entryFileName = entry.match(reFileWithExtension)[1] // 'filename'
    // See: https://webpack.js.org/concepts/entry-points/#entrydescription-object
    entries[entryFileName] = entry
    return entries
}, {})

const entries = {...widgetEntries, ...pageEntries}

module.exports = {
    entry: entries,

    externals: {
        jquery: 'jQuery', // Do not bundle jquery, included from CDN or `@Html.Kentico().PageBuilderScripts()`
    },

    output: {
        path: __dirname + '/public/legacy', // 'dist' folder
        publicPath: '/legacy/', // i.e. path to 'dist' relative to Kentico 'wwwroot' folder
        filename: 'scripts/[name].js', // NOTE controls output of webpack runtime file
        chunkFilename: 'scripts/chunks/[id].[contenthash].js' // See: https://webpack.js.org/configuration/output/#outputchunkfilename
    },

    optimization: {
        runtimeChunk: 'single',
        chunkIds: 'named', // See: https://webpack.js.org/configuration/optimization/#optimizationchunkids
        moduleIds: 'named', // See: https://webpack.js.org/configuration/optimization/#optimizationmoduleids

        splitChunks: {
            // IMPORTANT Only split 'async' chunks, so webpack handles loading of these chunks, and
            // we only need to link to bundles that we control from our named entries.
            chunks: 'async',

            cacheGroups: {
                vendor: {
                    test: /[\\/]node_modules[\\/]/,
                    chunks: 'async',
                    name (module) {
                        const packageName = module.context.match(/[\\/]node_modules[\\/](.*?)([\\/]|$)/)[1]
                        return `npm.${packageName.replace('@', '')}`
                    },
                },
            },
        },
    },

    module: {
        rules: [
            // Extract styles into `.css` bundle
            // for imports that match our 'include' paths
            {
                test: /\.(scss|css)$/,
                /*include: [
                  path.resolve(__dirname, 'src/styles/'),
                  path.resolve(__dirname, 'src/views/'),
                  path.resolve(__dirname, 'src/widgets/'),
                  path.resolve(__dirname, 'src/plugins/'),
                ],*/

                include: [
                    path.resolve(__dirname, 'src/styles/global.scss'),
                    path.resolve(__dirname, 'src/styles/print.scss')
                ],

                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                        options: {
                            // you can specify a publicPath here
                            // by default it uses publicPath in webpackOptions.output
                            publicPath: '../'
                        },
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: {
                                config: path.resolve(__dirname, 'postcss.config.cjs'),
                            },
                        },
                    }
                ]
            },

            // Handle styles
            // NOTE: we 'exclude' paths we included to be extracted (see above rule)
            {
                test: /\.(scss|css)$/,
                /*exclude: [
                  '/node_modules/',
                  path.resolve(__dirname, 'src/styles/'),
                  path.resolve(__dirname, 'src/views/'),
                  path.resolve(__dirname, 'src/widgets/'),
                  path.resolve(__dirname, 'src/plugins/'),
                ],*/

                exclude: [
                    '/node_modules/',
                    path.resolve(__dirname, 'src/styles/global.scss'),
                    path.resolve(__dirname, 'src/styles/print.scss')
                ],

                use: [
                    {
                        loader: 'style-loader'
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: {
                                config: path.resolve(__dirname, 'postcss.config.cjs'),
                            },
                        },
                    }
                ]
            },

            {
                test: /\.(gif|png|jpg|svg)$/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        name: 'images/[name].[ext]'
                    }
                }]
            },

            {
                test: /\.(eot|woff|woff2|ttf)(\?\S*)?$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: 'styles/fonts/',
                        publicPath: 'src/styles/fonts'
                    }
                }]
            },

            // Transpile scripts
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            },

            // Handles handlebars templates
            {
                test: /\.(hbs|handlebars)$/,
                loader: 'handlebars-loader'
            },

            // Handles Nunjucks templates
            {
                test: /\.(njk|nunjucks)$/,
                loader: 'simple-nunjucks-loader'
            }
        ]
    },

    plugins: [
        // new BundleAnalyzerPlugin(), // See: https://www.npmjs.com/package/webpack-bundle-analyzer

        // Cleans output folder
        new CleanWebpackPlugin(),

        // Extract CSS from scripts and output as `.css` files
        new MiniCssExtractPlugin({
            filename: 'styles/[name].css',
            chunkFilename: 'styles/chunks/[id].[contenthash].css',
        }),

        // Have webpack create a .html file and inject bundle chunks, so we have a record
        // of what is expected to be linked to within our master razor pages and 'pt' razor pages.
        new HtmlWebpackPlugin({
            filename: 'webpack.bundles.html',
            inject: true
        }),

        // Compile '/demo/pages' nunjucks to HTML files
        // new NunjucksWebpackPlugin(demoPagesTemplates),

        // IMPORTANT: Beautify should come after HTML creation, so
        // 'BeautifyHtmlWebpackPlugin' comes after 'HtmlWebpackPlugin' plugins.
        // new BeautifyHtmlWebpackPlugin(),

        // new CopyWebpackPlugin([
        //     {
        //         from: 'src/images',
        //         to: 'images'
        //     },
        //     {
        //         from: 'src/styles/fonts',
        //         to: 'styles/fonts'
        //     },
        //     {
        //         from: 'src/api',
        //         to: 'api'
        //     }
        // ]),

        /* NOTE not required for webpack v5
        new ChunkRenamePlugin(generateChunkNames()),*/
    ],

    /* webpack v4 devServer config
    devServer: {
      compress: true,
      port: 8080,
      publicPath: '/',
      contentBase: './src',
      watchContentBase: true,
      hot: true
    },*/

    // devServer: {
    //     port: 8080,
    //     static: [
    //         {
    //             directory: './dist',
    //             publicPath: '/',
    //         }, {
    //             directory: './src/api',
    //             publicPath: '/api',
    //         }
    //     ],
    //     compress: true,
    //     open: ['/index.html'],
    //     client: {
    //         logging: 'none', // See: https://webpack.js.org/configuration/dev-server/#logging
    //         overlay: false, // See: https://webpack.js.org/configuration/dev-server/#overlay
    //     },
    //     devMiddleware: {
    //         publicPath: '/',
    //         writeToDisk: true,
    //     },
    //     proxy: {
    //         // rewrite paths to correct for paths written relative to Kentico's public root folder, i.e. `wwwroot`
    //         // so everything will work for relative to our dev server public root, i.e. `devMiddleware.publicPath`
    //         '/client/dist': {
    //             target: 'http://localhost:8080',
    //             pathRewrite: { '^/client/dist': '/' },
    //         },
    //     },
    // },
}