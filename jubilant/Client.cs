using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace jubilant
{
    public static class Client
    {
        private const int PORT = 36187;
        private static Socket client;

        //The response from the server
        private static String response = String.Empty;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);


        public static void StartClient()
        {
            //Connect to the server
            try
            {
                //Establish the remote endpoint for the socket
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Manager.ip);
                IPAddress iPAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(iPAddress, PORT);

                //Create TCP/IP socket
                client = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), null);
                connectDone.WaitOne();

                Package package = new Packages.Welcome();
                package.Send();
                sendDone.WaitOne();

                StartReceiving();
                receiveDone.WaitOne();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);
                connectDone.Set();

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private static void StartReceiving()
        {
            try
            {
                Debug.WriteLine("Waiting for a new message...");
                response = String.Empty;
                StateObject state = new StateObject();

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));


                    // All the data has arrived; put it in response.  
                    if (state.sb.ToString().IndexOf("<EOF>") > -1)
                    {

                        response = state.sb.ToString();
                        state.sb = new StringBuilder();

                        //We wait for the next connection
                        Debug.WriteLine($"Received: {response}");
                        Manager.HandlePackage(new Package(response));
                        receiveDone.Set();
                        StartReceiving();
                    }
                    else
                    {
                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }


        public static void Send(string data)
        {
            Debug.WriteLine("Sending " + data);
            Send(client, data);
        }

        private static void Send(Socket client, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Debug.WriteLine("Sent {0} bytes to server.", bytesSent);
                sendDone.Set();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public static void Disconnect()
        {
            Package disconnect = new Packages.Disconnect();
            disconnect.Send();
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public static bool Connected()
        {
            if (client == null) return false;
            return !((client.Poll(1000, SelectMode.SelectRead) && (client.Available == 0)) || !client.Connected);
        }
    }

    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
}
