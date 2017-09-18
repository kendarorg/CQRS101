package org.cqrs;

import java.util.function.Consumer;

public interface Bus
{
    void RegisterHandler(Consumer<Object> handlerFunction,Class messageType);
    void Send(Message message);
}
