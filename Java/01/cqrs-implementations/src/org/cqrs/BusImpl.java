package org.cqrs;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;
import java.util.function.Consumer;

public class BusImpl implements Bus {

    public BusImpl(List<MessageHandler> validators) {
        for (int i = 0; i < validators.size(); i++) {
            MessageHandler validator = validators.get(i);
            validator.Register(this);
        }
    }

    private Hashtable<Class, ArrayList<Consumer<Object>>> _handlerFunctions = new Hashtable<>();

    /// <summary>
    /// This method contains the system to retrieve the handle methods from all
    /// message handlers passed
    /// </summary>
    @Override
    public void RegisterHandler(Consumer<Object> handlerFunction, Class messageType) {
        if (!_handlerFunctions.containsKey(messageType)) {
            _handlerFunctions.put(messageType, new ArrayList<>());
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
}
