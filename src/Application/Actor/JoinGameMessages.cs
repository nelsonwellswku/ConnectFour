using System;

namespace Octogami.ConnectFour.Application.Actor
{
	public abstract class SignalRUserMessage
	{
		public string ConnectionId { get; }

		protected SignalRUserMessage(string connectionId)
		{
			ConnectionId = connectionId;
		}
	}

	public class JoinGame : SignalRUserMessage
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

	public class JoinGameRejectionMessage : SignalRUserMessage
	{
		public string Username { get; set; }
		public Guid GameId { get; set; }
		public string Reason { get; set; }

		public JoinGameRejectionMessage(string connectionId, string username, Guid gameId, string reason) : base(connectionId)
		{
			Username = username;
			GameId = gameId;
			Reason = reason;
		}
	}

	public class JoinGameAcceptedMessage : SignalRUserMessage
	{
		public string Username { get; set; }
		public Guid GameId { get; set; }
		public string PlayerSlot { get; set; }

		public JoinGameAcceptedMessage(string connectionId, string username, Guid gameId, string playerSlot) : base(connectionId)
		{
			Username = username;
			GameId = gameId;
			PlayerSlot = playerSlot;
		}
	}
}