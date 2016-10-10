import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

/**
 * Acts as an interface to a database.
 * To configure the database to connect to, the url,
 * username and password fields need to be manually changed.
 */
public class DBConnection {
    
    /**
     * Address of the MySQL database.
     * The database on the same machine, using port 3306.
     * The name of the database is td.
     */
    private String url = "jdbc:mysql://localhost:3306/td";
    
    /** Name of the user with access to the database. */ 
    private String username = "root";
    /** No password set for the database. */
    private String password = "";
    
    /** Object that establishes a connection with a database. */
    private Connection connection = null;
    /** Object to execute queries. */
    private Statement statement = null;
    
    /** The only instance of this class. */
    private static DBConnection instance = new DBConnection();
    
    /**
     * Singleton. Establishes connections with the database.
     * Throws a runtime exception if database connection fails.
     */
    private DBConnection() {
        try {
            connection = DriverManager.getConnection(url, username, password);
            statement = connection.createStatement();

        } catch (SQLException e) {
            throw new IllegalStateException("Cannot connect the database", e);
        }
    }

    /**
     * Get the database connection object.
     * @return The database connection instance.
     */
    public static DBConnection GetInstance() {
        return instance;
    }
    
    /**
     * Wrapper for the namesake method in the java.sql.Statement class.
     * Prints stack trace on SQL exceptions.
     * @param sql An SQL statement that returns nothing,
     *            such as an INSERT, UPDATE, DELETE.
     */
    public void executeUpdate(String sql) {
        try {
            statement.executeUpdate(sql);
            
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
    
    /**
     * Wrapper for the namesake method in the java.sql.Statement class.
     * Prints stack trace on SQL exceptions.
     * @param sql An SQL query, such as a SELECT.
     * @return A ResultSet that contains the data returned by the SQL query.
     *         Returns null on SQL exceptions.
     */
    public ResultSet executeQuery(String sql) {
        ResultSet resultSet = null;
        try {
            resultSet = statement.executeQuery(sql);
            
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return resultSet;
    }
    
    /**
     * Closes all connections to the database.
     */
    public void close() {
        try {
            if (statement != null) {
                statement.close();
            }
            
            if (connection != null) {
                connection.close();
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
}
