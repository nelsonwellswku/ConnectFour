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

		public bool IsDraw()
		{
			return _board.SelectMany(x => x).All(x => x != BoardPiece.Empty);
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
		/// Return an ASCII art summary of the current board state
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
}