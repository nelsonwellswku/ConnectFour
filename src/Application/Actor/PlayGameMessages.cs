using System.Collections.Generic;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Actor
{
	public class MakeMove
	{
		public MakeMove(string username, int column)
		{
			Username = username;
			Column = column;
		}
		public string Username;
		public int Column;
	}

	public class GameStatusMessage
	{
		public IReadOnlyList<IReadOnlyList<BoardPiece>> Board { get; set; }
		public bool IsGameOver { get; set; }
		public string Winner { get; set; }

		public GameStatusMessage(IReadOnlyList<IReadOnlyList<BoardPiece>> board, bool isGameOver, string winner)
		{
			Board = board;
			IsGameOver = isGameOver;
			Winner = winner;
		}
	}
}