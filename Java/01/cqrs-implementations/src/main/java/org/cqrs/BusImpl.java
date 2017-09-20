package org.cqrs;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Consumer;
import javax.inject.Inject;
import javax.inject.Named;

@Named("bus")
public class BusImpl implements Bus {

    @Inject
    public BusImpl(List<MessageHandler> messageHandlers) {
        for (int i = 0; i < messageHandlers.size(); i++) {
            MessageHandler messageHandler = messageHandlers.get(i);
            messageHandler.Register(this);
        }
    }

    private final ConcurrentHashMap<String, Class> _messageTypes = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<Class, ArrayList<Consumer<Object>>> _handlerFunctions = new ConcurrentHashMap<>();

    @Override
    public void RegisterHandler(Consumer<Object> handlerFunction, Class messageType) {
        if (!_handlerFunctions.containsKey(messageType)) {
            _handlerFunctions.put(messageType, new ArrayList<>());
        }
        if (Command.class.isAssignableFrom(messageType)) {
            String messageTypeName = messageType.getSimpleName().toUpperCase();
            if (!_messageTypes.containsKey(messageTypeName)) {
                _messageTypes.put(messageTypeName, messageType);
            }
        }
        _handlerFunctions.get(messageType).add(handlerFunction);
    }

    @Override
    public void Send(Message message) {
        if (message == null) {
            return;
        }
        Class messageType = message.getClass();
        if (_handlerFunctions.containsKey(messageType)) {
            List<Consumer<Object>> handlerFunction = _handlerFunctions.get(messageType);
            for (int i = 0; i < handlerFunction.size(); i++) {
                try {
                    handlerFunction.get(i).accept(message);
                } catch (Exception ex) {
                    System.out.println(ex);
                }
            }
        }
    }

    @Override
    public Class getType(String messageTypeName) {
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
