using System.Collections.Generic;
using System.Linq;

namespace Octogami.ConnectFour.Application.Game
{
	public class GameBoard
	{
		private readonly List<List<BoardPiece>> _board;
		/*
			_board[row][column]
			_board[0][0] - Top Left
			_board[0][6] - Top Right
			_board[5][0] - Bottom Left
			_board[5][6] - Bottom Right
		*/

		public GameBoard()
		{
			_board = new List<List<BoardPiece>>
			{
				Enumerable.Range(0, 7).Select(_ => BoardPiece.Empty).ToList(),
				Enumerable.Range(0, 7).Select(_ => BoardPiece.Empty).ToList(),
				Enumerable.Range(0, 7).Select(_ => BoardPiece.Empty).ToList(),
				Enumerable.Range(0, 7).Select(_ => BoardPiece.Empty).ToList(),
				Enumerable.Range(0, 7).Select(_ => BoardPiece.Empty).ToList(),
				Enumerable.Range(0, 7).Select(_ => BoardPiece.Empty).ToList()
			};
		}

		public IReadOnlyList<IReadOnlyList<BoardPiece>> Board => _board;
	}
}