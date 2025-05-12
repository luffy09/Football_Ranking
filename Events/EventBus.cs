
public class EventBus : IEventBus
{
	private readonly Dictionary<Type, List<Delegate>> _handlers = new();

	public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : EventArgs
	{
		var type = typeof(TEvent);
		if (!_handlers.ContainsKey(type))
			_handlers[type] = new List<Delegate>();

		_handlers[type].Add(handler);
	}

	public void Publish<TEvent>(object sender, TEvent @event) where TEvent : EventArgs
	{
		var type = typeof(TEvent);
		if (_handlers.TryGetValue(type, out var delegates))
		{
			foreach (var handler in delegates.Cast<Action<TEvent>>())
			{
				handler.Invoke(@event);
			}
		}
	}
}