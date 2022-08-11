const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    '/tokengenerator',
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'http://localhost:3978/api/',
        secure: false,
        changeOrigin: true,
    });

    app.use(appProxy);
};
