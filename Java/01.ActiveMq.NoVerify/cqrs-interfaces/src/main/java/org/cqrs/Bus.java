package org.cqrs;

import java.util.List;
import java.util.function.Consumer;

public interface Bus
{
    Class getType(String name);
    List<String> getTypes();
    void RegisterHandler(Consumer<Object> handlerFunction,Class messageType);
    void Send(Message message);
}
