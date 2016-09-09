import java.net.*;

// Derived from Computer Networking: A Top Down Approach, by Kurose and Ross
public class UDPServer {

    public static void main(String args[]) throws Exception {
        DatagramSocket serverSocket = new DatagramSocket(9876);
        byte[] receiveData = new byte[1024];
        byte[] sendData = new byte[1024];

        while (true) {
            // Receive packet and print sentence
            DatagramPacket receivePacket =
                    new DatagramPacket(receiveData, receiveData.length);
            serverSocket.receive(receivePacket);
            String sentence = new String(receivePacket.getData());
            System.out.println(sentence);

            // Repack and send back sentence
            InetAddress IPAddress = receivePacket.getAddress();
            int port = receivePacket.getPort();
            sendData = sentence.getBytes();
            DatagramPacket sendPacket =
                    new DatagramPacket(sendData, sendData.length, IPAddress,
                            port);

            serverSocket.send(sendPacket);
        }
    }
}
