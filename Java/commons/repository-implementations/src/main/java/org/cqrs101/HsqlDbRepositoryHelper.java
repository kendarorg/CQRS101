package org.cqrs101;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.core.env.Environment;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.UUID;
import java.util.function.BiConsumer;
import java.util.function.Function;

public class HsqlDbRepositoryHelper implements RepositoryHelper {

    private static final ObjectMapper mapper = new ObjectMapper();
    private final Environment environment;
    private String name;
    private final String driverName = "org.hsqldb.jdbcDriver";
    private Class clazz;


    public HsqlDbRepositoryHelper(Environment environment){
        this.environment = environment;
    }

    private Connection createConnection() throws SQLException {
        String url = environment.getProperty("hsqldb.url");
        String customer = environment.getProperty("hsqldb.customer");
        String password = environment.getProperty("hsqldb.password");
        return DriverManager.getConnection(url,customer,password);
    }

    @Override
    public RepositoryHelper create(Class clazz) {
        Connection conn = null;
        this.clazz = clazz;

        try {
            Class.forName(driverName);
        } catch (Exception e) {
            throw new RuntimeException("Missing HSQLDB Driver");
        }
        HsqlDbRepositoryHelper helper = new HsqlDbRepositoryHelper(environment);
        helper.name = clazz.getSimpleName().toUpperCase(Locale.ROOT).toUpperCase(Locale.ROOT);
        try {
            conn = createConnection();
            Statement stmt = conn.createStatement();
            stmt.executeUpdate(
                    "CREATE TABLE \"" + helper.name + "\" ("
                    + "id UUID  NOT NULL, data varchar(680000) NOT NULL,"
                    + "PRIMARY KEY (id));");
            return helper;
        } catch (Exception ex) {
            return helper;
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
    public String getName() {
        return name;
    }

    @Override
    public Object getById(UUID id) {
        Connection conn = null;
        try {
            conn = createConnection();
            Statement stmt = conn.createStatement();
            ResultSet resultSet = stmt.executeQuery(
                    "SELECT data FROM \"" + this.name + "\" WHERE "
                    + "id ='" + id + "';");
            for (; resultSet.next();) {
                String data = resultSet.getString("data");
                return mapper.readValue(data, clazz);
            }
            return null;
        } catch (Exception ex) {
            throw new RuntimeException("Missing hslqDb Driver", ex);
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
    public List<Object> getAll() {
        Connection conn = null;
        try {
            List<Object> result = new ArrayList<Object>();
            conn = createConnection();
            Statement stmt = conn.createStatement();
            ResultSet resultSet = stmt.executeQuery(
                    "SELECT * FROM \"" + this.name + "\";");
            for (; resultSet.next();) {
                String data = resultSet.getString("data");
                Object target = mapper.readValue(data, clazz);
                result.add(target);
            }
            return result;
        } catch (Exception ex) {
            throw new RuntimeException("Missing hslqDb Driver", ex);
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
    public Object save(Object item, BiConsumer<Object, UUID> idSetter, Function<Object, UUID> idGetter) {
        Connection conn = null;
        try {
            boolean exists = false;
            conn = createConnection();
            UUID id = idGetter.apply(item);
            if (id != null) {
                Statement stmt = conn.createStatement();
                ResultSet resultSet = stmt.executeQuery(
                        "SELECT data FROM \"" + this.name + "\" WHERE "
                        + "id ='" + id + "';");
                for (; resultSet.next();) {
                    exists = true;
                    break;
                }
            } else {
                id = UUID.randomUUID();
                idSetter.accept(item, id);
            }
            String itemString = mapper.writeValueAsString(item);
            Statement stmt = conn.createStatement();
            if (!exists) {
                stmt.execute(
                        "INSERT INTO \"" + this.name + "\" (id,data) VALUES ("
                        + "'" + id + "','" + itemString + "';");
            } else {
                stmt.execute(
                        "UPDATE \"" + this.name + "\" set data = '" + itemString + "'"
                        + "WHERE id ='" + id + "';");
            }
            return item;
        } catch (Exception ex) {
            throw new RuntimeException("Missing hslqDb Driver", ex);
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
    public void delete(UUID id) {
        Connection conn = null;
        try {
            conn = createConnection();
            Statement stmt = conn.createStatement();
            stmt.execute(
                    "DELETE FROM \"" + this.name + "\" WHERE "
                    + "id ='" + id + "';");
        } catch (Exception ex) {
            throw new RuntimeException("Missing hslqDb Driver", ex);
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
}
