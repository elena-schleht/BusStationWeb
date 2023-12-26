this.addEventListener('fetch', function (event) {
    if (
        event.request.url.startsWith('chrome-extension') ||
        event.request.url.includes('extension') ||
        !(event.request.url.indexOf('http') === 0)
    ) return;

    event.respondWith(
        caches.open('v1').then(function (cache) {
            return cache.match(event.request).then(function (response) {
                return response || fetch(event.request).then(function (response) {
                    cache.put(event.request, response.clone());
                    return response;
                });
            });
        })
    );
});

this.addEventListener('activate', function activator(event) {

    // remove old cache
    event.waitUntil(
        (async () => {
            const keys = await caches.keys();
            return keys.map(async cache => {
                if (cache !== 'v1') {
                    console.log('Service Worker: Removing old cache: ' + cache);
                    return await caches.delete(cache);
                }
            })
        })
    )

    event.waitUntil(
        caches.keys().then(function (keys) {
            return Promise.all(keys
                .filter(function (key) {
                    return key.indexOf('v1') !== 0;
                })
                .map(function (key) {
                    return caches.delete(key);
                })
            );
        })
    );
});