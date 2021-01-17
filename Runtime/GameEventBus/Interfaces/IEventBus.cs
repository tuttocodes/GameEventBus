using System;
using TC.GameEventBus.Events;

namespace TC.GameEventBus.Interfaces
{

    public interface IEventBus<EventBase>
    {
        /// <summary>
        /// Subscribes to the specified event type with the specified action
        /// </summary>
        /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="action">The Action to invoke when an event of this type is published</param>
        void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;

        /// <summary>
        /// Unsubscribe the action from the event type/>
        /// </summary>
        /// /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="action">The action we want to unsubscribe.</param>
        void Unsubscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;

        /// <summary>
        /// Publishes the specified event to any subscribers for the <see cref="TEventBase"/> event type
        /// </summary>
        /// <typeparam name="TEventBase">The type of event</typeparam>
        /// <param name="eventArgs">Event to publish</param>
        void Publish<TEventBase>(TEventBase eventArgs) where TEventBase : EventBase;

    }
}
