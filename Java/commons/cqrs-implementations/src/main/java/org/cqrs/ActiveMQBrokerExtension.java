package org.cqrs;

import org.apache.activemq.broker.Broker;
import org.apache.activemq.broker.ConnectionContext;
import org.apache.activemq.broker.region.Destination;
import org.apache.activemq.broker.region.Queue;
import org.apache.activemq.broker.region.Subscription;
import org.apache.activemq.broker.region.Topic;
import org.apache.activemq.broker.region.virtual.MappedQueueFilter;
import org.apache.activemq.broker.region.virtual.VirtualTopicInterceptor;
import org.apache.activemq.command.ActiveMQDestination;
import org.apache.activemq.store.MessageStore;

import java.io.IOException;
import java.util.List;
import java.util.Map;

public class ActiveMQBrokerExtension {
    private final Broker broker;

    public ActiveMQBrokerExtension(Broker broker) {
        this.broker = broker;
    }

    public void clearAllMessages() throws Exception {
        Map<ActiveMQDestination, Destination> destinationMap
                = broker.getDestinationMap();
        for (Destination destination : destinationMap.values()) {
            ActiveMQDestination activeMQDestination
                    = destination.getActiveMQDestination();
            if (activeMQDestination.isTopic()) {
                if(destination.getClass() == Topic.class) {
                    clearAllMessages((Topic) destination);
                }else{
                    clearAllMessages((VirtualTopicInterceptor)destination);
                }
            } else if (activeMQDestination.isQueue()) {
                if(destination.getClass() == Queue.class) {
                    clearAllMessages((Queue) destination);
                }else{
                    clearAllMessages((MappedQueueFilter)destination);
                }
            }
        }
    }

    private void clearAllMessages(VirtualTopicInterceptor topic) throws IOException {
        List<Subscription> consumers = topic.getConsumers();
        for (Subscription consumer : consumers) {
            ConnectionContext consumerContext = consumer.getContext();
            MessageStore messageStore = topic.getMessageStore();
            messageStore.removeAllMessages(consumerContext);
        }
    }

    private void clearAllMessages(MappedQueueFilter mqf) throws IOException {

        mqf.clearPendingMessages();
        List<Subscription> consumers = mqf.getConsumers();
        for (Subscription consumer : consumers) {
            ConnectionContext consumerContext = consumer.getContext();
            MessageStore messageStore = mqf.getMessageStore();
            messageStore.removeAllMessages(consumerContext);
        }
    }

    private void clearAllMessages(Topic topic) throws IOException {
        List<Subscription> consumers = topic.getConsumers();
        for (Subscription consumer : consumers) {
            ConnectionContext consumerContext = consumer.getContext();
            MessageStore messageStore = topic.getMessageStore();
            messageStore.removeAllMessages(consumerContext);
        }
    }
    private void clearAllMessages(Queue queue) throws Exception {
        queue.purge();
    }
}
