package org.cqrs;

public interface MessageHandler {

    void register(Bus bus);
}
