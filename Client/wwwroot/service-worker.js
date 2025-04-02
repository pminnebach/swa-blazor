// In production, we register a service worker to serve assets from local cache.

// This lets the app load faster on subsequent visits in production, and gives
// it offline capabilities. However, it also means that developers (and users)
// will only see deployed updates on the "N+1" visit to a page, since previously
// cached resources are updated in the background.

const CACHE_NAME = 'blazor-cache-v1';
const OFFLINE_URL = 'offline.html';

self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME).then(cache => {
            return cache.addAll([
                './',
                './index.html',
                './_framework/blazor.webassembly.js',
                './css/bootstrap/bootstrap.min.css',
                './css/app.css',
                './Client.styles.css',
                './manifest.json'
            ]);
        })
    );
});

self.addEventListener('fetch', event => {
    if (event.request.method !== 'GET') return;

    event.respondWith(
        fetch(event.request).catch(() => {
            return caches.match(event.request).then(response => {
                if (response) {
                    return response;
                }
                // When the cache doesn't have the resource
                return caches.match('./index.html');
            });
        })
    );
});

self.addEventListener('activate', event => {
    const cacheKeeplist = [CACHE_NAME];
    event.waitUntil(
        caches.keys().then(keyList => {
            return Promise.all(keyList.map(key => {
                if (cacheKeeplist.indexOf(key) === -1) {
                    return caches.delete(key);
                }
            }));
        })
    );
}); 