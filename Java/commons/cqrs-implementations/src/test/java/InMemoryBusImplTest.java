import org.cqrs.InMemoryBusImpl;
import org.cqrs.Message;
import org.cqrs.MessageHandler;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertSame;

public class InMemoryBusImplTest {

    private List<Message> handled;

    @Before
    public void setUp(){
        handled = new ArrayList<>();
    }

    @Test
    public void shouldAllowExecutingWithoutHandler(){
        InMemoryBusImpl target = new InMemoryBusImpl(new ArrayList<>());

        target.send(new SimpleMessage());
    }

    @Test
    public void shouldAllowExecutingSimpleHandler(){
        List<MessageHandler> handlers = getSimpleMessageHandler();
        SimpleMessage message = new SimpleMessage();

        InMemoryBusImpl target = new InMemoryBusImpl(handlers);

        target.send(message);

        assertEquals(1,handled.size());
        assertSame(message,handled.get(0));
    }

    @Test
    public void shouldExecuteCommandsOnlyOnce(){
        SimpleCommand message = new SimpleCommand();

        List<MessageHandler> handlers = getDoubleCommandHandler();

        InMemoryBusImpl target = new InMemoryBusImpl(handlers);

        target.send(message);

        assertEquals(1,handled.size());
        assertSame(message,handled.get(0));
    }

    @Test
    public void shouldExecuteMessagesForAllHandlers(){
        SimpleMessage message = new SimpleMessage();

        List<MessageHandler> handlers = getDoubleMessageHandler();

        InMemoryBusImpl target = new InMemoryBusImpl(handlers);

        target.send(message);

        assertEquals(2,handled.size());
        assertSame(message,handled.get(0));
        assertSame(message,handled.get(1));
    }

    @Test
    public void shouldGetMultipleTypes(){

        List<MessageHandler> handlers = getDoubleMessageHandler();
        handlers.addAll(getDoubleCommandHandler());

        InMemoryBusImpl target = new InMemoryBusImpl(handlers);


        List<String> types = target.getTypes();

        assertEquals(2,types.size());
        assertEquals("SIMPLECOMMAND",types.get(0));
        assertEquals("SIMPLEMESSAGE",types.get(1));
    }

    @Test
    public void shouldTranslateLowerCaseTypes(){

        List<MessageHandler> handlers = getSimpleMessageHandler();

        InMemoryBusImpl target = new InMemoryBusImpl(handlers);


        Object type = target.getType("simplemessage");

        assertEquals(SimpleMessage.class,type);
    }

    @Test
    public void shouldTranslateMixedCaseTypes(){

        List<MessageHandler> handlers = getSimpleMessageHandler();

        InMemoryBusImpl target = new InMemoryBusImpl(handlers);


        Object type = target.getType("sImpleMESSage");

        assertEquals(SimpleMessage.class,type);
    }

    private List<MessageHandler> getDoubleCommandHandler() {
        List<MessageHandler> handlers =new ArrayList<>();
        handlers.add(new SimpleMessageHandler<SimpleCommand>() {
            @Override
            public void handle(SimpleCommand message) {
                handled.add(message);
            }
        });
        handlers.add(new SimpleMessageHandler<SimpleCommand>() {
            @Override
            public void handle(SimpleCommand message) {
                handled.add(message);
            }
        });
        return handlers;
    }

    private List<MessageHandler> getDoubleMessageHandler() {
        List<MessageHandler> handlers =new ArrayList<>();
        handlers.add(new SimpleMessageHandler<SimpleMessage>() {
            @Override
            public void handle(SimpleMessage message) {
                handled.add(message);
            }
        });
        handlers.add(new SimpleMessageHandler<SimpleMessage>() {
            @Override
            public void handle(SimpleMessage message) {
                handled.add(message);
            }
        });
        return handlers;
    }

    private List<MessageHandler> getSimpleMessageHandler() {
        List<MessageHandler> handlers =new ArrayList<>();
        handlers.add(new SimpleMessageHandler<SimpleMessage>() {
            @Override
            public void handle(SimpleMessage message) {
                handled.add(message);
            }
        });
        return handlers;
    }
}
