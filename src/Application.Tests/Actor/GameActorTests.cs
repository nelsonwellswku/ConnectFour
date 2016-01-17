using System;
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
			var actor = ActorOfAsTestActorRef<GameActor>();
			actor.Tell(new JoinGameById("John", gameId));
			ExpectMsg(new JoinGameAcceptedMessage("John", gameId, "PlayerOne"));
		}

		[Test]
		public void Player_two_can_join_existing_game()
		{
			var gameId = Guid.NewGuid();
			var actor = ActorOfAsTestActorRef<GameActor>();
			actor.Tell(new JoinGameById("John", gameId));
			actor.Tell(new JoinGameById("Jane", gameId));
			ExpectMsg(new JoinGameAcceptedMessage("Jane", gameId, "PlayerTwo"));
		}

		[Test]
		public void Player_three_gets_a_game_full_rejection_message()
		{
			var gameId = Guid.NewGuid();
			var actor = ActorOfAsTestActorRef<GameActor>();
			actor.Tell(new JoinGameById("John", gameId));
			actor.Tell(new JoinGameById("Jane", gameId));
			actor.Tell(new JoinGameById("Rudolph", gameId));
			ExpectMsg(new JoinGameRejectionMessage("Rudolph", gameId, "Game full"));
		}
	}
}