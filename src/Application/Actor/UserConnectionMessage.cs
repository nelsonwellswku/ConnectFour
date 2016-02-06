namespace Octogami.ConnectFour.Application.Actor
{
	public abstract class UserConnectionMessage
	{
		public string ConnectionId { get; }

		protected UserConnectionMessage(string connectionId)
		{
			ConnectionId = connectionId;
		}
	}
}