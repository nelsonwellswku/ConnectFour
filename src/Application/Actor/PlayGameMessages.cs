using System;
using System.Collections.Generic;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Actor
{
	public class MakeMove : UserConnectionMessage
	{
		public string Username { get; }
		public int Column { get; }

		public MakeMove(string connectionId, string username, int column) : base(connectionId)
		{
			Username = username;
			Column = column;
		}
	}

	public class GameStatusConnectionMessage : UserConnectionMessage
	{
		public IReadOnlyList<IReadOnlyList<BoardPiece>> Board { get; }
		public bool IsGameOver { get; }
		public string Winner { get; }

		public GameStatusConnectionMessage(string connectionId, IReadOnlyList<IReadOnlyList<BoardPiece>> board, bool isGameOver, string winner) : base(connectionId)
		{
			Board = board;
			IsGameOver = isGameOver;
			Winner = winner;
		}
	}

	public class GameOverConnectionMessage : UserConnectionMessage
	{
		public Guid GameId { get; }

		public GameOverConnectionMessage(string connectionId, Guid gameId) : base(connectionId)
		{
			GameId = gameId;
		}
	}
}