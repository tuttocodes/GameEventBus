﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameEventBus.Events;
using GameEventBus.Interfaces;

namespace GameEventBus
{
    /// <summary>
    /// Implements <see cref="IEventBus"/>.
    /// </summary>
    public class EventBus<EventClass> : IEventBus<EventClass>
    {
        private readonly Dictionary<Type, List<ISubscription<EventClass>>> _subscriptions;
        private readonly Dictionary<object, Type> typeMap; 
        private static readonly object SubscriptionsLock = new object();

        public EventBus()
        {
            _subscriptions = new Dictionary<Type, List<ISubscription<EventClass>>>();
            typeMap = new Dictionary<object, Type>();
        }

        /// <summary>
        /// Subscribes to the specified event type with the specified action
        /// </summary>
        /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="action">The Action to invoke when an event of this type is published</param>
        /// <returns>A <see cref="SubscriptionToken"/> to be used when calling <see cref="Unsubscribe"/></returns>
        public void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventClass
        {
            if (action == null)
                throw new ArgumentNullException("action");

            lock (SubscriptionsLock)
            {
                if (!_subscriptions.ContainsKey(typeof(TEventBase)))
                    _subscriptions.Add(typeof(TEventBase), new List<ISubscription<EventClass>>());

                typeMap.Add(action, typeof(TEventBase));
                _subscriptions[typeof(TEventBase)].Add(new Subscription<TEventBase>(action));
            }
        }

        /// <summary>
        /// Unsubscribe from the Event type related to the specified <see cref="SubscriptionToken"/>
        /// </summary>
        /// <param name="token">The <see cref="SubscriptionToken"/> received from calling the Subscribe method</param>
        public void Unsubscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventClass
        {
            if (action == null)
                throw new ArgumentNullException("action");

            lock (SubscriptionsLock)
            {
                Type type = typeMap[action];
                if (_subscriptions.ContainsKey(type))
                {
                    var allSubscriptions = _subscriptions[type];
                    var subscriptionToRemove = allSubscriptions.FirstOrDefault(x => x.SubscriptionToken == action);
                    if (subscriptionToRemove != null)
                        _subscriptions[type].Remove(subscriptionToRemove);
                }
            }
        }

        /// <summary>
        /// Publishes the specified event to any subscribers for the <see cref="TEventBase"/> event type
        /// </summary>
        /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="eventItem">Event to publish</param>
        public void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventClass
        {
            if (eventItem == null)
                throw new ArgumentNullException("eventItem");

            List<ISubscription<EventClass>> allSubscriptions = new List<ISubscription<EventClass>>();
            lock (SubscriptionsLock)
            {
                if (_subscriptions.ContainsKey(typeof(TEventBase)))
                    allSubscriptions = _subscriptions[typeof(TEventBase)];
            }

            foreach (var subscription in allSubscriptions)
            {
                try
                {
                    subscription.Publish(eventItem);
                }
                catch (Exception exception)
                {

                }
            }
        }
    }
}
