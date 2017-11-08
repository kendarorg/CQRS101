package org.cqrs;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Consumer;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;

public class InMemoryBusImpl implements Bus {

    private static final Logger logger = Logger.getLogger(InMemoryBusImpl.class.getSimpleName());

    public InMemoryBusImpl(List<MessageHandler> messageHandlers) {
        for (int i = 0; i < messageHandlers.size(); i++) {
            MessageHandler messageHandler = messageHandlers.get(i);
            messageHandler.register(this);
        }
    }

    private final ConcurrentHashMap<String, Class> messageTypes = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<Class, ArrayList<Consumer<Object>>> handlerFunctions = new ConcurrentHashMap<>();

    @Override
    public void registerHandler(Consumer<Object> handlerFunction, Class messageType, Class callerType) {
        handlerFunctions.putIfAbsent(messageType, new ArrayList<>());

        String messageTypeName = messageType.getSimpleName().toUpperCase(Locale.ROOT);
        messageTypes.putIfAbsent(messageTypeName, messageType);

        if (Command.class.isAssignableFrom(messageType) &&
                handlerFunctions.get(messageType).size()>0) {
            return;
        }
        handlerFunctions.get(messageType).add(handlerFunction);
    }

    @Override
    public void send(Message message) {
        if (message == null) {
            return;
        }

        Class messageType = message.getClass();
        if (handlerFunctions.containsKey(messageType)) {
            List<Consumer<Object>> handlerFunction = handlerFunctions.get(messageType);
            for (int i = 0; i < handlerFunction.size(); i++) {
                try {
                    handlerFunction.get(i).accept(message);
                } catch (Exception ex) {
                    logger.log(Level.SEVERE, "Error handling message: " + messageType.getSimpleName(), ex);
                }
            }
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
