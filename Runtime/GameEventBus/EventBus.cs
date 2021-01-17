using System;
using System.Collections.Generic;
using System.Linq;
using TC.GameEventBus.Events;
using TC.GameEventBus.Interfaces;

namespace TC.GameEventBus
{
    /// <summary>
    /// Implements <see cref="IEventBus"/>.
    /// </summary>
    public class EventBus<EventBase> : IEventBus<EventBase>
    {   
        /// <summary>
        /// List of subscribed actions for any given event type.
        /// </summary>
        private readonly Dictionary<Type, object> subscriptions;

        private readonly Dictionary<object, Type> typeMap; 
        private static readonly object SubscriptionsLock = new object();

        public EventBus()
        {
            subscriptions = new Dictionary<Type, object>();
            typeMap = new Dictionary<object, Type>();
        }

        /// <summary>
        /// Subscribes to the specified event type with the specified action
        /// </summary>
        /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="action">The Action to invoke when an event of this type is published</param>
        /// <returns>A <see cref="SubscriptionToken"/> to be used when calling <see cref="Unsubscribe"/></returns>
        public void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            if (action == null)
                throw new ArgumentNullException("action");

            lock (SubscriptionsLock)
            {
                Type eventType = typeof(TEventBase);
                if (!subscriptions.ContainsKey(eventType))
                    subscriptions.Add(eventType, new List<Action<EventBase>>());

                List<Action<TEventBase>> typeSubscriptions = (List<Action<TEventBase>>) subscriptions[typeof(TEventBase)];
                typeSubscriptions.Add(action);
            }
        }

        /// <summary>
        /// Unsubscribe from the Event type related to the specified <see cref="SubscriptionToken"/>
        /// </summary>
        /// <param name="token">The <see cref="SubscriptionToken"/> received from calling the Subscribe method</param>
        public void Unsubscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            if (action == null)
                throw new ArgumentNullException("action");

            lock (SubscriptionsLock)
            {
                if (subscriptions.ContainsKey(typeof(TEventBase)))
                {   
                    List<Action<TEventBase>> typeSubscriptions = (List<Action<TEventBase>>) subscriptions[typeof(TEventBase)];
                    typeSubscriptions.Remove(action);
                }
            }
        }

        /// <summary>
        /// Publishes the specified event to any subscribers for the <see cref="TEventBase"/> event type
        /// </summary>
        /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="eventArgs">Event to publish</param>
        public void Publish<TEventBase>(TEventBase eventArgs) where TEventBase : EventBase
        {
            if (eventArgs == null)
                throw new ArgumentNullException("eventArgs");

            lock (SubscriptionsLock)
            {
                Type eventType = typeof(TEventBase);
                if (subscriptions.ContainsKey(eventType))
                {
                    List<Action<TEventBase>> typeSubscriptions = (List<Action<TEventBase>>) subscriptions[eventType];
                    foreach (var subscription in typeSubscriptions)
                    {
                        try
                        {
                            subscription(eventArgs);
                        }
                        catch (Exception exception)
                        {
                            // TODO: Log exception
                        }
                    }
                }
            }

            
        }
    }
}
