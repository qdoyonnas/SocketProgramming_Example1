using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Threading;
using System.Text;

namespace Synchronous_Server_Example1
{
    public class SynchronousServer
    {
        private const int backLogCount = 10;
        private const int bufferSize = 1024;

        private Socket socket;
        private IPEndPoint endPoint;

        public SynchronousServer( string sPort )
        {
            try {
                int port = int.Parse(sPort);
                endPoint = new IPEndPoint(IPAddress.Any, port);

                socket = new Socket(endPoint.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                socket.EnableBroadcast = true;
                socket.Bind(endPoint);
            } catch( SocketException e ) {
                Console.WriteLine("Socket Exception: " + e);
                throw e;
            } catch( FormatException e ) {
                Console.WriteLine("Format Exception: " + e);
                throw e;
            } catch( SystemException e ) {
                Console.WriteLine("Unknown exception: " + e);
                throw e;
            }
        }
        ~SynchronousServer()
        {
            socket.Close();
        }

        public string ReceiveMessage()
        {
            byte[] buffer = new byte[bufferSize];
            try {
                socket.Receive(buffer);

                return Encoding.ASCII.GetString(buffer).Trim();

            } catch( FormatException e ) {
                Console.WriteLine("Format Exception: " + e);
                return "<Receive Failed>";
            } catch( SocketException e ) {
                Console.WriteLine("Socket Exception: " + e);
                return "<Receive Failed>";
            } catch( SystemException e ) {
                Console.WriteLine("Unknown exception: " + e);
                return "<Receive Failed>";
            }
        }

        static void Main()
        {
            Console.WriteLine("Setup Asynchronous Server:");
            Console.Write("Port: ");
            string sPort = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Creating UDP Asynchronous Server...");
            SynchronousServer server = new SynchronousServer(sPort);

            Console.WriteLine();
            Console.WriteLine("Listening on port " + sPort + "...");
            
            while( true ) {
                string msg = server.ReceiveMessage();
                Console.WriteLine("Message Received:");
                Console.WriteLine(msg);
            }
        }
    }
}
