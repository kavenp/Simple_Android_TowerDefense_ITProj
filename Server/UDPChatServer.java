import java.net.*;
import java.util.*;
import org.json.simple.*;

// Derived from Computer Networking: A Top Down Approach, by Kurose and Ross
public class UDPChatServer {

	private String senderID = "";
	private String roomName = "";
	private HashMap roomConnections = new HashMap(); 
	
    public static void main(String args[]) throws Exception {
        DatagramSocket serverSocket = new DatagramSocket(9876);
        byte[] receiveData = new byte[1024];
        byte[] sendData = new byte[1024];

        while (true) {
            // Receive packet and print sentence
            DatagramPacket receivePacket =
                    new DatagramPacket(receiveData, receiveData.length);
            serverSocket.receive(receivePacket);
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
            		Tuple newConn = new Tuple<String, InetAddress, int> (senderID, IPAddress, port);
            		roomConnections.get(roomName).add(newConn);
            		JSONObject obj = new JSONObject();
            		obj.put("senderID", "server");
            		obj.put("roomID", roomName);
            		obj.put("message","");
            		ackData = obj.toJSONString().getBytes();
            		DatagramPacket ackPacket = 
            				new DatagramPacket(ackData, ackData.length, IPAddress, port);
            		serversocket.send(ackPacket);
            	} else {
            		ArrayList connList = new ArrayList();
            		Tuple newConn = new Tuple<String, InetAddress, int> (senderID, IPAddress, port);
            		connList.add(newConn);
            		roomConnections.put(roomName,connList);
            		JSONObject obj = new JSONObject();
            		obj.put("senderID", "server");
            		obj.put("roomID", roomName);
            		obj.put("message","");
            		ackData = obj.toJSONString().getBytes();
            		DatagramPacket ackPacket =
            				new DatagramPacket(ackData, ackData.length, IPAddress, port);
            		serversocket.send(ackPacket);
            	}
            }
            
            String msg = getMsg(sentence);
            String room = getRoom(sentence);
            
            if(roomConnections.containsKey(room)) {
            	conns = roomConnections.get(room);
            	for (Tuple temp : conns) {
            		sendData = sentence.getBytes();
            		DatagramPacket sendPacket = 
            				new DatagramPacket(sendData, sendData.length, temp.y, temp.z);
            		serverSocket.send(sendPacket);
            	}
            }

            // Repack and send back sentence
            //sendData = sentence.getBytes();
            //DatagramPacket sendPacket =
            //        new DatagramPacket(sendData, sendData.length, IPAddress,
            //                port);

            //serverSocket.send(sendPacket);
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
    		this.roomName = (String) jsonObject.get("roomName");
    		this.senderID = (String) jsonObject.get("senderID");
    		return True;
    		
    	} catch (ParseException e) {
    		//exception happens when there is no roomName attribute in jsonObject
    		//which means this is a chat message, not a initial connection
    		return False;
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
