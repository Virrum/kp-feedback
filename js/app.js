var statusSpan = document.getElementById("spanStatus");
var webSocket = new WebSocket("ws://localhost:5000");
var changeMeColorBody = document.getElementsByClassName("change-me-color");
var counterOfThumbDowns = 0;
var counterOfThumbUps = 0;

var buttons = document.getElementsByClassName("reactionButton");

if (statusSpan != null) {
    webSocket.onopen = function() {
        statusSpan.textContent = "connected";
    };
    webSocket.onclose = function() {
        statusSpan.textContent = "disconnected";
    };

}
webSocket.onmessage = function(evt) {
    if (changeMeColorBody.length > 0) {

        var socketListening = document.getElementById("socketListening");
        var thumbUpCounterLabel = document.getElementById("thumbCounterUp");
        var thumbDownCounterLabel = document.getElementById("thumbCounterDown");

        if (socketListening != null) {
            if (parseInt(evt.data) == -1) {
                socketListening.textContent = "👎";
                document.body.style.backgroundColor = "salmon";
                counterOfThumbDowns++;
                thumbDownCounterLabel.textContent = counterOfThumbDowns;
                if (counterOfThumbDowns % 5 == 0) {
                    socketListening.textContent = "💩";
                    document.body.style.backgroundColor = "#FFA500";
                }
            } else {
                counterOfThumbUps++;
                thumbUpCounterLabel.textContent = counterOfThumbUps;
                socketListening.textContent = "👍";
                document.body.style.backgroundColor = "lightgreen";
            }
        }
    }
};
webSocket.onerror = function(evt) {
    alert(evt.message);
};

if (buttons != null) {
    for (var i = 0; i < buttons.length; ++i) {
        buttons[i].addEventListener("click", sendReaction, false);
    }
}

function sendReaction(params) {
    var buttonClickedValue = params.target.value;

    if (webSocket.readyState == WebSocket.OPEN) {
        webSocket.send(buttonClickedValue);
    } else {
        statusSpan.text("Connection is closed");
    }
}