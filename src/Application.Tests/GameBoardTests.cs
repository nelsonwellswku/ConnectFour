using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Octogami.ConnectFour.Application.Game;

namespace Octogami.ConnectFour.Application.Tests
{
	public class GameBoardTests
	{
		[Test]
		public void Construct_an_empty_board()
		{
			// Arrange

			// Act
			var gameBoard = new GameBoard();

			// Assert
			gameBoard.Board.Count.Should().Be(6, "becase the board should have 6 rows");
			foreach(var row in gameBoard.Board)
			{
				row.Count.Should().Be(7, "because each row should have 7 columns");
			}
			gameBoard.Board.SelectMany(x => x).Distinct().Single().Should().Be(BoardPiece.Empty, "because the board should start out empty");

		}
	}
}