using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Threading;
using System.Text;

namespace Synchronous_Client_Example1
{
    public class SynchronousClient
    {
        private Socket socket;
        private IPEndPoint endPoint;

        public SynchronousClient( string sPort, string sAddress )
        {
            try {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.EnableBroadcast = true;

                IPAddress address = IPAddress.Parse(sAddress);
                int port = int.Parse(sPort);

                endPoint = new IPEndPoint(address, port);
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

        ~SynchronousClient()
        {
            socket.Close();
        }

        public void SendMessage( string msg )
        {
            byte[] buffer = Encoding.ASCII.GetBytes(msg);
            
            socket.SendTo(buffer, endPoint);
            Console.WriteLine();
            Console.WriteLine("Message sent to end point");
        }

        static void Main()
        {
            Console.WriteLine("Setup Asynchronous Client:");
            Console.Write("Port: ");
            string sPort = Console.ReadLine();
            Console.Write("Address: ");
            string sAddress = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Creating UDP Asynchronous Client...");
            SynchronousClient client = new SynchronousClient(sPort, sAddress);

            string input = "";
            while( true )
            {
                Console.WriteLine();
                Console.WriteLine("Message to send (q to quit):");
                Console.Write("> ");
                input = Console.ReadLine();

                if( input == "q" ) {
                    return;
                }
                client.SendMessage(input);
            }
        }
    }
}