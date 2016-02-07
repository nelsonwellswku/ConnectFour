angular.module("ConnectFour", [])

    .factory("GameStateService", GameStateService)

    .controller("GameController", ["GameStateService", GameController]);