using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Tests
{
	public class GameBoardTests
	{
		private GameBoard _gameBoard;

		[SetUp]
		public void SetUp()
		{
			_gameBoard = new GameBoard();
		}

		[Test]
		public void Construct_an_empty_board()
		{
			// Assert
			_gameBoard.Board.Count.Should().Be(6, "becase the board should have 6 rows");
			foreach(var row in _gameBoard.Board)
			{
				row.Count.Should().Be(7, "because each row should have 7 columns");
			}
			_gameBoard.Board.SelectMany(x => x).Distinct().Single().Should().Be(BoardPiece.Empty, "because the board should start out empty");
		}

		[Test]
		public void Can_not_drop_an_empty_piece()
		{
			// Act - Assert
			Action act = () => _gameBoard.TryDropPiece(BoardPiece.Empty, GameBoardColumn.Zero);
			act.ShouldThrowExactly<InvalidOperationException>().WithMessage("Can not drop an empty piece into the game board");
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		public void Can_drop_pieces_in_columns(int columnNumber)
		{
			var column =
				columnNumber == 1 ? GameBoardColumn.One :
				columnNumber == 2 ? GameBoardColumn.Two :
				columnNumber == 3 ? GameBoardColumn.Three :
				columnNumber == 4 ? GameBoardColumn.Four :
				columnNumber == 5 ? GameBoardColumn.Five :
				columnNumber == 6 ? GameBoardColumn.Six : 
					GameBoardColumn.Zero;

			for(var i = 5; i >= 0; i--)
			{
				var pieceToDrop = i % 2 == 0 ? BoardPiece.PlayerOne : BoardPiece.PlayerTwo;

				var result = _gameBoard.TryDropPiece(pieceToDrop, column);

				result.Should().BeTrue();
				_gameBoard.Board[columnNumber][i].Should().Be(pieceToDrop);
			}
		}

		[Test]
		public void Can_not_drop_more_pieces_in_a_column_than_the_board_can_accomodate()
		{
			// Arrange
			for(int i = 0; i <= 6; i++)
			{
				_gameBoard.TryDropPiece(BoardPiece.PlayerOne, GameBoardColumn.Zero);
			}

			// Act
			var result = _gameBoard.TryDropPiece(BoardPiece.PlayerTwo, GameBoardColumn.Zero);

			// Assert
			result.Should().BeFalse();
			_gameBoard.Board[0].Select(x => x).Distinct().Single().Should().Be(BoardPiece.PlayerOne);

		}
	}
}