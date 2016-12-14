webSocket.onopen = function() {
    console.log("connected");
};
webSocket.onmessage = function(evt) {
    console.log(evt.data);
};
webSocket.onerror = function(evt) {
    console.log(evt.message);
};
webSocket.onclose = function() {
    console.log("disconnected");
};

console.log(webSocket);