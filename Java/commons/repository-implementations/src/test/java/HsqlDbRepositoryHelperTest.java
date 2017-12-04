import org.cqrs101.HsqlDbRepositoryHelper;
import org.cqrs101.utils.MainEnvironment;
import org.junit.*;
import org.mockito.Matchers;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import java.util.UUID;

import static org.junit.Assert.*;
import static org.mockito.BDDMockito.given;
import static org.mockito.Matchers.anyString;
import static org.mockito.Mockito.mock;

public class HsqlDbRepositoryHelperTest {

    private HsqlDbRepositoryHelper factory;

    private Connection connection;
    private static Driver driver;
    private Statement statement;
    private ResultSet resultSet;
    private ArrayList<String> queries;
    private MainEnvironment mainEnvironment;

    @BeforeClass
    public static void setUpClass() throws Exception {
        // (Optional) Print DriverManager logs to system out
        //DriverManager.setLogWriter(new PrintWriter((System.out)));

        try {
            DriverManager.registerDriver(new MockDriver());
            // (Optional) Sometimes you need to get rid of a driver (e.g JDBC-ODBC Bridge)
            Driver configuredDriver = DriverManager.getDriver("jdbc:odbc:url");

            DriverManager.deregisterDriver(configuredDriver);
        }catch(Exception ex){

        }

        // Register the mocked driver
        driver = mock(Driver.class);
        DriverManager.registerDriver(driver);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        // Let's cleanup the global state
        DriverManager.deregisterDriver(driver);
    }

    private void setUpDriver() throws Exception {
        // given

        connection = mock(Connection.class);
        statement = mock(Statement.class);
        resultSet = mock(ResultSet.class);

        given(driver.acceptsURL(anyString())).willReturn(true);
        given(driver.connect(anyString(), Matchers.<Properties>any()))
                .willReturn(connection);
        given(connection.createStatement())
                .willReturn(statement);
        given(statement.executeQuery(anyString()))
                //.willReturn(resultSet)
                .will(invocationOnMock -> {
                    queries.add(invocationOnMock.getArguments()[0].toString());
                    return resultSet;
                });
        given(statement.execute(anyString()))
                            //.willReturn(resultSet)
                            .will(invocationOnMock1 -> {
                                queries.add(invocationOnMock1.getArguments()[0].toString());
                                return true;
        });
    }

    @Before
    public void setUp() throws Exception {
        setUpDriver();
        mainEnvironment = new MainEnvironment(null);
        mainEnvironment.setProperty("db.url","test");
        mainEnvironment.setProperty("db.user","test");
        mainEnvironment.setProperty("db.password","test");
        //env.setProperty();
        queries = new ArrayList<>();
        factory = new HsqlDbRepositoryHelper(mainEnvironment);
    }

    @After
    public void cleanUp(){
        factory.truncate();
    }

    @Test
    public void shouldAssignTheNameBasedOnClassName(){

        HsqlDbRepositoryHelper result =(HsqlDbRepositoryHelper) factory.create(Object.class);

        assertEquals("OBJECT", result.getName());
    }

    @Test
    public void shouldCreateDifferentInstancesForEachCall(){
        HsqlDbRepositoryHelper target = new HsqlDbRepositoryHelper(mainEnvironment);

        HsqlDbRepositoryHelper result1 = (HsqlDbRepositoryHelper) target.create(SimpleObject.class);
        HsqlDbRepositoryHelper result2 = (HsqlDbRepositoryHelper) target.create(SimpleObject.class);

        assertNotSame(result1,result2);
    }

    @Test
    public void shouldStoreAndRetrieveTheSameObject(){
        HsqlDbRepositoryHelper target = (HsqlDbRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so = new SimpleObject();
        UUID id = UUID.randomUUID();
        so.setId(id);
        so.setData("");

        target.save(so,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        assertEquals(2,queries.size());
        assertTrue(queries.get(0).indexOf("SELECT data FROM \"SIMPLEOBJECT\" WHERE id =")==0);
        assertTrue(queries.get(0).indexOf(so.getId().toString())>0);
        assertTrue(queries.get(1).indexOf("INSERT INTO \"SIMPLEOBJECT\" (id,data) VALUES (")==0);
        assertTrue(queries.get(1).indexOf(so.getId().toString())>0);
    }


    @Test
    public void shouldStoreAndRetrieveTheSameObjectBetweenTwoHelpers(){
        HsqlDbRepositoryHelper targetWrite = (HsqlDbRepositoryHelper) factory.create(SimpleObject.class);
        HsqlDbRepositoryHelper targetRead =(HsqlDbRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so = new SimpleObject();
        UUID id = UUID.randomUUID();
        so.setId(id);
        so.setData("");

        targetWrite.save(so,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        SimpleObject result = (SimpleObject)targetRead.getById(id);
        assertEquals(3,queries.size());
        assertTrue(queries.get(0).indexOf("SELECT data FROM \"SIMPLEOBJECT\" WHERE")==0);
        assertTrue(queries.get(0).indexOf(so.getId().toString())>0);
        assertTrue(queries.get(1).indexOf("INSERT INTO \"SIMPLEOBJECT\" (id,data) VALUES (")==0);
        assertTrue(queries.get(1).indexOf(so.getId().toString())>0);
        assertEquals(queries.get(2),queries.get(0));
    }

    @Test
    public void shouldAssignIdWhenNotPresent(){
        HsqlDbRepositoryHelper target =(HsqlDbRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so = new SimpleObject();

        assertNull(so.getId());

        target.save(so,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        List<Object> resultList = target.getAll();
        assertEquals(0,resultList.size());
        assertEquals(2,queries.size());
        assertTrue(queries.get(0).indexOf("INSERT INTO \"SIMPLEOBJECT\" (id,data) VALUES (")==0);
        assertTrue(queries.get(0).indexOf(so.getId().toString())>0);
        assertTrue(queries.get(1).indexOf("SELECT * FROM \"SIMPLEOBJECT\";")==0);
    }

    @Test
    public void shouldRetrieveAllItems(){
        HsqlDbRepositoryHelper target =(HsqlDbRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so1 = new SimpleObject();
        SimpleObject so2 = new SimpleObject();

        target.save(so1,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );
        target.save(so2,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        List<Object> resultList = target.getAll();


        assertEquals(0,resultList.size());
        assertEquals(3,queries.size());
        assertTrue(queries.get(0).indexOf("INSERT INTO \"SIMPLEOBJECT\" (id,data) VALUES (")==0);
        assertTrue(queries.get(0).indexOf(so1.getId().toString())>0);
        assertTrue(queries.get(1).indexOf("INSERT INTO \"SIMPLEOBJECT\" (id,data) VALUES (")==0);
        assertTrue(queries.get(1).indexOf(so2.getId().toString())>0);
        assertTrue(queries.get(2).indexOf("SELECT * FROM \"SIMPLEOBJECT\";")==0);
    }
}
