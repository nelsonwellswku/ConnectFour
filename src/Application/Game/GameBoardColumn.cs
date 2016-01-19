using System;

namespace Octogami.ConnectFour.Application.Game
{
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

		public static GameBoardColumn GetColumn(int number)
		{
			GameBoardColumn column;
			switch(number)
			{
				case 0:
					column = Zero;
					break;
				case 1:
					column = One;
					break;
				case 2:
					column = Two;
					break;
				case 3:
					column = Three;
					break;
				case 4:
					column = Four;
					break;
				case 5:
					column = Five;
					break;
				case 6:
					column = Six;
					break;

				default:
					throw new ArgumentException("Column number not in range 0 - 6");
			}

			return column;
		}
	}
}