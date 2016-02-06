using System;

namespace Octogami.ConnectFour.Application.Actor
{
	public class JoinGame : UserConnectionMessage
	{
		public JoinGame(string connectionId, string username) : base(connectionId)
		{
			Username = username;
		}

		public string Username { get; }
	}

	public class JoinGameById : JoinGame
	{
		public JoinGameById(string connectionId, string username, Guid gameId) : base(connectionId, username)
		{
			GameId = gameId;
		}

		public Guid GameId { get; }
	}

	public class JoinGameRejectionConnectionMessage : UserConnectionMessage
	{
		public string Username { get; set; }
		public Guid GameId { get; set; }
		public string Reason { get; set; }

		public JoinGameRejectionConnectionMessage(string connectionId, string username, Guid gameId, string reason) : base(connectionId)
		{
			Username = username;
			GameId = gameId;
			Reason = reason;
		}
	}

	public class JoinGameAcceptedConnectionMessage : UserConnectionMessage
	{
		public string Username { get; set; }
		public Guid GameId { get; set; }
		public string PlayerSlot { get; set; }

		public JoinGameAcceptedConnectionMessage(string connectionId, string username, Guid gameId, string playerSlot) : base(connectionId)
		{
			Username = username;
			GameId = gameId;
			PlayerSlot = playerSlot;
		}
	}
}