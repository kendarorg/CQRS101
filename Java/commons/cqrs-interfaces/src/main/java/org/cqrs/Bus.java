package org.cqrs;

import java.util.List;
import java.util.function.Consumer;

public interface Bus
{
    Class getType(String name);
    List<String> getTypes();
    void registerHandler(Consumer<Object> handlerFunction,Class messageType,Class callerType);
    void send(Message message);
    void resetHandlers();
}
