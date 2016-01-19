using System;
using Akka.Actor;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Actor
{
	public class GameActor : ReceiveActor
	{
		private Guid _gameId;
		private readonly GameBoard _gameBoard;
		private string _playerOne;
		private string _playerTwo;

		public GameActor()
		{
			_gameBoard = new GameBoard();

			Become(NewGame);
		}

		public void NewGame()
		{
			Receive<JoinGameById>(msg =>
			{
				// Assign, not generate, the game id only when a new game is created
				// because the game id is assigned by the game manager, not the game itself
				_gameId = msg.GameId;

				_playerOne = msg.Username;
				var playerSlot = "PlayerOne";

				Sender.Tell(new JoinGameAcceptedMessage(msg.Username, _gameId, playerSlot));

				Become(PendingGame);
			});
		}

		public void PendingGame()
		{
			Receive<JoinGameById>(msg =>
			{
				_playerTwo = msg.Username;
				var playerSlot = "PlayerTwo";

				Sender.Tell(new JoinGameAcceptedMessage(msg.Username, _gameId, playerSlot));

				Become(FullGame);
			});
		}

		public void FullGame()
		{
			Receive<JoinGameById>(msg => Sender.Tell(new JoinGameRejectionMessage(msg.Username, msg.GameId, "Game full")));

			Receive<MakeMove>(msg =>
			{
				var isPlayerOne = msg.Username == _playerOne;

				var column = GameBoardColumn.Zero;
				switch(msg.Column)
				{
					case 0: column = GameBoardColumn.Zero;	break;
					case 1: column = GameBoardColumn.One;	break;
					case 2: column = GameBoardColumn.Two;	break;
					case 3: column = GameBoardColumn.Three;	break;
					case 4: column = GameBoardColumn.Four;	break;
					case 5: column = GameBoardColumn.Five;	break;

					// TODO: Send a failure message back to the sender
					default: break;
				}

				_gameBoard.DropPiece(isPlayerOne ? BoardPiece.PlayerOne : BoardPiece.PlayerTwo, column);

				var playerOneWins = _gameBoard.IsGameOver(BoardPiece.PlayerOne);
				var playerTwoWins = _gameBoard.IsGameOver(BoardPiece.PlayerTwo);

				string winner = null;
				if(playerOneWins)
				{
					winner = _playerOne;
				}
				else if(playerTwoWins)
				{
					winner = _playerTwo;
				}

				// TODO: Need to check if the game is a draw and include that in the message. This should be a method in the GameBoard class.

				Sender.Tell(new GameStatusMessage(_gameBoard.Board, playerOneWins || playerTwoWins, winner));
			});
		}
	}
}