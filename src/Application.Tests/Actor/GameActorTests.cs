using System;
using Akka.Actor;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using Octogami.ConnectFour.Application.Actor;

namespace Octogami.ConnectFour.Application.Tests.Actor
{
	public class GameActorTests : TestKit
	{
		[Test]
		public void Player_one_can_join_new_game()
		{
			var gameId = Guid.NewGuid();
			var actor = Sys.ActorOf(Props.Create(() => new GameActor()));
			actor.Tell(new JoinGameById("1", "John", gameId));
			ExpectMsg<JoinGameAcceptedConnectionMessage>(msg =>
				msg.Username == "John" &&
				msg.GameId == gameId &&
				msg.PlayerSlot == "PlayerOne"
				&& msg.ConnectionId == "1");
		}

		[Test]
		public void Player_two_can_join_existing_game()
		{
			var gameId = Guid.NewGuid();
			var actor = Sys.ActorOf(Props.Create(() => new GameActor()));
			actor.Tell(new JoinGameById("1", "John", gameId));
			ExpectMsg<JoinGameAcceptedConnectionMessage>();
			actor.Tell(new JoinGameById("2", "Jane", gameId));
			ExpectMsg<JoinGameAcceptedConnectionMessage>(msg => msg.Username == "Jane" && msg.GameId == gameId && msg.PlayerSlot == "PlayerTwo");
		}

		[Test]
		public void Player_three_gets_a_game_full_rejection_message()
		{
			var gameId = Guid.NewGuid();
			var actor = Sys.ActorOf(Props.Create(() => new GameActor()));
			actor.Tell(new JoinGameById("1", "John", gameId));
			ExpectMsg<JoinGameAcceptedConnectionMessage>();

			actor.Tell(new JoinGameById("2", "Jane", gameId));
			ExpectMsg<JoinGameAcceptedConnectionMessage>();

			actor.Tell(new JoinGameById("3", "Rudolph", gameId));
			ExpectMsg<JoinGameRejectionConnectionMessage>(msg => msg.Username == "Rudolph" && msg.GameId == gameId && msg.Reason == "Game full");
		}
	}
}