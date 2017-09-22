package org.cqrs;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Consumer;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.jms.Connection;
import javax.jms.ConnectionFactory;
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
    public static String brokerURL = "failover:tcp://localhost:61616";
    private ConnectionFactory factory = new ActiveMQConnectionFactory(brokerURL);
    private final Connection connection;
    private final String instanceName;
    private ObjectMapper mapper = new ObjectMapper();
    private Session session;

    @Inject
    public ActiveMqBusImpl(List<MessageHandler> messageHandlers, String instanceName) throws Exception {
        try {
            this.instanceName = instanceName;
            connection = factory.createConnection();
            connection.start();
            session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);

            for (int i = 0; i < messageHandlers.size(); i++) {
                MessageHandler messageHandler = messageHandlers.get(i);
                messageHandler.Register(this);
            }
        } catch (JMSException ex) {
            logger.log(Level.SEVERE, "Error initializing ActiveMQ and Handlers;", ex);
            throw new Exception(ex);
        }
    }

    private final ConcurrentHashMap<String, Class> _messageTypes = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<Class, ArrayList<Consumer<Object>>> _handlerFunctions = new ConcurrentHashMap<>();

    @Override
    public void RegisterHandler(Consumer<Object> handlerFunction, Class messageType) {

        boolean isEvent = Event.class.isAssignableFrom(messageType);
        boolean isCommand = Command.class.isAssignableFrom(messageType);
        String messageTypeName = messageType.getSimpleName().toUpperCase();
        if (!_messageTypes.containsKey(messageTypeName)) {
            _messageTypes.put(messageTypeName, messageType);
        }

        if (!_handlerFunctions.containsKey(messageType)) {
            _handlerFunctions.put(messageType, new ArrayList<>());
            MessageConsumer consumer = null;
            try {
                String queueName = "";
                if (isCommand) {
                    queueName = "COMMANDS";
                } else if (isEvent) {
                    queueName = "Consumer." + instanceName + ".VirtualTopic.EVENTS_" + messageTypeName;
                }

                ActiveMQQueue commandsQueue = new ActiveMQQueue(queueName);
                consumer = session.createConsumer(commandsQueue);
                consumer.setMessageListener((javax.jms.Message genericMsg) -> {
                    HandleMessage(genericMsg);
                });
            } catch (Exception ex) {
                logger.log(Level.SEVERE, "Error registering message handler", ex);
            }
        }
        _handlerFunctions.get(messageType).add(handlerFunction);
    }

    private void HandleMessage(javax.jms.Message genericMsg) throws RuntimeException {
        try {
            TextMessage msg = (TextMessage) genericMsg;
            String jsonContent = msg.getText();
            String stringType = msg.getStringProperty("TYPE").toUpperCase();
            Class classType = _messageTypes.get(stringType);
            Message event = (Message) mapper.readValue(jsonContent, classType);

            if (_handlerFunctions.containsKey(classType)) {
                List<Consumer<Object>> handlerFunctions = _handlerFunctions.get(classType);
                for (int i = 0; i < handlerFunctions.size(); i++) {
                    handlerFunctions.get(i).accept(event);

                }
            }
        } catch (Exception ex) {
            logger.log(Level.SEVERE, "Error receiving message", ex);
            throw new RuntimeException(ex);
        }
    }

    @Override
    public void Send(Message message) {
        if (message == null) {
            return;
        }
        try {
            Class messageType = message.getClass();
            String messageTypeName = messageType.getSimpleName().toUpperCase();
            boolean isEvent = Event.class.isAssignableFrom(messageType);
            boolean isCommand = Command.class.isAssignableFrom(messageType);
            Destination destination = null;
            if (isCommand) {
                destination = session.createQueue("COMMANDS");
            } else if (isEvent) {
                destination = session.createTopic("VirtualTopic.EVENTS_" + messageTypeName);
            }

            // Create a MessageProducer from the Session to the Topic or Queue
            MessageProducer producer = session.createProducer(destination);
            producer.setDeliveryMode(DeliveryMode.PERSISTENT);

            TextMessage amqMessage = session.createTextMessage(mapper.writeValueAsString(message));
            amqMessage.setStringProperty("TYPE", messageTypeName);
            producer.send(amqMessage);
        } catch (Exception ex) {
            logger.log(Level.SEVERE, "Error sending data", ex);
        }
    }

    @Override
    public Class getType(String messageTypeName) {
        messageTypeName = messageTypeName.toUpperCase();
        if (!_messageTypes.containsKey(messageTypeName)) {
            return null;
        }
        return _messageTypes.get(messageTypeName);
    }

    @Override
    public List<String> getTypes() {
        return new ArrayList<>(_messageTypes.keySet());
    }
}
