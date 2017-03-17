using System;
using GameEventBus.Events;
using GameEventBus.Interfaces;

namespace GameEventBus
{
    internal class Subscription<TEventBase> : ISubscription<TEventBase> 
    {
        public object SubscriptionToken { get { return _action; } }

        public Subscription(Action<TEventBase> action)
        {
            if(action == null)
                throw new ArgumentNullException("action");

            _action = action;
//            _subscriptionToken = token;
        }


        public void Publish(TEventBase eventItem)
        {
            if (!(eventItem is TEventBase))
                throw new ArgumentException("Event Item is not the correct type.");

            _action.Invoke(eventItem);
        }

        private readonly Action<TEventBase> _action;
//        private readonly SubscriptionToken _subscriptionToken;
    }
}
