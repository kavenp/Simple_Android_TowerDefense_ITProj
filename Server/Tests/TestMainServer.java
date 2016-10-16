import java.lang.reflect.Method;

/**
 * Tests for the MainServer class.
 * Requires the DBConnection and ScoreServer classes.
 * In order to run this class, first run the following command so that
 * the JDBC driver and JSON.simple library can be found:
 * export CLASSPATH=$CLASSPATH:/home/ubuntu/
 * mysql-connector-java-5.1.39-bin.jar:/home/ubuntu/json-simple-1.1.1.jar
 */
public class TestMainServer {

    public static void main(String[] args) throws Exception {
        
        MainServer mainServer = new MainServer();
        Method method;
        
        // Test private methods using reflection
        method = mainServer.getClass().getDeclaredMethod(
                "createUserEntry", String.class);
        method.setAccessible(true);
        
        // Create a new entry with UserID = "new user"
        method.invoke(mainServer, "new user");
        
        
        method = mainServer.getClass().getDeclaredMethod("userExists",
                String.class);
        method.setAccessible(true);
        
        // Test that user exists fails to find a non-existent user
        boolean userExists = (Boolean)method.invoke(mainServer,
                "non-existent user");
        if (userExists) {
            System.out.println("Error: Finds a non-existent user.");
        } else {
            System.out.println("Success: Does not find a non-existent user.");
        }
        
        // Test that user exists succeeds in finding the new user
        userExists = (Boolean)method.invoke(mainServer, "new user");
        if (!userExists) {
            System.out.println("Error: New user does not exist.");
        } else {
            System.out.println("Success: New user exists.");
        }
        
        
        // Clean up database
        DBConnection dbConnection = DBConnection.GetInstance();
        String query = "DELETE FROM UserInfo WHERE UserID = '"
                + "new user" + "'";
        dbConnection.executeUpdate(query);
        
        dbConnection.close();
    }

}
