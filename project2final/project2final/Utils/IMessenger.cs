namespace project2final.Utils
{
    public interface IMessenger
    {
        void Send<TMessage>(TMessage message);
        void Subscribe<TMessage>(object subscriber, Action<object> action);
    }
}