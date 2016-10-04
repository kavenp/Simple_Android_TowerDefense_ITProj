import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.UUID;

/**
 * Tests for the ScoreServer class.
 * Requires the DBConnection class.
 * In order to run this class, first run the following command so that
 * the JDBC driver can be found:
 * export CLASSPATH=$CLASSPATH:/home/ubuntu/mysql-connector-java-5.1.39-bin.jar
 */
public class TestScoreServer {

    public static void main(String[] args) {
        
        ScoreServer scoreServer = new ScoreServer();
        DBConnection dbConnection = DBConnection.GetInstance();
        String query;
        
        // Create a test user
        String uid = UUID.randomUUID().toString();
        query = "INSERT INTO UserInfo(UserID, Score1, Score2, Score3) "
                + "VALUES('" + uid + "', 800, 400, 200)";
        dbConnection.executeUpdate(query);
        
        System.out.println("before score update");
        int[] scores;
        scores = scoreServer.retrieveScores(uid);
        printScores(scores);
        
        
        // New second high score
        scoreServer.updateScores(uid, 500);
        
        System.out.println();
        System.out.println("after score update");
        scores = scoreServer.retrieveScores(uid);
        printScores(scores);
        
        // Print whether the test succeeded
        System.out.println();
        testScore2Equals(500, uid);
        
        
        // Clean up database
        query = "DELETE FROM UserInfo WHERE UserID = '" + uid + "'";
        dbConnection.executeUpdate(query);
        
        dbConnection.close();
    }
    
    // Prints the scores for the user.
    private static void printScores(int[] scores) {
        
        for (int i = 0; i < scores.length; i++) {
            System.out.println("Score" + (i+1) + " = " + scores[i] + " ");
        }
    }
    
    // Tests whether Score2 for the user equals the expected score,
    // printing the result.
    private static void testScore2Equals(int score, String uid) {
        
        DBConnection dbConnection = DBConnection.GetInstance();
        String query;
        
        query = "SELECT Score2 FROM UserInfo "
                + "WHERE UserID = '" + uid + "'";
        ResultSet resultSet = dbConnection.executeQuery(query);
        
        int score2 = -1;
        try {
            while (resultSet.next()) {
                score2 = resultSet.getInt(1);
            }
            
        } catch (SQLException e) {
            e.printStackTrace();
        }
        
        if (score2 == score) {
            System.out.println("Success.");
        } else {
            System.out.println("Fail.");
        }
    }

}
