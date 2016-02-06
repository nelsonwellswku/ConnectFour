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

				var gameToJoin = Guid.NewGuid();
				var dictToSendMessageTo = _pendingGames;

				if(!gameAvailableToJoin)
				{
					var newGame = Context.ActorOf<GameActor>("Game-" + gameToJoin);
					_pendingGames.Add(gameToJoin, newGame);
				}
				else
				{
					dictToSendMessageTo =  _activeGames;
					gameToJoin = potentialGame.Key;
					_activeGames.Add(gameToJoin, _pendingGames[gameToJoin]);
					_pendingGames.Remove(gameToJoin);
				}

				dictToSendMessageTo[gameToJoin].Tell(new JoinGameById(msg.ConnectionId, msg.Username, gameToJoin), Sender);
			});

			Receive<GameOverConnectionMessage>(msg =>
			{
				EndGame(_pendingGames, msg.GameId);
				EndGame(_activeGames, msg.GameId);
			});
		}

		private static void EndGame(IDictionary<Guid, IActorRef> gameDict, Guid id)
		{
			if(gameDict.ContainsKey(id))
			{
				var actor = gameDict[id];
				gameDict.Remove(id);
				Context.Stop(actor);
			}
		}
	}
}