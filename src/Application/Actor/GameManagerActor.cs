using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace Octogami.ConnectFour.Application.Actor
{
	public class GameManagerActor : ReceiveActor
	{
		private readonly Dictionary<Guid, IActorRef> _pendingGames;
		private readonly Dictionary<Guid, IActorRef> _activeGames;

		public GameManagerActor()
		{
			_pendingGames = new Dictionary<Guid, IActorRef>();
			_activeGames = new Dictionary<Guid, IActorRef>();

			Receive<JoinGame>(msg =>
			{
				var potentialGame = _pendingGames.FirstOrDefault();

				var gameAvailableToJoin = potentialGame.Key != default(Guid);

				Guid gameToJoin;

				if(!gameAvailableToJoin)
				{
					gameToJoin = Guid.NewGuid();
					var newGame = Context.ActorOf<GameActor>("Game-" + gameToJoin);
					_pendingGames.Add(gameToJoin, newGame);
				}
				else
				{
					gameToJoin = potentialGame.Key;
					_activeGames.Add(gameToJoin, _pendingGames[gameToJoin]);
					_pendingGames.Remove(gameToJoin);
					_activeGames[gameToJoin].Tell(new JoinGameById(msg.Username, gameToJoin));
				}
			});
		}
	}
}