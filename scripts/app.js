(function() {
    'use strict';
    var socketUrl = 'ws://' + location.host;
    var WebSocket = new WebSocket(socketUrl);
    console.log(socketUrl, WebSocket);
    var app = {
        // webSocket = new WebSocket(socketUrl),
        heading: document.querySelector("h1"),
        subheading: document.querySelector("h2")
    };
})();
