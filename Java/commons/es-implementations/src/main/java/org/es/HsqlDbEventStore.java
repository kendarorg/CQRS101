package org.es;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.cqrs.Event;
import org.cqrs101.utils.MainEnvironment;

import javax.inject.Inject;
import javax.inject.Named;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;

@Named("eventStore")
public class HsqlDbEventStore implements EventStore {

    private static final ObjectMapper mapper = new ObjectMapper();
    private final MainEnvironment environment;
    private final String driverName = "org.hsqldb.jdbcDriver";
    private static ConcurrentHashMap<String,Class> classNames = new ConcurrentHashMap<>();

    @Inject
    public HsqlDbEventStore(MainEnvironment environment,List<AggregateRoot> aggregateRoots) {
        this.environment = environment;
        this.initialize(aggregateRoots);
        this.create();
    }

    private void initialize(List<AggregateRoot> aggregateRoots) {
        for(AggregateRoot ar : aggregateRoots){
            ar.initializeApply(this);
        }
    }

    public void create() {
        Connection conn = null;

        try {
            Class.forName(driverName);
        } catch (Exception e) {
            throw new RuntimeException("Missing HSQLDB Driver");
        }

        try {
            conn = createConnection();
            Statement stmt = conn.createStatement();
            stmt.executeUpdate(
                    "CREATE TABLE \"EVENT_STORE\" ("
                            + "id UUID  NOT NULL,version integer NOT NULL,clazz varchar(512),"
                            + "data varchar(680000) NOT NULL,"
                            + "PRIMARY KEY (id,version));");
        } catch (Exception ex) {

        } finally {
            try {
                if (conn != null) {
                    conn.close();
                }
            } catch (SQLException ex) {
                System.out.println(ex);
            }
        }
    }

    private Connection createConnection() throws SQLException {
        String url = environment.getProperty("hsqldb.url");
        String customer = environment.getProperty("hsqldb.customer");
        String password = environment.getProperty("hsqldb.password");
        return DriverManager.getConnection(url, customer, password);
    }

    @Override
    public List<EsEvent> getEventsForAggregate(UUID aggregateId) {
        List<EsEvent> result = new ArrayList<>();
        Connection conn = null;
        try {
            conn = createConnection();
            Statement stmt = conn.createStatement();
            ResultSet resultSet = stmt.executeQuery(
                    "SELECT * FROM \"EVENT_STORE\" WHERE "
                            + "id ='" + aggregateId + "' ORDER BY version ASC;");
            for (; resultSet.next();) {
                EventDescriptor ed = new EventDescriptor();
                String clazzName = resultSet.getString("clazz").
                        toLowerCase(Locale.ROOT);
                String data = resultSet.getString("data");

                int version = resultSet.getInt("version");

                Class clazz = classNames.get(clazzName);
                EsEvent evt = (EsEvent)mapper.readValue(data, clazz);
                result.add(evt);

            }
            return result;
        } catch (Exception ex) {
            throw new RuntimeException("Problem loading events", ex);
        } finally {
            try {
                if (conn != null) {
                    conn.close();
                }
            } catch (SQLException ex) {
                System.out.println(ex);
            }
        }
    }

    @Override
    public void saveEvent(UUID aggregateId, List<EsEvent> uncommittedChanges, long expectedVersion) throws AggregateConcurrencyException {
        List<EsEvent> data = getEventsForAggregate(aggregateId);

        if (!data.isEmpty()) {
            EsEvent last = data.get(data.size() - 1);
            if (last.getVersion() != expectedVersion) {
                throw new AggregateConcurrencyException();
            }
        }

        long version = expectedVersion+1;

        Connection conn = null;
        Savepoint savePoint = null;
        try {
            conn = createConnection();
            savePoint= conn.setSavepoint();
            conn.setAutoCommit(false);

            for(EsEvent uncommittedChange : uncommittedChanges){
                uncommittedChange.setVersion(version);
                String itemString = mapper.writeValueAsString(uncommittedChange);
                Statement stmt = conn.createStatement();
                String clazzName = uncommittedChange.getClass().getSimpleName();

                stmt.execute(
                        "INSERT INTO \"EVENT_STORE\" (id,version,clazz,data) VALUES ("
                                + "'" + uncommittedChange.getId() + "','"
                                +  + uncommittedChange.getVersion() + ",'"
                                + "'" + clazzName + "','"
                                + itemString + "';");
                version++;
            }
            conn.commit();
            conn.close();
        } catch (Exception ex) {
            try {
                if (conn != null) {
                    conn.rollback(savePoint);
                    conn.close();
                }
            }catch (Exception subEx){

            }
            throw new RuntimeException("Problem loading events", ex);
        }
    }

    @Override
    public void registerClass(Class messageType) {
        String name = messageType.getSimpleName().toLowerCase(Locale.ROOT);
        classNames.putIfAbsent(name,messageType);
    }

}
