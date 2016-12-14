(function() {
    'use strict';
    var socketUrl = 'ws://' + location.host;
    console.log(socketUrl);
    var app = {
        // webSocket = new WebSocket(socketUrl),
        heading: document.querySelector("h1"),
        subheading: document.querySelector("h2")
    };
})();