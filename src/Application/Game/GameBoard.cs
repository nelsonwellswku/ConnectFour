using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octogami.ConnectFour.Application.Game
{
	public class GameBoard
	{
		private const int MaxRowIndex = 5;
		private const int MaxColumnIndex = 6;

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

			for(var row = MaxRowIndex; row >= 0; row--)
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
			for (int row = MaxRowIndex; row >= 0; row--)
			{
				for(int col = 0; col <= MaxColumnIndex; col++)
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
			var standardCheck = false;
			if(row >= 3 && column <= 3)
			{
				standardCheck = CheckGameOverVertical(boardPiece, row, column) ||
				                CheckGameOverHorizontal(boardPiece, row, column) ||
				                CheckGameOverDiagonalRight(boardPiece, row, column);
			}

			var diagonalLeftCheck = CheckGameOverDiagonalLeft(boardPiece, row, column);

			return standardCheck || diagonalLeftCheck;
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

		private bool CheckGameOverDiagonalLeft(BoardPiece boardPiece, int row, int column)
		{
			// TODO: Do bounds checking instead of relying on the exception being thrown
			bool result;
			try
			{
				result = _board[row][column] == boardPiece &&
				         _board[row - 1][column - 1] == boardPiece &&
				         _board[row - 2][column - 2] == boardPiece &&
				         _board[row - 3][column - 3] == boardPiece;
			}
			catch(IndexOutOfRangeException)
			{
				return false;
			}

			return result;
		}

		private bool CheckGameOverDiagonalRight(BoardPiece boardPiece, int row, int column)
		{
			return _board[row][column] == boardPiece &&
				   _board[row - 1][column + 1] == boardPiece &&
				   _board[row - 2][column + 2] == boardPiece &&
				   _board[row - 3][column + 3] == boardPiece;
		}

		/// <summary>
		/// Display an ASCII art summery of the current board state
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach(var row in _board)
			{
				builder.AppendLine(string.Join(" ", row.Select(x => x == BoardPiece.Empty ? "_" : x == BoardPiece.PlayerOne ? "1" : x == BoardPiece.PlayerTwo ? "2" : "0")));
			}

			return builder.ToString();
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