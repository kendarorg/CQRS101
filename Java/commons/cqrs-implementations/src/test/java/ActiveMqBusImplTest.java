import org.apache.activemq.ActiveMQConnection;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.advisory.DestinationSource;
import org.apache.activemq.broker.Broker;
import org.apache.activemq.broker.BrokerService;
import org.apache.activemq.broker.TransportConnector;
import org.apache.activemq.command.ActiveMQDestination;
import org.apache.activemq.command.ActiveMQQueue;
import org.apache.activemq.command.ActiveMQTopic;
import org.cqrs.*;
import org.cqrs101.utils.MainEnvironment;
import org.junit.*;

import javax.print.attribute.standard.Destination;
import java.net.URI;
import java.util.*;

import static org.junit.Assert.assertEquals;

public class ActiveMqBusImplTest {

    private  List<Message> handled;
    private ActiveMqBusHelper busHelper;
    private static BrokerService broker;

    @BeforeClass
    public static void setUpAll() throws Exception {
        broker = new BrokerService();

        TransportConnector connector = new TransportConnector();
        connector.setUri(new URI("vm://localhost?broker.persistent=false"));
        broker.addConnector(connector);
        broker.start();
        broker.waitUntilStarted();

    }

    @AfterClass
    public static void tearDownAll() throws Exception {
        broker.stop();
        broker.waitUntilStopped();

    }

    @After
    public void tearDown() throws Exception {

        broker.deleteAllMessages();
        new ActiveMQBrokerExtension(broker.getBroker()).clearAllMessages();

    }

    @Before
    public void setUp() throws Exception {

        broker.deleteAllMessages();

        handled = new ArrayList<>();
        MainEnvironment env = new MainEnvironment(null);
        env.setProperty("amq.brokerurl","vm://localhost?broker.persistent=false");
        busHelper = new ActiveMqBusHelper(env);
    }

    @Test
    public void shouldAllowExecutingWithoutHandler() throws Exception {
        ActiveMqBusImpl target = new ActiveMqBusImpl(new ArrayList<>(),"test", busHelper);

        target.send(new SimpleMessage());

        assertEquals(0,handled.size());
        target.resetHandlers();
    }

    @Test
    public void shouldAllowExecutingSimpleHandler() throws Exception {
        List<MessageHandler> handlers = getSimpleMessageHandler();
        SimpleMessage message = new SimpleMessage();
        message.setCorrelationId(UUID.randomUUID());

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test10", busHelper);

        target.send(message);
        Thread.sleep(500);

            assertEquals(1,handled.size());
        assertEquals(message.getCorrelationId(),handled.get(0).getCorrelationId());

        target.resetHandlers();
    }

    @Test
    public void shouldExecuteCommandsOnlyOnce() throws Exception {
        SimpleCommand message = new SimpleCommand();

        List<MessageHandler> handlers = getDoubleCommandHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test2", busHelper);

        target.send(message);
        Thread.sleep(500);

        assertEquals(1,handled.size());
        SimpleCommand result = (SimpleCommand)handled.get(0);
        assertEquals(message.getCorrelationId(),result.getCorrelationId());

        target.resetHandlers();
    }

    @Test
    public void shouldExecuteMessagesForAllHandlers() throws Exception {
        SimpleMessage message = new SimpleMessage();

        List<MessageHandler> handlers = getDoubleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test3", busHelper);

        target.send(message);
        Thread.sleep(500);

        assertEquals(2,handled.size());
        SimpleMessage result =(SimpleMessage) handled.get(0);
        assertEquals(message.getCorrelationId(),result.getCorrelationId());
        result =(SimpleMessage) handled.get(1);
        assertEquals(message.getCorrelationId(),result.getCorrelationId());

        target.resetHandlers();
    }

    @Test
    public void shouldGetMultipleTypes() throws Exception {

        List<MessageHandler> handlers = getDoubleMessageHandler();
        handlers.addAll(getDoubleCommandHandler());

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test5", busHelper);


        List<String> types = target.getTypes();

        assertEquals(2,types.size());
        assertEquals("SIMPLECOMMAND",types.get(0));
        assertEquals("SIMPLEMESSAGE",types.get(1));

        target.resetHandlers();
    }

    @Test
    public void shouldTranslateLowerCaseTypes() throws Exception {

        List<MessageHandler> handlers = getSimpleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test6", busHelper);


        Object type = target.getType("simplemessage");

        assertEquals(SimpleMessage.class,type);

        target.resetHandlers();
    }

    @Test
    public void shouldTranslateMixedCaseTypes() throws Exception {

        List<MessageHandler> handlers = getSimpleMessageHandler();

        ActiveMqBusImpl target = new ActiveMqBusImpl(handlers,"test7", busHelper);


        Object type = target.getType("sImpleMESSage");

        assertEquals(SimpleMessage.class,type);

        target.resetHandlers();
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
