import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.UUID;

/**
 * Tests for the DBConnection class.
 * In order to run this class, first run the following command so that
 * the JDBC driver can be found:
 * export CLASSPATH=$CLASSPATH:/home/ubuntu/mysql-connector-java-5.1.39-bin.jar
 */
public class TestDBConnection {

    public static void main(String[] args) {
        // Test connecting to database
        DBConnection dbConnection = DBConnection.GetInstance();
        String query;
        
        testSelect();
        
        
        // Test INSERT statement
        String uid = UUID.randomUUID().toString();
        query = "INSERT INTO UserInfo(UserID, Score1) VALUES('"
                + uid + "', 0)";
        dbConnection.executeUpdate(query);
        
        System.out.println();
        System.out.println("After INSERT statement");
        testSelect();
        
        
        // Test UPDATE statement
        query = "UPDATE UserInfo SET Score1 = 200 WHERE UserID = '"
                + uid + "'";
        dbConnection.executeUpdate(query);
        
        System.out.println();
        System.out.println("After UPDATE statement");
        testSelect();
        
        
        // Test DELETE statement
        query = "DELETE FROM UserInfo WHERE UserID = '" + uid + "'";
        dbConnection.executeUpdate(query);
        
        System.out.println();
        System.out.println("After DELETE statement");
        testSelect();
        
        
        // Test closing connection to database
        dbConnection.close();
    }

    // Test SELECT statement
    private static void testSelect() {
        DBConnection dbConnection = DBConnection.GetInstance();
        String query;
        
        query = "SELECT UserID,Score1 FROM UserInfo";
        ResultSet resultSet = dbConnection.executeQuery(query);
        
        // Print the results of the query
        try {
            while (resultSet.next()) {
                System.out.print("UserID:");
                System.out.print(resultSet.getString(1));
                System.out.print(", Score1:");
                System.out.println(resultSet.getInt(2));
            }
            
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
    
}
