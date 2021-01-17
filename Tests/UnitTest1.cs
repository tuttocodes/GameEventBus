using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameEventBus;
using GameEventBus.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void TestEventOrder() 
        {
            EventBus bus = new EventBus();

            List<int> results = new List<int>();

            bus.Subscribe<EventBase>(e => {
                results.Add(0);
            });

            Action<CustomEvent1> action1 = (event1) => {
                results.Add(1);
            };
            bus.Subscribe<CustomEvent1>(action1);

            Action<CustomEvent2> action2 = (event1) => {
                results.Add(2);
            };
            bus.Subscribe<CustomEvent2>(action2);

            Action<CustomEvent3> action3 = (event1) => {
                results.Add(3);
            };
            bus.Subscribe<CustomEvent3>(action3);

            bus.Publish(new CustomEvent1());
            bus.Publish(new CustomEvent1());
            bus.Publish(new CustomEvent3());
            bus.Publish(new CustomEvent2());
            bus.Publish(new CustomEvent2());

            Assert.AreEqual(1, results[0]);
            Assert.AreEqual(1, results[1]);
            Assert.AreEqual(3, results[2]);
            Assert.AreEqual(2, results[3]);
            Assert.AreEqual(2, results[4]);

            bus.Unsubscribe(action1);
            bus.Publish(new CustomEvent1());
            bus.Publish(new CustomEvent1()); // these do not get added
            
            Assert.AreEqual(5, results.Count);

            bus.Unsubscribe(action3);
            bus.Publish(new CustomEvent2());
            Assert.AreEqual(2, results[5]);
        }

        [TestMethod]
        public void UnSubInActionDelegate()
        {
            EventBus bus = new EventBus();

            Action<CustomEvent1> action1 = (event1) => {
                Assert.IsTrue(false); // should reach here
            };
            bus.Subscribe<CustomEvent1>(action1);


            Action<CustomEvent2> action2 = (event1) => {
                bus.Unsubscribe(action1);
            };
            bus.Subscribe<CustomEvent2>(action2);

            bus.Publish(new CustomEvent2());
            bus.Publish(new CustomEvent1());

            Assert.IsTrue(true); // should make it here without invoking action1
        }

        [TestMethod]
        public void CsharpSanityTest()
        {
            EventBus bus = new EventBus();

            EventBase eventBase = new CustomEvent1();

            int i = 0;

            bus.Subscribe<EventBase>(e => {
                i = 1;
            });

            bus.Publish(eventBase);
            Assert.AreEqual(1, i);
        }
    }


    class CustomEvent1 : EventBase {

    }

    class CustomEvent2 : EventBase
    {
        
    }

    class CustomEvent3 : CustomEvent2
    {
        
    }

}
