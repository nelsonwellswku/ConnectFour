var GameStateService = function () {
    var currentBoard = [
        [0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0]
    ];

    var connectFourHubProxy = $.connection.connectFourHub;
    alert($.connection);
    connectFourHubProxy.client.updateBoard = function(board) {
        currentBoard = board;
    };

    $.connection.hub.start().done(function () { });

    return {
        board : currentBoard
    }
};