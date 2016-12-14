(function() {
    'use strict';
    var socketUrl = 'wss://' + location.host;
    var theSocket = new WebSocket(socketUrl);
    console.log(socketUrl, theSocket);
    var app = {
        // webSocket = new WebSocket(socketUrl),
        heading: document.querySelector("h1"),
        subheading: document.querySelector("h2")
    };
})();