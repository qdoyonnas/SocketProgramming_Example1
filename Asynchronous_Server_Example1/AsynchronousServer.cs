using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Asynchronous_Server_Example1
{
	public class StateObject
    {  
        public Socket workSocket = null;  
        public const int BufferSize = 256;  
        public byte[] buffer = new byte[BufferSize];  
        public StringBuilder sb = new StringBuilder();  
    } 

	public class Client
	{
		public readonly int identity;
		public long timeStamp;
		public readonly IPEndPoint endPoint;

		public Client(int identity, long timeStamp, IPEndPoint endPoint)
		{
			this.identity = identity;
			this.timeStamp = timeStamp;
			this.endPoint = endPoint;
		}
	}

	public class AsynchronousServer
	{
		public bool readyToSend = false;

		private Socket socket;
		private List<Client> clients = new List<Client>();

		private Socket listener;
		private int localPort;
		private EndPoint localEndPoint;

		public AsynchronousServer()
		{

		}

		public bool Start( string sPort )
		{
			readyToSend = false;

			// Parse string sPort into int localPort
			try {
				localPort = int.Parse(sPort);
			} catch( FormatException e ) {
				Console.WriteLine( "Server could not parse port number: {0}", sPort );
				Console.WriteLine(e);
				return false;
			}

			try {
				listener = new Socket( AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
				localEndPoint = new IPEndPoint(IPAddress.Any, localPort );
				listener.Bind(localEndPoint);
			} catch( SocketException e ) {
				Console.WriteLine("Socket Exception: " + e);
				return false;
			}

			try {
				socket = new Socket( AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
			} catch( SocketException e ) {
				Console.WriteLine("Socket Exception: " + e);
				return false;
			}

			readyToSend = true;
			StartListening();

			return true;
		}
		public void Stop()
		{
			listener.Close(5);
			socket.Close(5);
		}

		void StartListening()
		{
			if( !readyToSend ) { return; }

			StateObject state = new StateObject();
			state.workSocket = listener;

			listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback( ReceiveCallback ), state);
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
						Console.WriteLine("Received Message: " + content);
                        Interpret(content);

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
				Console.WriteLine("failed to parse received message: " + segments);
				Console.WriteLine(e);
				return;
			}

			switch( messageType ) {
				case "connect":
					HandleConnect( remoteIdentity, segments );
					break;
				case "disconnect":
					HandleDisconnect( remoteIdentity, segments );
					break;
				default:
					HandleOther( remoteIdentity, segments );
					break;
			}

			Relay( remoteIdentity, content );
		}
		void HandleConnect( int remoteIdentity, string[] segments )
		{
			if( segments.Length < 5 ) { return; }

			long timeStamp;
			IPAddress address;
			int port;
			try {
				timeStamp = long.Parse(segments[2]);
				address = IPAddress.Parse(segments[3]);
				port = int.Parse(segments[4]);
			} catch( FormatException e ) {
				Console.WriteLine("failed to parse received message: " + segments);
				Console.WriteLine(e);
				return;
			}

			Client subjectClient = null;
			foreach( Client client in clients ) {
				if( client.identity == remoteIdentity ) {
					subjectClient = client;
					break;
				}
			}
			if( subjectClient == null ) {
				subjectClient = new Client( remoteIdentity, timeStamp, new IPEndPoint(address, port) );
				clients.Add(subjectClient);
				Console.WriteLine("Player connected");
			} else {
				subjectClient.timeStamp = timeStamp;
			}
		}
		void HandleDisconnect( int remoteIdentity, string[] segments )
		{
			Client subjectClient = null;
			foreach( Client client in clients ) {
				if( client.identity == remoteIdentity ) {
					subjectClient = client;
					break;
				}
			}
			if( subjectClient != null ) {
				clients.Remove(subjectClient);
				Console.WriteLine("Player disconnected");
			}
		}
		void HandleOther( int remoteIdentity, string[] segments )
		{
			if( segments.Length < 3 ) { return; }

			long timeStamp;
			try {
				timeStamp = long.Parse(segments[2]);
			} catch( FormatException e ) {
				Console.WriteLine("failed to parse received message: " + segments);
				Console.WriteLine(e);
				return;
			}

			Client subjectClient = null;
			foreach( Client client in clients ) {
				if( client.identity == remoteIdentity ) {
					subjectClient = client;
					break;
				}
			}
			if( subjectClient != null ) {
				subjectClient.timeStamp = timeStamp;
			}
		}

		void Relay( int remoteIdentity, string content )
		{
			if( !readyToSend ) { return; }

			content += "<EOF>";

			StateObject state = new StateObject();
			state.buffer = Encoding.ASCII.GetBytes(content);
			state.workSocket = socket;

			foreach( Client client in clients ) {
				if( client.identity == remoteIdentity ) { continue; }

				socket.BeginSendTo( state.buffer, 0, state.buffer.Length, 0, client.endPoint,
							new AsyncCallback( SendCallback ), state );
			}
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
