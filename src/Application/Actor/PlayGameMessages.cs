using System;
using System.Collections.Generic;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Actor
{
	public class MakeMove : SignalRUserMessage
	{
		public string Username { get; }
		public int Column { get; }

		public MakeMove(string connectionId, string username, int column) : base(connectionId)
		{
			Username = username;
			Column = column;
		}
	}

	public class GameStatusMessage
	{
		public IReadOnlyList<IReadOnlyList<BoardPiece>> Board { get; }
		public bool IsGameOver { get; }
		public string Winner { get; }

		public GameStatusMessage(IReadOnlyList<IReadOnlyList<BoardPiece>> board, bool isGameOver, string winner)
		{
			Board = board;
			IsGameOver = isGameOver;
			Winner = winner;
		}
	}

	public class GameOverMessage
	{
		public Guid GameId { get; }

		public GameOverMessage(Guid gameId)
		{
			GameId = gameId;
		}
	}
}