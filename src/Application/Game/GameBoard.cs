using System;
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

		public bool DropPiece(BoardPiece piece, GameBoardColumn column)
		{
			if(piece == BoardPiece.Empty)
			{
				throw new ArgumentException("Can not drop an empty piece into the game board");
			}

			var columnNumber = column.Column;

			for(var row = 5; row >= 0; row--)
			{
				if(_board[row][columnNumber] == BoardPiece.Empty)
				{
					_board[row][columnNumber] = piece;
					return true;
				}
			}

			return false;
		}

		public bool IsGameOver(BoardPiece boardPiece)
		{
			// Only check from rows 5 to 3 so that private functions that go up rows don't have to do bounds checking
			for (int row = 5; row >= 3; row--)
			{
				// Likewise, only columns 0 to 3 so that checking up column indices doesn't have to do bounds checking
				for(int col = 0; col <= 3; col++)
				{
					if(CheckGameOver(boardPiece, row, col))
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool CheckGameOver(BoardPiece boardPiece, int row, int column)
		{
			return CheckGameOverVertical(boardPiece, row, column) || CheckGameOverHorizontal(boardPiece, row, column);
		}

		private bool CheckGameOverVertical(BoardPiece boardPiece, int row, int column)
		{
			return _board[row][column] == boardPiece &&
			       _board[row - 1][column] == boardPiece &&
			       _board[row - 2][column] == boardPiece &&
			       _board[row - 3][column] == boardPiece;
		}

		private bool CheckGameOverHorizontal(BoardPiece boardPiece, int row, int column)
		{
			return _board[row][column] == boardPiece &&
			       _board[row][column + 1] == boardPiece &&
			       _board[row][column + 2] == boardPiece &&
			       _board[row][column + 3] == boardPiece;
		}
	}

	public struct GameBoardColumn
	{
		private GameBoardColumn(int column)
		{
			Column = column;
		}

		public int Column { get; }

		public static GameBoardColumn Zero => new GameBoardColumn(0);
		public static GameBoardColumn One => new GameBoardColumn(1);
		public static GameBoardColumn Two => new GameBoardColumn(2);
		public static GameBoardColumn Three => new GameBoardColumn(3);
		public static GameBoardColumn Four => new GameBoardColumn(4);
		public static GameBoardColumn Five => new GameBoardColumn(5);
		public static GameBoardColumn Six => new GameBoardColumn(6);
	}
}