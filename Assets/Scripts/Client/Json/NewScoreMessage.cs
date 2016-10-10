
// Class to store information of the score of
// a winning game and senderID. Used with
// Json.NET to send this information in JSON format.
public class NewScoreMessage
{
    public string type;
    public string senderID;
    public int score;
}
