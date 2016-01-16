using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Tests.Game
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
			_gameBoard.Board.Count.Should().Be(6, "because the board should have 6 rows");
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
			Action act = () => _gameBoard.DropPiece(BoardPiece.Empty, GameBoardColumn.Zero);
			act.ShouldThrowExactly<ArgumentException>().WithMessage("Can not drop an empty piece into the game board");
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

				var result = _gameBoard.DropPiece(pieceToDrop, column);

				result.Should().BeTrue();
				_gameBoard.Board[i][columnNumber].Should().Be(pieceToDrop);
			}
		}

		[Test]
		public void Can_not_drop_more_pieces_in_a_column_than_the_board_can_accomodate()
		{
			// Arrange
			for(int i = 0; i <= 6; i++)
			{
				_gameBoard.DropPiece(BoardPiece.PlayerOne, GameBoardColumn.Zero);
			}

			// Act
			var result = _gameBoard.DropPiece(BoardPiece.PlayerTwo, GameBoardColumn.Zero);

			// Assert
			result.Should().BeFalse();
			_gameBoard.Board[0][0].Should().Be(BoardPiece.PlayerOne);

		}

		[Test]
		public void Board_knows_game_over_vertical()
		{
			// Arrange
			for(var i = 0; i < 4; i++)
			{
				_gameBoard.DropPiece(BoardPiece.PlayerOne, GameBoardColumn.Three);
			}

			// Act
			var result = _gameBoard.IsGameOver(BoardPiece.PlayerOne);

			// Assert
			result.Should().BeTrue();
		}

		[Test]
		public void Board_knows_game_over_horizontal()
		{
			// Arrange
			var _ = new[] {GameBoardColumn.One, GameBoardColumn.Two, GameBoardColumn.Three, GameBoardColumn.Four}
				.Select(col => _gameBoard.DropPiece(BoardPiece.PlayerOne, col))
				.ToList();

			// Act
			var result = _gameBoard.IsGameOver(BoardPiece.PlayerOne);

			// Assert
			result.Should().BeTrue();
		}

		[Test]
		public void Board_knows_game_over_diagonal_right()
		{
			// Arrange
			var _ = new[]
			{
				new {gbc = GameBoardColumn.One, bp = BoardPiece.PlayerOne},
				new {gbc = GameBoardColumn.Two, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Two, bp = BoardPiece.PlayerOne},
				new {gbc = GameBoardColumn.Three, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Three, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Three, bp = BoardPiece.PlayerOne},
				new {gbc = GameBoardColumn.Four, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Four, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Four, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Four, bp = BoardPiece.PlayerOne}
			}.Select(x => _gameBoard.DropPiece(x.bp, x.gbc)).ToList();

			// Act
			var result = _gameBoard.IsGameOver(BoardPiece.PlayerOne);

			// Assert
			result.Should().BeTrue();
		}

		[Test]
		public void Board_knows_game_over_diagonal_left()
		{
			// Arrange
			var _ = new[]
			{
				new {gbc = GameBoardColumn.One, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.One, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.One, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.One, bp = BoardPiece.PlayerOne},
				new {gbc = GameBoardColumn.Two, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Two, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Two, bp = BoardPiece.PlayerOne},
				new {gbc = GameBoardColumn.Three, bp = BoardPiece.PlayerTwo},
				new {gbc = GameBoardColumn.Three, bp = BoardPiece.PlayerOne},
				new {gbc = GameBoardColumn.Four, bp = BoardPiece.PlayerOne}
			}.Select(x => _gameBoard.DropPiece(x.bp, x.gbc)).ToList();

			// Act
			var result = _gameBoard.IsGameOver(BoardPiece.PlayerOne);

			// Assert
			result.Should().BeTrue();
		}

		[Test]
		public void Board_game_over_no_false_positives()
		{
			// Act
			var playerOneResult = _gameBoard.IsGameOver(BoardPiece.PlayerOne);
			var playerTwoResult = _gameBoard.IsGameOver(BoardPiece.PlayerTwo);

			// Assert
			playerOneResult.Should().BeFalse();
			playerTwoResult.Should().BeFalse();
		}
	}
}