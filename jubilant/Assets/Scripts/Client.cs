using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;


public class Client : MonoBehaviour
{

    // The port number for the remote device.  
    private const int port = 36187;
    private const int TIMEOUT = 5000;

    // The response from the remote device.
    private static String response = String.Empty;

    static Socket client;

    public static void StartClient()
    {
        // Connect to the server
        try
        {
            // Establish the remote endpoint for the socket.  
            // The name of the
            IPHostEntry ipHostInfo = Dns.GetHostEntry(GameManager.ip);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(remoteEP);
            StartReceiving();
        }
        catch (Exception e)
        {
            Debug.LogFormat(e.ToString());
        }
    }

    private static void StartReceiving()
    {
        try
        {
            Debug.Log("Waiting for incoming connection...");
            response = String.Empty;
            // Create the state object.  
            StateObject state = new StateObject();

            // Begin receiving the data from the remote device.  
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.LogFormat(e.ToString());
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
                    Debug.Log($"Received: {response}");
                    GameManager.HandlePackage(new Package(response));
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
            Debug.LogFormat(e.ToString());
        }
    }


    public static void Send(string data)
    {
        Debug.Log("Sending " + data);
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
            Debug.LogFormat("Sent {0} bytes to server.", bytesSent);

        }
        catch (Exception e)
        {
            Debug.LogFormat(e.ToString());
        }
    }
    
    private static void Disconnect()
    {
        //TODO send logout data
        client.Shutdown(SocketShutdown.Both);
        client.Close();
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