package org.cqrs101;

import com.fasterxml.jackson.databind.ObjectMapper;
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
    private String name;
    private final String driverName = "org.hsqldb.jdbcDriver";
    private Class clazz;

    private Connection createConnection() throws SQLException {
        return DriverManager.getConnection("jdbc:hsqldb:hsql://localhost/testdb", "SA", "");
    }

    @Override
    public RepositoryHelper create(Class clazz) {
        Connection conn = null;
        this.clazz = clazz;
        try {
            Class.forName(driverName);
            HsqlDbRepositoryHelper helper = new HsqlDbRepositoryHelper();
            helper.name = clazz.getSimpleName().toUpperCase(Locale.ROOT);

            conn = createConnection();
            Statement stmt = conn.createStatement();
            stmt.executeUpdate(
                    "CREATE TABLE " + helper.name + " ("
                    + "id UUID  NOT NULL, data VARCHAR(MAX) NOT NULL,,"
                    + "PRIMARY KEY (id));");
            return helper;
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
                    "SELECT data FROM " + this.name + " WHERE "
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
                    "SELECT * FROM " + this.name + ";");
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
                        "SELECT data FROM " + this.name + " WHERE "
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
                        "INSERT INTO " + this.name + "(id,data) VALUES ("
                        + "'" + id + "','" + itemString + "';");
            } else {
                stmt.execute(
                        "UPDATE " + this.name + " set data = '" + itemString + "'"
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
                    "DELETE FROM " + this.name + " WHERE "
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
