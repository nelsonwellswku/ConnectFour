using Akka.TestKit.NUnit;
using NUnit.Framework;
using Octogami.ConnectFour.Application.Actor;

namespace Octogami.ConnectFour.Application.Tests.Actor
{
	public class GameManagerActorTests : TestKit
	{
		// TODO: Not actually sure how to test that a child actor is created and a message is sent to it
		// without changing the code of the child actor to send back an acknowledgment
		[Test]
		public void Manager_creates_new_game_when_a_player_joins_with_no_pending_games()
		{
			var actor = ActorOfAsTestActorRef<GameManagerActor>();

			actor.Tell(new JoinGame("1", "John"));
			ExpectMsg<JoinGameAcceptedMessage>();
		}
	}
}