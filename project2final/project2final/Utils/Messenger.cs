using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project2final.Utils
{
    public class Messenger : IMessenger
    {
        private readonly Dictionary<Type, List<SubscriberActionPair>> _subscribers;

        public Messenger()
        {
            _subscribers = new Dictionary<Type, List<SubscriberActionPair>>();
        }

        public void Send<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);

            if (!_subscribers.ContainsKey(messageType))
                return;

            var subscriberActionPairs = _subscribers[messageType];
            foreach (var subscriberActionPair in subscriberActionPairs)
            {
                subscriberActionPair.Action(message);
            }
        }

        public void Subscribe<TMessage>(object subscriber, Action<object> action)
        {
            var messageType = typeof(TMessage);

            SubscriberActionPair subscriberActionPair = new SubscriberActionPair(subscriber, action);

            if (_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType].Add(subscriberActionPair);
            }
            else
            {
                _subscribers[messageType] = new List<SubscriberActionPair> { subscriberActionPair };
            }
        }

        private class SubscriberActionPair
        {
            public object Subscriber { get; }
            public Action<object> Action { get; }

            public SubscriberActionPair(object subscriber, Action<object> action)
            {
                Subscriber = subscriber;
                Action = action;
            }
        }
    }
}
