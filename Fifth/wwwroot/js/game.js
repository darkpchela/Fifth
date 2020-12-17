const homeUrl = "/Home/";
const crossElem = '<svg viewBox="4 4 8 8" class="bi bi-x" fill="currentColor" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/></svg>'
const circleElem = '<svg viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd"d="M8 15A7 7 0 1 0 8 1a7 7 0 0 0 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" /></svg>';
const cells = $('#gameboard button');
const disableField = () => cells.prop("disabled", true);
const enableField = () => cells.prop("disabled", false);
const disableCell = index => $(`#gameboard #${index}`).prop("disabled", true)
const goHome = () => window.location.replace(homeUrl);
const setCharToCell = (index, elem) => $(`#gameboard #${index}`).append(elem);
const getElemId = elem => $(elem).attr('id');
const userNameElems = $('div[name="userName"] h3');
const turnOffOnSuccess = (elem) => elem.hasClass("text-success") ? elem.removeClass("text-success") : elem.addClass("text-success");
const showStartModal = () => $('#modalStart').modal('show');
const showResultModal = (resultStr) => {
    $('#result').text(resultStr);
    $('#modalResult').modal('show');
};

let myChar;
let isMyMove = false;
let myConnectionId;

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/GameHub")
    .build();

hubConnection.on("AcceptConnectionId", id => {
    myConnectionId = id;
})

hubConnection.on("AcceptPlayersInfo", (players) => {
    for (let i = 0; i < players.length; i++) {
        $(userNameElems[i]).attr("data-id", players[i].connectionId)
        $(userNameElems[i]).text(players[i].userName);
    }
});

hubConnection.on("OnGameStarted", data => {
    showStartModal();
    if (isMyMove === true) {
        turnOffOnSuccess($(`h3[data-id=${myConnectionId}]`));
        enableField();
    }
    else {
        turnOffOnSuccess($(`h3[data-id!=${myConnectionId}]`));
    }
});

hubConnection.on("OnMoveMaid", index => {
    disableCell(index);
    if (isMyMove) {
        disableField();
        myChar == "o" ? setCharToCell(index, circleElem) : setCharToCell(index, crossElem);
    }
    else {
        enableField();
        myChar == "o" ? setCharToCell(index, crossElem) : setCharToCell(index, circleElem);
    }
    turnOffOnSuccess($(`h3[data-id=${myConnectionId}]`));
    turnOffOnSuccess($(`h3[data-id!=${myConnectionId}]`));
    isMyMove = !isMyMove;
});

hubConnection.on("AcceptChar", char => {
    myChar = char;
    if (myChar == "x")
        isMyMove = true;
});

hubConnection.on("OnDisconnect", () => {
    showResultModal("Game closed or connection lost");
});

hubConnection.on("OnGameOver", res => {
    disableField();
    if (res.isDraw === true)
        showResultModal("It' draw!");
    else
        showResultModal(`Winner is ${res.winner}!`);
});

$('#gameboard button').click(e => {
    e.preventDefault();
    let index = getElemId(e.target);
    hubConnection.invoke('AcceptMoveRequest', index);
});

hubConnection.start();
disableField();