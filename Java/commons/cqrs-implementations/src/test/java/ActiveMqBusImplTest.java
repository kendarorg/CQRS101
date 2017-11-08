import org.cqrs.ActiveMqBusHelper;
import org.cqrs.ActiveMqBusImpl;
import org.cqrs.Message;
import org.cqrs.MessageHandler;
import org.cqrs101.utils.MainEnvironment;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertSame;
import static org.mockito.Mockito.mock;

public class ActiveMqBusImplTest {

    private List<Message> handled;
    private ActiveMqBusHelper environment;

    @Before
    public void setUp(){
        handled = new ArrayList<>();
        environment = mock(ActiveMqBusHelper.class);
    }

    @Test
    public void shouldAllowExecutingWithoutHandler() throws Exception {
        ActiveMqBusImpl target = new ActiveMqBusImpl(new ArrayList<>(),"test",environment);

        target.send(new SimpleMessage());
    }

    @Test
    public void shouldAllowExecutingSimpleHandler() throws Exception {
        List<MessageHandler> handlers = getSimpleMessageHandler();
        SimpleMessage message = new SimpleMessage();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test",environment);

        target.send(message);

        assertEquals(1,handled.size());
        assertSame(message,handled.get(0));
    }

    @Test
    public void shouldExecuteCommandsOnlyOnce() throws Exception {
        SimpleCommand message = new SimpleCommand();

        List<MessageHandler> handlers = getDoubleCommandHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test",environment);

        target.send(message);

        assertEquals(1,handled.size());
        assertSame(message,handled.get(0));
    }

    @Test
    public void shouldExecuteMessagesForAllHandlers() throws Exception {
        SimpleMessage message = new SimpleMessage();

        List<MessageHandler> handlers = getDoubleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test",environment);

        target.send(message);

        assertEquals(2,handled.size());
        assertSame(message,handled.get(0));
        assertSame(message,handled.get(1));
    }

    @Test
    public void shouldGetMultipleTypes() throws Exception {

        List<MessageHandler> handlers = getDoubleMessageHandler();
        handlers.addAll(getDoubleCommandHandler());

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test",environment);


        List<String> types = target.getTypes();

        assertEquals(2,types.size());
        assertEquals("SIMPLECOMMAND",types.get(0));
        assertEquals("SIMPLEMESSAGE",types.get(1));
    }

    @Test
    public void shouldTranslateLowerCaseTypes() throws Exception {

        List<MessageHandler> handlers = getSimpleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test",environment);


        Object type = target.getType("simplemessage");

        assertEquals(SimpleMessage.class,type);
    }

    @Test
    public void shouldTranslateMixedCaseTypes() throws Exception {

        List<MessageHandler> handlers = getSimpleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test",environment);


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
