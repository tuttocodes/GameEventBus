using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redbus;
using Redbus.Events;

namespace EventsTest {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            EventBus bus = new EventBus();

            bus.Subscribe<EventBase>(e => {
                Debug.Write("base event");
                Assert.IsTrue(false);
            });

            bus.Subscribe<CustomEvent1>(event1 => {
                Debug.WriteLine("custom event");
                Assert.IsTrue(true);
            });

            bus.Publish(new CustomEvent1());
            Debug.WriteLine("published custom event 1");
            Assert.IsTrue(true);

        }
    }

    class CustomEvent1 : EventBase {

    }

}
