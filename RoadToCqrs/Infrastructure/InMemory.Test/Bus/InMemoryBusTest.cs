using System;
using System.Collections.Generic;
using Bus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
// ReSharper disable SuggestBaseTypeForParameter

// ReSharper disable RedundantLambdaSignatureParentheses
namespace InMemory.Bus
{
    [TestClass]
    public class InMemoryBusTest
    {
        private InMemoryBus _target;
        private List<object> _topics;
        private List<object> _queues;
        private List<object> _listeners;

        private static string SerializeObject(object message)
        {
            return JsonConvert.SerializeObject(message);
        }

        private static void VerifyMessageInFirstListOnly(object message, List<object> first, List<object> second)
        {
            Assert.AreEqual(1, first.Count);
            Assert.AreEqual(0, second.Count);

            var received = first[0];
            Assert.AreEqual(SerializeObject(message), SerializeObject(received));
            Assert.AreNotSame(message, received);
        }

        private static void VerifyMessagesInDifferentQueues(object message, List<object> first, List<object> second)
        {
            Assert.AreEqual(1, first.Count);
            Assert.AreEqual(1, second.Count);

            var received1 = second[0];
            Assert.AreEqual(SerializeObject(message), SerializeObject(received1));
            Assert.AreNotSame(message, received1);

            var received2 = first[0];
            Assert.AreEqual(SerializeObject(message), SerializeObject(received2));
            Assert.AreNotSame(message, received2);

            Assert.AreEqual(SerializeObject(received1), SerializeObject(received2));
            Assert.AreNotSame(received1, received2);
        }

        [TestInitialize]
        public void Setup()
        {
            _target = new InMemoryBus();
            _topics = new List<object>();
            _queues = new List<object>();
            _listeners = new List<object>();
        }

        [TestMethod]
        public void ShouldNotRegisterForQueueAndTopic()
        {
            //Given
            _target.RegisterQueue<SecondMessage>(_queues.Add);

            //When Then
            Assert.ThrowsException<HandlerAlreadyUsedException>(() => 
                _target.RegisterTopic<SecondMessage>(_topics.Add));
        }

        [TestMethod]
        public void ShouldNotRegisterForTopicAndQueue()
        {
            //Given
            _target.RegisterTopic<SecondMessage>(_topics.Add);

            //When Then
            Assert.ThrowsException<HandlerAlreadyUsedException>(() => 
                _target.RegisterQueue<SecondMessage>(_topics.Add));
        }

        [TestMethod]
        public void ShouldNotRegisterTwiceForQueue()
        {
            //Given
            _target.RegisterQueue<SecondMessage>(_queues.Add);

            //When Then
            Assert.ThrowsException<HandlerAlreadyUsedException>(() => 
                _target.RegisterQueue<SecondMessage>(_queues.Add));
        }

        [TestMethod]
        public void ShouldBePossibleToHandleQueues()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterQueue<SecondMessage>(_queues.Add);

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyMessageInFirstListOnly(message, _queues, _topics);
        }

        [TestMethod]
        public void ShouldBePossibleToHandleTopics()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterTopic<SecondMessage>(_topics.Add);

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyMessageInFirstListOnly(message, _topics, _queues);
        }

        private void VerifyNobodyReceivedData()
        {
            Assert.AreEqual(0, _queues.Count);
            Assert.AreEqual(0, _topics.Count);
            Assert.AreEqual(0, _listeners.Count);
        }

        [TestMethod]
        public void ShouldBePossibleToHandleMultipleTopicSubscribers()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterTopic<SecondMessage>(_topics.Add);
            _target.RegisterTopic<SecondMessage>(_topics.Add);

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            Assert.AreEqual(0, _queues.Count);
            Assert.AreEqual(2, _topics.Count);

            var received1 = _topics[0];
            Assert.AreEqual(SerializeObject(message), SerializeObject(received1));
            Assert.AreNotSame(message, received1);

            var received2 = _topics[1];
            Assert.AreEqual(SerializeObject(message), SerializeObject(received2));
            Assert.AreNotSame(message, received2);

            Assert.AreEqual(SerializeObject(received1), SerializeObject(received2));
            Assert.AreNotSame(received1, received2);
        }

        [TestMethod]
        public void ShouldListenToTopics()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterTopic<SecondMessage>(_topics.Add);
            _target.AddListener(_listeners.Add);

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyMessagesInDifferentQueues(message, _topics, _listeners);
        }

        [TestMethod]
        public void ShouldListenToQueue()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterQueue<SecondMessage>(_queues.Add);
            _target.AddListener(_listeners.Add);

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyMessagesInDifferentQueues(message, _queues, _listeners);
        }

        [TestMethod]
        public void ShouldHandleUnregisteredMessagesOnlyOnListener()
        {
            //Given
            var message = new SecondMessage();
            _target.AddListener(_listeners.Add);

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            Assert.AreEqual(0, _queues.Count);
            Assert.AreEqual(0, _topics.Count);
            Assert.AreEqual(1, _listeners.Count);

            var received = _listeners[0];
            Assert.AreEqual(SerializeObject(message), SerializeObject(received));
            Assert.AreNotSame(message, received);
        }

        [TestMethod]
        public void ShouldBePossibleToHandleTopicsNotThrowingException()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterTopic<SecondMessage>((a) => throw new Exception());

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyNobodyReceivedData();
        }

        [TestMethod]
        public void ShouldBePossibleToHandleQueuesNotThrowingException()
        {
            //Given
            var message = new SecondMessage();
            _target.RegisterQueue<SecondMessage>((a) => throw new Exception());

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyNobodyReceivedData();
        }

        [TestMethod]
        public void ShouldBePossibleToHandleListenersNotThrowingException()
        {
            //Given
            var message = new SecondMessage();
            _target.AddListener((a) => throw new Exception());

            //When
            _target.Send(message);
            _target.ForceRun();

            //Then
            VerifyNobodyReceivedData();
        }
    }
}