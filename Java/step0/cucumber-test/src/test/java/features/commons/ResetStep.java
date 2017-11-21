package features.commons;

import cucumber.api.java.en.Given;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.Statement;

public class ResetStep {


    private Connection createConnection() throws SQLException {
        String url = "jdbc:hsqldb:hsql://localhost/";
        String customer = "SA";
        String password = "";
        return DriverManager.getConnection(url,customer,password);
    }

    @Given("^Data tables cleaned$")
    public void data_tables_cleaned(){
        cleanUp("INVOICE");
        cleanUp("CUSTOMER");
        cleanUp("COMPLETEDINVOICE");
        cleanUp("INPROGRESSINVOICE");
    }

    private void cleanUp(String table){
        Connection conn = null;
        try {
            conn = createConnection();
            Statement stmt = conn.createStatement();
            stmt.execute(
                    "DELETE FROM \"" + table + "\";");
        } catch (Exception ex) {
            System.out.println(ex);
        } finally {
            try {
                if (conn != null) {
                    conn.close();
                }
            } catch (Exception ex) {

            }
        }
    }
}
