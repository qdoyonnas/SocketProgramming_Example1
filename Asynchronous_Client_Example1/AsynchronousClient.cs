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
		public int identity;
        public bool readyToSend = false;

        private int port;
        private IPAddress address;

        private Socket socket;
        private IPEndPoint endPoint;

		//private Socket listener;
		EndPoint localEndPoint;

        public AsynchronousClient()
        {
			identity = new Random().Next();

            try {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				//listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            } catch( SocketException e ) {
                Console.WriteLine("Socket Exception: " + e);
                throw e;
            }
        }

        public bool Connect( string sAddress, string sPort )
        {
            readyToSend = false;
			if( socket.Connected ) { socket.Disconnect(true); }

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
				localEndPoint = new IPEndPoint(IPAddress.Any, port);
				socket.Bind(localEndPoint);
            } catch( SystemException e ) {
                Console.WriteLine("Unknown exception: " + e);
                return readyToSend = false;
            }

			Console.WriteLine("Successfully connected");
            return readyToSend = true;
        }

		public bool StartListening()
		{
			if( !readyToSend ) { return false; }

			StateObject state = new StateObject();
			state.workSocket = socket;

			socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback( ReceiveCallback ), state);

			return true;
		}
		void ReceiveCallback( IAsyncResult result )
		{
			StateObject state = (StateObject)result.AsyncState;
			Socket workingSocket = state.workSocket;

			int bytesRead = workingSocket.EndReceive(result);
			if( bytesRead > 0 ) {
				state.sb.Append( Encoding.ASCII.GetString(state.buffer) );
				string content = state.sb.ToString();
				int eofIndex = content.IndexOf("<EOF>");
				if( eofIndex > -1 ) {
					content = content.Substring(0, eofIndex);
					//Console.WriteLine("Received characters: " + content);
					StartListening();
				} else {
					workingSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback( ReceiveCallback ), state);
				}
			}

		}

        public bool SendMessage( string message )
        {
            if( !readyToSend ) { return false; }

            StateObject state = new StateObject();
            state.buffer = Encoding.ASCII.GetBytes(message);
            state.workSocket = socket;
            
            socket.BeginSendTo(state.buffer, 0, state.buffer.Length, 0, endPoint, 
                    new AsyncCallback(SendCallback), state);

            return true;
        }
        public bool SendPosition( Point pos )
        {
            if( !readyToSend ) { return false; }

            long timeStamp = DateTime.UtcNow.Ticks;

			string message = identity + " " + timeStamp + " " + pos.X + " " + pos.Y + "<EOF>";
			//Console.WriteLine( "Sent: " + message );
			SendMessage(message);

            return true;
        }

        void SendCallback( IAsyncResult result )
        {
            StateObject state = (StateObject)result.AsyncState;
            Socket workingSocket = state.workSocket;

            int bytesSent = workingSocket.EndSendTo(result);
        }
    }
}
