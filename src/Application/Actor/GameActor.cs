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

			Receive<JoinGameById>(msg =>
			{
				// Assign the game id only when someone tries to join the game
				// because the game id is assigned by the game manager, not the game itself
				if(_gameId == default(Guid))
				{
					_gameId = msg.GameId;
				}

				// TODO: Read more about switchable behaviors; this seems like a better solution than checking if the game is full by hand
				// http://getakka.net/docs/working-with-actors/Switchable%20Behaviors
				if(_playerOne != null && _playerTwo != null)
				{
					// Game is full
					Sender.Tell(new JoinGameRejectionMessage(msg.Username, msg.GameId, "Game full"));
					return;
				}

				string playerSlot = null;
				if(_playerOne == null)
				{
					_playerOne = msg.Username;
					playerSlot = "PlayerOne";
				}
				else if(_playerTwo == null)
				{
					_playerTwo = msg.Username;
					playerSlot = "PlayerTwo";
				}

				Sender.Tell(new JoinGameAcceptedMessage(msg.Username, _gameId, playerSlot));
			});
		}
	}
}