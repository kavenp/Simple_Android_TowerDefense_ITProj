import java.io.IOException;
import java.io.StringWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;


/**
 * Manages the user information on the
 * database. Deciphers messages requesting
 * high scores, informing of new scores and
 * room chat.
 */
public class MainServer {

    /** Interface to the database. */
    DBConnection dbConnection;
    
    /** Manages the score information on the database. */
    ScoreServer scoreServer;
    
    /** Socket for sending and receiving packets. */
    DatagramSocket serverSocket = null;
    
    /** Manages in-game chat */
    ChatServer chatServer;
    
    /**
     * Creates a new MainServer object.
     */
    public MainServer() {
        dbConnection = DBConnection.GetInstance();
        scoreServer = new ScoreServer();
        chatServer = new ChatServer();
        
        try {
            serverSocket = new DatagramSocket(9876);
        } catch (SocketException e) {
            e.printStackTrace();
        }
    }
    
    /**
     * Runs a loop that waits for messages from clients.
     */
    public void run() {
        byte[] receiveData = new byte[1024];
        
        while (true) {
            // Receive packet and determine the type of information
            // in the message
            DatagramPacket receivePacket =
                    new DatagramPacket(receiveData, receiveData.length);
            
            try {
                serverSocket.receive(receivePacket);
                
            } catch (IOException e) {
                e.printStackTrace();
                continue;
            }
            
            InetAddress ipAddress = receivePacket.getAddress();
            int port = receivePacket.getPort();
            String message = new String(receivePacket.getData(),
                    0, receivePacket.getLength());
            
            processMessage(message, ipAddress, port);
        }
    }
    
    /**
     * Processes the client's message based on the type key.
     * Creates a new entry for the user if the user cannot be found.
     * @param message The message sent by a client.
     * @param ipAddress The IP address where the message originated.
     * @param port The port used by the message's packet.
     */
    private void processMessage(String message, InetAddress ipAddress,
            int port) {
        JSONParser parser = new JSONParser();
        
        try {
            // Testing only
            System.out.println(message);
            
            Object obj = parser.parse(message);
            JSONObject jsonObject = (JSONObject)obj;
            
            String userID = (String)jsonObject.get("senderID");
            if (!userExists(userID)) {
                createUserEntry(userID);
            }
            
            String type = (String)jsonObject.get("type");
            
            if (type == null) {
                System.err.println("Warning: " +
                		"Received message with no type key.");
                return;
            }
            
            if (type.equals("NewScore")) {
                long newScore = (Long)jsonObject.get("score");
                scoreServer.updateScores(userID, (int)newScore);
                
            } else if (type.equals("GetScores")) {
                sendScores(userID, ipAddress, port);
                
            } else if (type.equals("roomInfo")) {
                // roomInfo acknowledgement
            	String roomName = (String)jsonObject.get("roomName");
            	chatServer.addConnection(userID, roomName, ipAddress, port);
            	sendRoomAck(roomName, ipAddress, port);
            } else if (type.equals("message")) {
            	// a chat message
            	String room = (String)jsonObject.get("roomID");
            	ArrayList<Tuple<String, InetAddress, Integer>> conns = chatServer.getConnections(room);
	            if(conns != null) {
	            	//a check to make sure that connections list is not null
            		for(Tuple<String, InetAddress, Integer> temp : conns) {
	            		//sends the received message to all connections in the room
	            		sendChatMessage(message, temp.y, temp.z.intValue());
	            	}
	            } 	
            } else {
            	//some unexpected type
            }
            
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }
    
    /**
     * Sends JSON message that contains the roomName as an acknowledgement from chat server
     * stating that the room has already been created
     * @param roomName the chat room name
     * @param ipAddress IP address of the original sender
     * @param port Port used by sender to send packet
     */
    private void sendRoomAck(String roomName, InetAddress ipAddress, int port) {
    	byte[] ackData = new byte[1024];
    	JSONObject obj = new JSONObject();
		obj.put("senderID", "server");
		obj.put("roomID", roomName);
		obj.put("message","");
		ackData = obj.toJSONString().getBytes();
		DatagramPacket ackPacket =
				new DatagramPacket(ackData, ackData.length, ipAddress, port);
		try {
			serverSocket.send(ackPacket);
		} catch (IOException e) {
			e.printStackTrace();
		}
    }
    
    /**
     * Sends JSON chat message to the given ipAddress and port
     * @param message Message containing all necessary details for chat
     * @param ipAddress IP address of recipient
     * @param port Port for recipient's packet
     */
    private void sendChatMessage(String message, InetAddress ipAddress, int port) {
    	byte[] sendData = new byte[1024];
    	sendData = message.getBytes();
    	DatagramPacket sendPacket =
                new DatagramPacket(sendData, sendData.length, ipAddress, port);
    	 try {
             serverSocket.send(sendPacket);
         } catch (IOException e) {
             e.printStackTrace();
         }
    }
    
    /**
     * Sends a JSON message containing the high scores of the user.
     * @param userID A unique identifier for the user's phone.
     * @param ipAddress The IP address where the message originated.
     * @param port The port used by the message's packet.
     */
    private void sendScores(String userID, InetAddress ipAddress, int port) {
        String message = createScoresMessage(userID);
        byte[] sendData = message.getBytes();
        
        DatagramPacket sendPacket =
            new DatagramPacket(sendData, sendData.length, ipAddress, port);
        
        try {
            serverSocket.send(sendPacket);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    
    /**
     * Creates a JSON message containing the high scores of the user.
     * @param userID A unique identifier for the user's phone.
     * @return A JSON string.
     */
    @SuppressWarnings("unchecked")
    private String createScoresMessage(String userID) {
        
        int[] scores = scoreServer.retrieveScores(userID);
        
        JSONObject obj = new JSONObject();
        obj.put("Score1", new Integer(scores[0]));
        obj.put("Score2", new Integer(scores[1]));
        obj.put("Score3", new Integer(scores[2]));
        
        StringWriter out = new StringWriter();
        try {
            obj.writeJSONString(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
        
        return out.toString();
    }
    
    /**
     * True if the user exists in the database.
     * @param userID A unique identifier for the user's phone.
     * @return True if the user can be found.
     */
    private boolean userExists(String userID) {
        String query = "SELECT Score1 FROM UserInfo "
                + "WHERE UserID = '" + userID + "'";
        ResultSet resultSet = dbConnection.executeQuery(query);
        
        try {
            if (resultSet.next()) {
                return true;
            }
            
        } catch (SQLException e) {
            e.printStackTrace();
        }
        
        return false;
    }
    
    /**
     * Creates a new entry in the database for a user.
     * Initialises the scores to zero.
     * @param userID A unique identifier for the user's phone.
     */
    private void createUserEntry(String userID) {
        String query = "INSERT INTO UserInfo(UserID, Score1, Score2, Score3)"
                + " VALUES('" + userID + "', 0, 0, 0)";
        
        dbConnection.executeUpdate(query);
    }
}
