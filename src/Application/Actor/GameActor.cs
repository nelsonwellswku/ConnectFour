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

				Sender.Tell(new JoinGameAcceptedConnectionMessage(msg.ConnectionId, msg.Username, _gameId, playerSlot));

				Become(PendingGame);
			});
		}

		public void PendingGame()
		{
			Receive<JoinGameById>(msg =>
			{
				_playerTwo = msg.Username;
				var playerSlot = "PlayerTwo";

				Sender.Tell(new JoinGameAcceptedConnectionMessage(msg.ConnectionId, msg.Username, _gameId, playerSlot));

				Become(PlayerOneTurn);
			});
		}

		public void PlayerOneTurn()
		{
			Receive<JoinGameById>(msg => Sender.Tell(new JoinGameRejectionConnectionMessage(msg.ConnectionId, msg.Username, msg.GameId, "Game full")));

			Receive<MakeMove>(msg =>
			{
				if (msg.Username != _playerOne)
				{
					// TODO: Tell sender that that this isn't a valid move
					// Sender.Tell(...)

					return;
				}

				var column = GameBoardColumn.GetColumn(msg.Column);
				_gameBoard.DropPiece(BoardPiece.PlayerOne, column);

				if (_gameBoard.IsGameOver(BoardPiece.PlayerOne))
				{
					Sender.Tell(new GameStatusConnectionMessage(msg.ConnectionId, _gameBoard.Board, true, _playerOne));
					Sender.Tell(new GameOverConnectionMessage(msg.ConnectionId, _gameId));
				}
				else if (_gameBoard.IsDraw())
				{
					Sender.Tell(new GameStatusConnectionMessage(msg.ConnectionId, _gameBoard.Board, true, "Draw"));
					Sender.Tell(new GameOverConnectionMessage(msg.ConnectionId, _gameId));
				}
				else
				{
					Become(PlayerTwoTurn);
				}
			});
		}

		public void PlayerTwoTurn()
		{
			Receive<JoinGameById>(msg => Sender.Tell(new JoinGameRejectionConnectionMessage(msg.ConnectionId, msg.Username, msg.GameId, "Game full")));

			Receive<MakeMove>(msg =>
			{
				if(msg.Username != _playerTwo)
				{
					// TODO: Tell sender that that this isn't a valid move
					// Sender.Tell(...)

					return;
				}

				var column = GameBoardColumn.GetColumn(msg.Column);
				_gameBoard.DropPiece(BoardPiece.PlayerTwo, column);

				if(_gameBoard.IsGameOver(BoardPiece.PlayerTwo))
				{
					Sender.Tell(new GameStatusConnectionMessage(msg.ConnectionId, _gameBoard.Board, true, _playerTwo));
					Sender.Tell(new GameOverConnectionMessage(msg.ConnectionId, _gameId));
				}
				else if(_gameBoard.IsDraw())
				{
					Sender.Tell(new GameStatusConnectionMessage(msg.ConnectionId, _gameBoard.Board, true, "Draw"));
					Sender.Tell(new GameOverConnectionMessage(msg.ConnectionId, _gameId));
				}
				else
				{
					Become(PlayerOneTurn);
				}
			});
		}
	}
}