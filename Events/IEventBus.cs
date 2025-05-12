public interface IEventBus
{
	void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : EventArgs;
	void Publish<TEvent>(object sender, TEvent @event) where TEvent : EventArgs;
}

