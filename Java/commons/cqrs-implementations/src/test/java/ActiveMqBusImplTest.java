import org.apache.activemq.broker.BrokerService;
import org.apache.activemq.broker.TransportConnector;
import org.apache.activemq.command.ActiveMQDestination;
import org.cqrs.ActiveMqBusHelper;
import org.cqrs.ActiveMqBusImpl;
import org.cqrs.Message;
import org.cqrs.MessageHandler;
import org.cqrs101.utils.MainEnvironment;
import org.junit.*;
import org.mockito.Mockito;
import org.mockito.invocation.InvocationOnMock;
import org.mockito.stubbing.Answer;

import javax.jms.*;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertSame;

public class ActiveMqBusImplTest {

    private  List<Message> handled;
    private ActiveMqBusHelper busHelper;
    private BrokerService broker;

    @BeforeClass
    public static void setUpAll() throws Exception {


    }

    @AfterClass
    public static void tearDownAll() throws Exception {

    }

    @After
    public void tearDown() throws Exception {
        broker.stop();
    }

    @Before
    public void setUp() throws Exception {
        broker = new BrokerService();

        TransportConnector connector = new TransportConnector();
        connector.setUri(new URI("tcp://localhost:61612"));
        broker.addConnector(connector);
        broker.start();

        handled = new ArrayList<>();
        MainEnvironment env = new MainEnvironment(null);
        env.setProperty("amq.brokerurl","tcp://localhost:61612");
        busHelper = new ActiveMqBusHelper(env);
    }

    @Test
    public void shouldAllowExecutingWithoutHandler() throws Exception {
        ActiveMqBusImpl target = new ActiveMqBusImpl(new ArrayList<>(),"test", busHelper);

        target.send(new SimpleMessage());
    }

    @Test
    @Ignore
    public void shouldAllowExecutingSimpleHandler() throws Exception {
        List<MessageHandler> handlers = getSimpleMessageHandler();
        SimpleMessage message = new SimpleMessage();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test", busHelper);

        target.send(message);
        Thread.sleep(500);

        assertEquals(1,handled.size());
        assertSame(message,handled.get(0));
    }

    @Test
    public void shouldExecuteCommandsOnlyOnce() throws Exception {
        SimpleCommand message = new SimpleCommand();

        List<MessageHandler> handlers = getDoubleCommandHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test", busHelper);

        target.send(message);
        Thread.sleep(500);

        assertEquals(1,handled.size());
        SimpleCommand result = (SimpleCommand)handled.get(0);
        assertEquals(message.getCorrelationId(),result.getCorrelationId());
    }

    @Test
    @Ignore
    public void shouldExecuteMessagesForAllHandlers() throws Exception {
        SimpleMessage message = new SimpleMessage();

        List<MessageHandler> handlers = getDoubleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test", busHelper);

        target.send(message);
        Thread.sleep(500);

        assertEquals(2,handled.size());
        SimpleMessage result =(SimpleMessage) handled.get(0);
        assertEquals(message.getCorrelationId(),result.getCorrelationId());
        result =(SimpleMessage) handled.get(1);
        assertEquals(message.getCorrelationId(),result.getCorrelationId());
    }

    @Test
    public void shouldGetMultipleTypes() throws Exception {

        List<MessageHandler> handlers = getDoubleMessageHandler();
        handlers.addAll(getDoubleCommandHandler());

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test", busHelper);


        List<String> types = target.getTypes();

        assertEquals(2,types.size());
        assertEquals("SIMPLECOMMAND",types.get(0));
        assertEquals("SIMPLEMESSAGE",types.get(1));
    }

    @Test
    public void shouldTranslateLowerCaseTypes() throws Exception {

        List<MessageHandler> handlers = getSimpleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test", busHelper);


        Object type = target.getType("simplemessage");

        assertEquals(SimpleMessage.class,type);
    }

    @Test
    public void shouldTranslateMixedCaseTypes() throws Exception {

        List<MessageHandler> handlers = getSimpleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test", busHelper);


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
