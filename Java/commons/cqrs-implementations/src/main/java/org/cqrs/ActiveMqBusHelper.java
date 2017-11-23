package org.cqrs;

import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.command.ActiveMQQueue;
import org.cqrs101.utils.MainEnvironment;

import javax.inject.Inject;
import javax.inject.Named;
import javax.jms.*;
import javax.jms.Message;

@Named("activeMqBusHelper")
public class ActiveMqBusHelper {
    private ActiveMQConnectionFactory factory;
    private  Connection connection;
    private Session session;

    @Inject
    public ActiveMqBusHelper(MainEnvironment environment) throws JMSException {
        factory = new ActiveMQConnectionFactory(environment.getProperty("amq.brokerurl"));
        factory.setWatchTopicAdvisories(false);
        connection = factory.createConnection();
        connection.start();
        session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
    }

    public MessageConsumer createConsumer(String queueName) throws JMSException {
        ActiveMQQueue commandsQueue = new ActiveMQQueue(queueName);
        return session.createConsumer(commandsQueue);
    }

    public Destination createQueue(String s) throws JMSException {
        return session.createQueue(s);
    }

    public Destination createTopic(String s) throws JMSException {
        return session.createTopic(s);
    }

    public MessageProducer createProducer(Destination destination) throws JMSException {
        MessageProducer producer= session.createProducer(destination);
        producer.setDeliveryMode(DeliveryMode.PERSISTENT);
        return producer;
    }

    public TextMessage createTextMessage(String s) throws JMSException {
        return session.createTextMessage(s);
    }

    public void removeConsumer(MessageConsumer consumer) {
        try {
            consumer.setMessageListener(new MessageListener() {
                @Override
                public void onMessage(Message message) {

                }
            });
        } catch (JMSException e) {
            e.printStackTrace();
        }
    }
}
