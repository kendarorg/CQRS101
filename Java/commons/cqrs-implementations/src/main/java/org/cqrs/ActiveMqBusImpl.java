package org.cqrs;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Consumer;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.jms.Connection;
import javax.jms.DeliveryMode;
import javax.jms.Destination;
import javax.jms.JMSException;
import javax.jms.MessageConsumer;
import javax.jms.MessageProducer;
import javax.jms.Session;
import javax.jms.TextMessage;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.command.ActiveMQQueue;

public class ActiveMqBusImpl implements Bus {

    private static final Logger logger = Logger.getLogger(ActiveMqBusImpl.class.getSimpleName());
    public static final String brokerURL = "failover:tcp://localhost:61616";
    private ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory(brokerURL);
    private final Connection connection;
    private final String instanceName;
    private ObjectMapper mapper = new ObjectMapper();
    private Session session;

    @Inject
    public ActiveMqBusImpl(List<MessageHandler> messageHandlers, String instanceName) throws Exception {
        try {
            this.instanceName = instanceName;
            factory.setWatchTopicAdvisories(false);
            connection = factory.createConnection();
            connection.start();
            session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);

            for (int i = 0; i < messageHandlers.size(); i++) {
                MessageHandler messageHandler = messageHandlers.get(i);
                messageHandler.register(this);
            }
        } catch (JMSException ex) {
            logger.log(Level.SEVERE, "Error initializing ActiveMQ and Handlers;", ex);
            throw new Exception(ex);
        }
    }

    private final ConcurrentHashMap<String, Class> messageTypes = new ConcurrentHashMap<>();

    @Override
    public void registerHandler(Consumer<Object> handlerFunction, Class messageType, Class callerType) {

        boolean isEvent = Event.class.isAssignableFrom(messageType);
        boolean isCommand = Command.class.isAssignableFrom(messageType);
        String messageTypeName = messageType.getSimpleName().toUpperCase(Locale.ROOT);
        messageTypes.putIfAbsent(messageTypeName, messageType);
        String realInstanceName = instanceName + "_" + callerType.getSimpleName();

        MessageConsumer consumer;
        try {
            String queueName = "";
            if (isCommand) {
                queueName = "COMMANDS." + messageTypeName;
            } else if (isEvent) {
                queueName = "Consumer." + realInstanceName + ".VirtualTopic.EVENTS_" + messageTypeName;
            }

            ActiveMQQueue commandsQueue = new ActiveMQQueue(queueName);
            consumer = session.createConsumer(commandsQueue);
            consumer.setMessageListener((javax.jms.Message genericMsg) -> {
                handleMessage(genericMsg, handlerFunction);
            });
        } catch (JMSException ex) {
            logger.log(Level.SEVERE, "Error registering message handler", ex);
        }

    }

    private void handleMessage(javax.jms.Message genericMsg, Consumer<Object> handlerFunction) throws RuntimeException {
        try {
            TextMessage msg = (TextMessage) genericMsg;
            String jsonContent = msg.getText();
            String stringType = msg.getStringProperty("TYPE").toUpperCase(Locale.ROOT);
            Class classType = messageTypes.get(stringType);
            Message event = (Message) mapper.readValue(jsonContent, classType);

            handlerFunction.accept(event);
        } catch (IOException | JMSException ex) {
            logger.log(Level.SEVERE, "Error receiving message", ex);
            throw new RuntimeException(ex);
        }
    }

    @Override
    public void send(Message message) {
        if (message == null) {
            return;
        }
        try {
            Class messageType = message.getClass();
            String messageTypeName = messageType.getSimpleName().toUpperCase(Locale.ROOT);
            boolean isEvent = Event.class.isAssignableFrom(messageType);
            boolean isCommand = Command.class.isAssignableFrom(messageType);
            Destination destination = null;
            if (isCommand) {
                destination = session.createQueue("COMMANDS." + messageTypeName);
            } else if (isEvent) {
                destination = session.createTopic("VirtualTopic.EVENTS_" + messageTypeName);
            }

            // Create a MessageProducer from the Session to the Topic or Queue
            MessageProducer producer = session.createProducer(destination);
            producer.setDeliveryMode(DeliveryMode.PERSISTENT);

            TextMessage amqMessage = session.createTextMessage(mapper.writeValueAsString(message));
            amqMessage.setStringProperty("TYPE", messageTypeName);
            amqMessage.setJMSCorrelationID(message.getCorrelationId().toString());
            producer.send(amqMessage);
        } catch (JMSException | JsonProcessingException ex) {
            logger.log(Level.SEVERE, "Error sending data", ex);
        }
    }

    @Override
    public Class getType(String messageTypeName) {
        messageTypeName = messageTypeName.toUpperCase(Locale.ROOT);
        if (!messageTypes.containsKey(messageTypeName)) {
            return null;
        }
        return messageTypes.get(messageTypeName);
    }

    @Override
    public List<String> getTypes() {
        return new ArrayList<>(messageTypes.keySet());
    }
}