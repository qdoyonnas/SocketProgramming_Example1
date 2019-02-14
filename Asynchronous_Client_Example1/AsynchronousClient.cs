using System;
using System.Drawing;
using System.Net;  
using System.Net.Sockets;
using System.Text;

namespace Asynchronous_Client_Example1
{
    public class StateObject
    {  
        public Socket workSocket = null;  
        public const int BufferSize = 256;  
        public byte[] buffer = new byte[BufferSize];  
        public StringBuilder sb = new StringBuilder();  
    }  

    public class AsynchronousClient
    {
        public bool readyToSend = false;
        

        private int port;
        private IPAddress address;

        private Socket socket;
        private IPEndPoint endPoint;

        public AsynchronousClient()
        {
            try {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            } catch( SocketException e ) {
                Console.WriteLine("Socket Exception: " + e);
                throw e;
            }
        }

        public bool Connect( string sAddress, string sPort )
        {
            readyToSend = false;

            // Parse port from string to int
            try {
                port = int.Parse(sPort);
            } catch( FormatException e ) {
                Console.WriteLine("Format Exception: " + e);
                return readyToSend = false;
            }

            // Parse IP address
            try {  
                address = IPAddress.Parse(sAddress);
            } catch (FormatException e) {  
                Console.WriteLine("Format Exception: " + e);  
                return readyToSend = false;
            }

            // Set up end point
            try {
                endPoint = new IPEndPoint(address, port);
            } catch( SystemException e ) {
                Console.WriteLine("Unknown exception: " + e);
                return readyToSend = false;
            }

            return readyToSend = true;
        }

        public bool SendMessage( string message )
        {
            if( !readyToSend ) { return false; }

            StateObject state = new StateObject();
            state.buffer = Encoding.ASCII.GetBytes(message);
            state.workSocket = socket;
            
            socket.BeginSendTo(state.buffer, 0, StateObject.BufferSize, 0, endPoint, 
                    new AsyncCallback(SendCallback), state);

            return true;
        }
        public bool SendPosition( Point pos )
        {
            if( !readyToSend ) { return false; }

            long timeStamp = DateTime.UtcNow.Ticks;


            return true;
        }

        void SendCallback( IAsyncResult result )
        {
            StateObject state = (StateObject)result;
            Socket workingSocket = state.workSocket;

            int bytesSent = workingSocket.EndSendTo(result);
            Console.WriteLine("Sent {0} bytes to server", bytesSent);
        }
    }
}
