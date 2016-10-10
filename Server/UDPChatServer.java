import java.io.IOException;
import java.net.*;
import java.util.*;
import org.json.simple.*;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

public class UDPChatServer {

	private static HashMap<String, ArrayList<Tuple<String, InetAddress, Integer>>> roomConnections = 
			new HashMap<String,ArrayList<Tuple<String, InetAddress, Integer>>>(); 
	private String senderID = "";
	private String roomName = "";
    
    public void run() {
    	DatagramSocket serverSocket = null;
		try {
			serverSocket = new DatagramSocket(9876);
		} catch (SocketException e1) {
			e1.printStackTrace();
		}
        byte[] receiveData = new byte[1024];
        byte[] sendData = new byte[1024];
        byte[] ackData = new byte[1024];

        while (true) {
            // Receive packet and print sentence
            DatagramPacket receivePacket =
                    new DatagramPacket(receiveData, receiveData.length);
			try {
				serverSocket.receive(receivePacket);
			} catch (IOException e) {
				e.printStackTrace();
			}
            String sentence = new String(receivePacket.getData(),
                    0, receivePacket.getLength());
            System.out.println(sentence);
            
            InetAddress IPAddress = receivePacket.getAddress();
            int port = receivePacket.getPort();
            
            if (checkInitConnection(sentence)) 
            //if received is an initConnection message
            {
            	if (roomConnections.containsKey(roomName)) 
            	//if room is already in map just add in the new connection
            	//and send ack packet
            	{
            		Tuple<String, InetAddress, Integer> newConn = new Tuple<String, InetAddress, Integer> (senderID, IPAddress, port);
            		roomConnections.get(roomName).add(newConn);
            		JSONObject obj = new JSONObject();
            		obj.put("senderID", "server");
            		obj.put("roomID", roomName);
            		obj.put("message","");
        			//reset roomName variable for next connection
        			this.roomName = null;
            		System.out.println(obj.toJSONString());
            		ackData = obj.toJSONString().getBytes();
            		DatagramPacket ackPacket = 
            				new DatagramPacket(ackData, ackData.length, IPAddress, port);
            		try {
						serverSocket.send(ackPacket);
					} catch (IOException e) {
						e.printStackTrace();
					}
            	} else {
            		ArrayList connList = new ArrayList();
            		Tuple newConn = new Tuple<String, InetAddress, Integer> (senderID, IPAddress, port);
            		connList.add(newConn);
            		roomConnections.put(roomName,connList);
            		JSONObject obj = new JSONObject();
            		obj.put("senderID", "server");
            		obj.put("roomID", roomName);
            		obj.put("message","");
        			//reset roomName variable for next connection
        			this.roomName = null;
            		System.out.println(obj.toJSONString());
            		ackData = obj.toJSONString().getBytes();
            		DatagramPacket ackPacket =
            				new DatagramPacket(ackData, ackData.length, IPAddress, port);
            		try {
						serverSocket.send(ackPacket);
					} catch (IOException e) {
						e.printStackTrace();
					}
            	}
            }
            
            String msg = getMsg(sentence);
            String room = getRoom(sentence);
            
            if(roomConnections.containsKey(room)) {
            	//get the all connections in a room
            	ArrayList<Tuple<String, InetAddress, Integer>> conns = roomConnections.get(room);
            	System.out.println(conns);
        		sendData = sentence.getBytes();
        		//create and send a packet to each connection
            	for (Tuple<String, InetAddress, Integer> temp : conns) {
            		DatagramPacket sendPacket = 
            				new DatagramPacket(sendData, sendData.length, temp.y, temp.z.intValue());
            		System.out.println("IP = "+temp.y +" Port = " +temp.z + "\n");
            		try {
						serverSocket.send(sendPacket);
					} catch (IOException e) {
						e.printStackTrace();
					}
            	}
            }
        }
    }
    
    public String getRoom(String jsonStr) {
    	JSONParser parser = new JSONParser();
    	try {
    		Object obj = parser.parse(jsonStr);
    		JSONObject jsonObject = (JSONObject) obj;
    		String room = (String) jsonObject.get("roomID");
    		return room;
    	} catch (ParseException e) {
    		return "";
    	}
    }
    
    public String getMsg(String jsonStr) {
    	JSONParser parser = new JSONParser();
    	try {
    		Object obj = parser.parse(jsonStr);
    		JSONObject jsonObject = (JSONObject) obj;
    		String msg = (String) jsonObject.get("message");
    		return msg;
    	} catch (ParseException e) {
    		return "";
    	}
    }
    
    public boolean checkInitConnection(String jsonStr) {
    	JSONParser parser = new JSONParser();
    	
    	try {
    		Object obj = parser.parse(jsonStr);
    		JSONObject jsonObject = (JSONObject) obj;
    		//get parsed values and store them
    		this.roomName =  (String) jsonObject.get("roomName");
    		this.senderID = (String) jsonObject.get("senderID");
    		if(roomName != null) {
    			return true;
    		} else {
    			return false;
    		}
    		
    	} catch (ParseException e) {
    		//exception happens when there is no roomName attribute in jsonObject
    		//which means this is a chat message, not an initial connection
    		return false;
    	}
    } 
    
    public class Tuple<X,Y,Z> {
    	public final X x;
    	public final Y y;
    	public final Z z;
    	
    	public Tuple(X x, Y y, Z z) {
    		this.x = x;
    		this.y = y;
    		this.z = z;
    	}
    }
    
}
