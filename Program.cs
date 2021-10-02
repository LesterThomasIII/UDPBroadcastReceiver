using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;


namespace UDPBroadcastReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            //using socket constructor takes 3 parameters~
            //address family InterNetwork means we are using IPv4
            //SocketType Dgram means we are using data-gram media
            //ProtocolType Udp means we are using Udp protocols
            Socket sockBroadCastReceiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //IPEndpoints need 2 parameters
            //IPAddress designed to receive specific or any IP addresses
            //23000 is the port that the receiver is listening on the receiver must be on the same port received
            IPEndPoint ipEndPointLocal = new IPEndPoint(IPAddress.Any, 23000);
           //this Identifies the Ip Sender to validate they are acceptable
           IPEndPoint ipEpSender = new IPEndPoint(IPAddress.Any, 0);
           //this establishes who the Sender is and applies it to a string variable for reference
           EndPoint eSender = (EndPoint)ipEpSender;
            //byte arrays are used to receive data 
            byte[] receiveBuffer = new byte[512];
            //generate the string for inputting the data
            string dataReceived = String.Empty;
            
            //we must bind the socket on the machine
            //try is used to ID if there is a problem with this
            
            try
            {
                //values for standard uses
                //used to bind the socket to the local IP Endpoint
                sockBroadCastReceiver.Bind(ipEndPointLocal);
               //place this into an infinite loop to constantly receive info
               while (true)
               {
                   int nCountReceived = 0;
                   int ipSender = 0;
                   //receive into the buffer the amount of data and returns an integer
                    nCountReceived = sockBroadCastReceiver.Receive(receiveBuffer);
                   //this shows the received from info
                     ipSender = sockBroadCastReceiver.ReceiveFrom(receiveBuffer, ref eSender);
                   //this takes in the data and converts it to readable info in ACSII format
                   //this takes 3 parameters
                   //bytes is # of Bytes from the buffer array //!receiveBuffer!//
                   //Index is the index starting point for the conversion (i.e.0) //!0!//
                   //count is the count of bytes in the broadcast the number of bytes to convert in the broadcast //!nCountReceived!//
                   dataReceived = Encoding.ASCII.GetString(receiveBuffer, 0, nCountReceived);
                   //ouput and validate if the data has been received
                   Console.WriteLine(
                       "# Bytes Received: " + nCountReceived + " Information Transferred: " + dataReceived);
                   //TODO This one was acting Weird, will work on this some more
                   //print out who sent it
                  // Console.WriteLine("Received from: "+ ipSender.ToString());
                  
                   //clears the array receiveBuffer once data has been received
                   Array.Clear(receiveBuffer, 0, receiveBuffer.Length);

                   //echo function to send message along another path
                   if (!dataReceived.Equals("<ECHO>")) continue;
                   //this rebroadcasts the info back to the sender
                   sockBroadCastReceiver.SendTo(receiveBuffer, 0, nCountReceived, SocketFlags.None, eSender);
                   Console.WriteLine(" Echoed Back ");

               }
            }
            //if you get this far you've broken something... next step is to figure out what
            catch (Exception e)
            {
                //the What that broke...
                Console.WriteLine(e);
                
            }
          
        }
    }
}
