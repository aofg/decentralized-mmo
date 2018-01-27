import path from 'path'
import HtmlWebpackPlugin from 'html-webpack-plugin'
import CleanWebpackPlugin from 'clean-webpack-plugin'

const defaultEnv = {
  dev: true,
  production: false
}

export default (env = defaultEnv) => ({
  entry: [path.resolve(__dirname, 'src/index.js')],
  output: {
    filename: '[name].bundle.js',
    path: path.resolve(__dirname, 'dist')
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        include: path.resolve(__dirname, 'src'),
        use: [
          {
            loader: 'babel-loader'
          }
        ]
      },

      {
        test: /\.scss$/,
        use: [
          {
            loader: 'style-loader', // creates style nodes from JS strings
            options: {
              sourceMap: true
            }
          },
          {
            loader: 'css-loader', // translate css into CommonJS
            options: {
              importLoaders: 1,
              sourceMap: true
            }
          },
          {
            loader: 'postcss-loader',
            options: {
              sourceMap: true
            }
          },
          {
            loader: 'sass-loader', // compiles sass to css
            options: {
              sourceMap: true
            }
          }
        ]
      },
      {
        test: /\.(png|jpg|gif)$/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 8192,
              name: 'images/[name].[hash:8].[ext]'
            }
          }
        ]
      },
      {
        test: /\.(html)$/,
        use: {
          loader: 'html-loader'
        }
      }
    ]
  },
  target: 'web',

  node: {
    fs: 'empty'
  },
  plugins: [
    ...(env.dev ? [] : [new CleanWebpackPlugin(['dist'])]),
    new HtmlWebpackPlugin({
      title: 'PIXI Starter',
      template: path.join(__dirname, 'src/index.html')
    })
  ],
  devtool: env.dev ? 'cheao-module-eval-source-map' : 'cheap-module-source-map',
  devServer: {
    contentBase: path.join(__dirname, 'dist'),
    compress: true,
    host: 'localhost',
    port: 9000
  }
})
