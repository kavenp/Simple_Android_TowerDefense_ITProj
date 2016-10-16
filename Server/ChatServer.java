import java.net.*;
import java.util.*;

public class ChatServer {
	private HashMap<String, ArrayList<Tuple<String, InetAddress, Integer>>> roomConnections = 
			new HashMap<String,ArrayList<Tuple<String, InetAddress, Integer>>>(); 

	/**
	 * Adds a chat connection to the hashmap storing all chat connections
	 * If it is a new chat room it will create and add that connection to a new room
	 * otherwise it will add the connection to an existing room in the hashmap.
	 * @param userID ID of sender
	 * @param roomName chat room name that sender belongs to
	 * @param ipAddress IP address of message sender
	 * @param port Port used to send packet by sender
	 */
	public void addConnection(String userID, String roomName, InetAddress ipAddress, int port) {
		if(roomConnections.containsKey(roomName)) {
			//if room already exists in map
			Tuple<String, InetAddress, Integer> newConn = new Tuple<String, InetAddress, Integer> (userID, ipAddress, port);
			ArrayList<Tuple<String, InetAddress, Integer>> conns = roomConnections.get(roomName);
			if(!conns.contains(newConn)) {
				//only add connection to room if it is unique
				roomConnections.get(roomName).add(newConn);
			} 
		} else {
			//new room
			ArrayList<Tuple<String, InetAddress, Integer>> connList = new ArrayList<Tuple<String, InetAddress, Integer>>();
    		Tuple<String, InetAddress, Integer> newConn = new Tuple<String, InetAddress, Integer> (userID, ipAddress, port);
    		connList.add(newConn);
    		roomConnections.put(roomName,connList);
		}	
	}

	public ArrayList<Tuple<String, InetAddress, Integer>> getConnections(String room) {
		if(roomConnections.containsKey(room)) {
        	//get the all connections in a room if it exists in map
        	ArrayList<Tuple<String, InetAddress, Integer>> conns = roomConnections.get(room);
        	return conns;
		} else {
			//room doesn't exist
			return null;
		}
	}
		
}
