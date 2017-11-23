package org.cqrs;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentLinkedQueue;
import java.util.function.Consumer;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.jms.Destination;
import javax.jms.JMSException;
import javax.jms.MessageConsumer;
import javax.jms.MessageProducer;
import javax.jms.TextMessage;

@SuppressWarnings("unchecked")
public class ActiveMqBusImpl implements Bus {

    private static final Logger logger = Logger.getLogger(ActiveMqBusImpl.class.getSimpleName());

    private final String instanceName;
    private final ActiveMqBusHelper busHelper;
    private ObjectMapper mapper = new ObjectMapper();


    public ActiveMqBusImpl(List<MessageHandler> messageHandlers, String instanceName,ActiveMqBusHelper activeMqBusHelper) throws Exception {

        this.instanceName = instanceName;
        busHelper = activeMqBusHelper;

        for (int i = 0; i < messageHandlers.size(); i++) {
            MessageHandler messageHandler = messageHandlers.get(i);
            messageHandler.register(this);
        }
    }

    private final ConcurrentHashMap<String, Class> messageTypes = new ConcurrentHashMap<>();
    private final ConcurrentLinkedQueue<MessageConsumer> consumers = new ConcurrentLinkedQueue<>();

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
            }else {
                queueName = "Consumer." + realInstanceName + ".VirtualTopic.MESSAGE_" + messageTypeName;
            }


            consumer = busHelper.createConsumer(queueName);
            consumer.setMessageListener((javax.jms.Message genericMsg) -> {
                handleMessage(genericMsg, handlerFunction);
            });
            consumers.add(consumer);
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
            if(message.getCorrelationId()==null){
                message.setCorrelationId(UUID.randomUUID());
            }
            Class messageType = message.getClass();
            String messageTypeName = messageType.getSimpleName().toUpperCase(Locale.ROOT);
            boolean isEvent = Event.class.isAssignableFrom(messageType);
            boolean isCommand = Command.class.isAssignableFrom(messageType);
            Destination destination = null;
            if (isCommand) {
                destination = busHelper.createQueue("COMMANDS." + messageTypeName);
            } else if (isEvent) {
                destination = busHelper.createTopic("VirtualTopic.EVENTS_" + messageTypeName);
            } else  {
                destination = busHelper.createTopic("VirtualTopic.MESSAGE_" + messageTypeName);
            }

            // Create a MessageProducer from the Session to the Topic or Queue
            MessageProducer producer = busHelper.createProducer(destination);


            TextMessage amqMessage = busHelper.createTextMessage(mapper.writeValueAsString(message));
            amqMessage.setStringProperty("TYPE", messageTypeName);
            amqMessage.setJMSCorrelationID(message.getCorrelationId().toString());
            producer.send(amqMessage);
        } catch (JMSException | JsonProcessingException ex) {
            logger.log(Level.SEVERE, "Error sending data", ex);
        }
    }

    @Override
    public void resetHandlers() {
        while(!consumers.isEmpty()){
            MessageConsumer consumer = consumers.poll();
            busHelper.removeConsumer(consumer);
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
