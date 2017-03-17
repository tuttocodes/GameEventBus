using System;
using GameEventBus.Events;

namespace GameEventBus.Interfaces
{
    public interface ISubscription<EventClass>
    {
        /// <summary>
        /// Token returned to the subscriber
        /// </summary>
        object SubscriptionToken { get; }

        /// <summary>
        /// Publish to the subscriber
        /// </summary>
        /// <param name="eventBase"></param>
        void Publish(EventClass eventBase);
    }
}
