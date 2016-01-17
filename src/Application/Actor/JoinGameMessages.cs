using System;

namespace Octogami.ConnectFour.Application.Actor
{
	public class JoinGame
	{
		public JoinGame(string username)
		{
			Username = username;
		}

		public string Username { get; }
	}

	public class JoinGameById : JoinGame
	{
		public JoinGameById(string username, Guid gameId) : base(username)
		{
			GameId = gameId;
		}

		public Guid GameId { get; }
	}

	public class JoinGameRejectionMessage
	{
		public string Username { get; set; }
		public Guid GameId { get; set; }
		public string Reason { get; set; }

		public JoinGameRejectionMessage(string username, Guid gameId, string reason)
		{
			Username = username;
			GameId = gameId;
			Reason = reason;
		}
	}

	public class JoinGameAcceptedMessage
	{
		public string Username { get; set; }
		public Guid GameId { get; set; }
		public string PlayerSlot { get; set; }

		public JoinGameAcceptedMessage(string username, Guid gameId, string playerSlot)
		{
			Username = username;
			GameId = gameId;
			PlayerSlot = playerSlot;
		}
	}
}