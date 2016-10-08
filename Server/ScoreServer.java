import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Arrays;

/**
 * Manages the score information on the
 * database. Contains methods to update the
 * scores based on a new score and retrieve
 * the top 3 scores.
 */
public class ScoreServer {
    
    /** Interface to the database. */
    DBConnection dbConnection;
    
    /**
     * Creates a new ScoreServer object.
     */
    public ScoreServer() {
        dbConnection = DBConnection.GetInstance();
    }
    
    /**
     * Updates the scores in the database if the new score
     * is one of the user's top 3 scores.
     * @param userID A unique identifier for the user's phone.
     * @param newScore The new score achieved by the user.
     */
    public void updateScores(String userID, int newScore) {
        
        int[] scores = retrieveScores(userID, newScore);
        
        if (scores[0] != newScore) {
            // New high score, update the user's entry
            // in the database
            String query = "UPDATE UserInfo SET "
                    + "Score1 = " + scores[3] + ", "
                    + "Score2 = " + scores[2] + ", "
                    + "Score3 = " + scores[1] + " "
                    + "WHERE UserID = '" + userID + "'";
            
            dbConnection.executeUpdate(query);
        }
    }
    
    /**
     * Retrieves the top 3 scores of the user and sorts them
     * with the new score in ascending order.
     * @param userID A unique identifier for the user's phone.
     * @param newScore The new score achieved by the user.
     * @return The sorted scores.
     */
    private int[] retrieveScores(String userID, int newScore) {
        
        String query = "SELECT Score1,Score2,Score3 FROM UserInfo "
                + "WHERE UserID = '" + userID + "'";
        ResultSet resultSet = dbConnection.executeQuery(query);
        
        int[] scores = {0, 0, 0, 0};
        scores[0] = newScore;
        
        try {
            while (resultSet.next()) {
                scores[1] = resultSet.getInt(1);
                scores[2] = resultSet.getInt(2);
                scores[3] = resultSet.getInt(3);
            }
            
        } catch (SQLException e) {
            e.printStackTrace();
        }
        
        Arrays.sort(scores);
        
        return scores;
    }
    
    /**
     * Retrieves the top 3 scores of the user, in descending
     * order.
     * @param userID A unique identifier for the user's phone.
     * @return The top 3 scores.
     */
    public int[] retrieveScores(String userID) {
        
        String query = "SELECT Score1,Score2,Score3 FROM UserInfo "
                + "WHERE UserID = '" + userID + "'";
        ResultSet resultSet = dbConnection.executeQuery(query);
        
        int[] scores = {0, 0, 0};
        try {
            while (resultSet.next()) {
                scores[0] = resultSet.getInt(1);
                scores[1] = resultSet.getInt(2);
                scores[2] = resultSet.getInt(3);
            }

        } catch (SQLException e) {
            e.printStackTrace();
        }
        
        return scores;
    }
}
