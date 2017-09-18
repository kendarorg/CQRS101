package org.cqrs;

    public interface MessageHandler
    {
        void Register(Bus bus);
    }
