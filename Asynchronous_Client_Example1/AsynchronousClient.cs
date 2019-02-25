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

        private int remotePort;
        private IPAddress address;

        private Socket socket;
        private IPEndPoint endPoint;

        private int localPort;
        private Socket listener;
		EndPoint localEndPoint;

        public AsynchronousClient()
        {
			identity = new Random().Next();
        }

        public bool Connect( string sAddress, string sRemotePort, string sLocalPort )
        {
            readyToSend = false;

            // Parse IP address
            try {  
                address = IPAddress.Parse(sAddress);
            } catch (FormatException e) {  
                Console.WriteLine("Format Exception: " + e);  
                return readyToSend = false;
            }

            // Parse remote port
            try {  
                remotePort = int.Parse(sRemotePort);
            } catch (FormatException e) {  
                Console.WriteLine("Format Exception: " + e);  
                return readyToSend = false;
            }

            // Parse local port
            try {  
                localPort = int.Parse(sLocalPort);
            } catch (FormatException e) {  
                Console.WriteLine("Format Exception: " + e);  
                return readyToSend = false;
            }

            // Set up local end point and bind
            try {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                localEndPoint = new IPEndPoint(IPAddress.Any, localPort);
                listener.Bind(localEndPoint);
            } catch( SocketException e ) {
                Console.WriteLine( "Socket Exception: " + e.SocketErrorCode );
                Console.WriteLine( e );
                return readyToSend = false;
            } catch( SystemException e ) {
                Console.WriteLine("Unknown exception: " + e);
                return readyToSend = false;
            }

            // Set up remote socket
            try {
                socket = new Socket(address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                endPoint = new IPEndPoint(address, remotePort);
            } catch( SocketException e ) {
                Console.WriteLine( "Socket Exception: " + e.SocketErrorCode );
                Console.WriteLine( e );
                return readyToSend = false;
            } catch( SystemException e ) {
                Console.WriteLine("Unknown exception: " + e);
                return readyToSend = false;
            }

			readyToSend = true;

            long timeStamp = DateTime.UtcNow.Ticks;
			SendMessage( string.Format("connect {0} {1} {2} {3}<EOF>", identity, timeStamp, "127.0.0.1", localPort) );

			Console.WriteLine("Successfully connected");
            return true;
        }
        public void Disconnect()
        {
			SendMessage( string.Format("disconnect {0}<EOF>", identity), true );
        }

		public bool StartListening()
		{
			if( !readyToSend ) { return false; }

			StateObject state = new StateObject();
			state.workSocket = listener;

			listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback( ReceiveCallback ), state);

			return true;
		}
		void ReceiveCallback( IAsyncResult result )
		{
			StateObject state = (StateObject)result.AsyncState;
            Socket workingSocket = state.workSocket;

            try {
			    int bytesRead = workingSocket.EndReceive(result);
			    if( bytesRead > 0 ) {
				    state.sb.Append( Encoding.ASCII.GetString(state.buffer) );
				    string content = state.sb.ToString();
				    int eofIndex = content.IndexOf("<EOF>");
				    if( eofIndex > -1 ) {
					    content = content.Substring(0, eofIndex);
                        Interpret(content);
					    //Console.WriteLine("Received characters: " + content);

					    StartListening();
				    } else {
					    workingSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback( ReceiveCallback ), state);
				    }
			    }
            } catch( ObjectDisposedException e ) {
                return;
            }

		}

		void Interpret( string content )
		{
			string[] segments = content.Split(' ');
			if( segments.Length < 2 ) { return; }

			string messageType = segments[0];

			int remoteIdentity;
            try {
                remoteIdentity = int.Parse(segments[1]);
            } catch( FormatException e ) {
                Console.WriteLine("Failed to parse received message: " + segments);
                return;
            } catch( SystemException e ) {
                Console.WriteLine("Unexpected error: " + e);
                return;
            }

			if( remoteIdentity == identity ) { return; }

			switch( messageType ) {
				case "connect":
					HandleConnect( remoteIdentity, segments );
					break;
				case "disconnect":
					HandleDisconnect( remoteIdentity, segments );
					break;
				case "update":
					HandleUpdate( remoteIdentity, segments );
					break;
			}

		}
		void HandleConnect( int remoteIdentity, string[] segments )
		{
			if( segments.Length < 3 ) { return; }

			long timeStamp;
            try {
                timeStamp = long.Parse(segments[2]);
            } catch( FormatException e ) {
                Console.WriteLine("Failed to parse received message: " + segments);
                return;
            } catch( SystemException e ) {
                Console.WriteLine("Unexpected error: " + e);
                return;
            }

			Program.form.ConnectPlayer( remoteIdentity, timeStamp );
		}
		void HandleDisconnect( int remoteIdentity, string[] segments )
		{
			Program.form.DisconnectPlayer( remoteIdentity );
		}
        void HandleUpdate( int remoteIdentity, string[] segments )
        {
            if( segments.Length < 5 ) { return; }

            long timeStamp;
            int x, y;
            try {
                timeStamp = long.Parse(segments[2]);
                x = int.Parse(segments[3]);
                y = int.Parse(segments[4]);
            } catch( FormatException e ) {
                Console.WriteLine("Failed to parse received message: " + segments);
                return;
            } catch( SystemException e ) {
                Console.WriteLine("Unexpected error: " + e);
                return;
            }

            if( remoteIdentity == identity ) { return; }
            Program.form.UpdatePlayer( remoteIdentity, timeStamp, x, y );
        }

        public bool SendMessage( string message, bool closeAfterSending = false )
        {
            if( !readyToSend ) { return false; }

            StateObject state = new StateObject();
            state.buffer = Encoding.ASCII.GetBytes(message);
            state.workSocket = socket;
            
            socket.BeginSendTo(state.buffer, 0, state.buffer.Length, 0, endPoint, 
                    new AsyncCallback(SendCallback), state);

			if( closeAfterSending ) {
				socket.Close(5);
				listener.Close(5);
			}

            return true;
        }
        public bool SendPosition( Point pos )
        {
            if( !readyToSend ) { return false; }

            long timeStamp = DateTime.UtcNow.Ticks;

			string message = string.Format( "update {0} {1} {2} {3}<EOF>", identity, timeStamp, pos.X, pos.Y );
			//Console.WriteLine( "Sent: " + message );
			SendMessage(message);

            return true;
        }

        void SendCallback( IAsyncResult result )
        {
			try {
				StateObject state = (StateObject)result.AsyncState;
				Socket workingSocket = state.workSocket;

				int bytesSent = workingSocket.EndSendTo(result);
			} catch( ObjectDisposedException e ) {
				return;
			}
        }
    }
}
