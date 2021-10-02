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
            IPEndPoint ipEpLocal = new IPEndPoint(IPAddress.Any, 23000);
            //byte arrays are used to receive data 
            byte[] receiveBuffer = new byte[512];
            //generate the string for inputting the data
            string dataRcvd= String.Empty;
            //this Identifies the Ip Sender to validate they are acceptable
            IPEndPoint ipEpSender = new IPEndPoint(IPAddress.Any, 0);
           //this establishes who the Sender is and applies it to a string variable for reference
           EndPoint eSender = (EndPoint)ipEpSender;
           //values for standard uses
          

          
            //try is used to ID if there is a problem with the connection!
            try
            {
                //we must bind the socket on the machine
                //used to bind the socket to the local IP Endpoint
                sockBroadCastReceiver.Bind(ipEpLocal);
               
                //place this into an infinite loop to constantly receive info
                while (true)
               {
                   //this shows the received from info and takes 2 parameters
                   //the byte array buffer 
                   //and the EndPointSender
                   int ipSender = sockBroadCastReceiver.ReceiveFrom(receiveBuffer, ref eSender);

                    //receive into the buffer the amount of data and returns an integer
                    int nCRcv = sockBroadCastReceiver.Receive(receiveBuffer);
                   //this takes in the data and converts it to readable info in ACSII format
                   //this takes 3 parameters
                   //bytes is # of Bytes from the buffer array //!receiveBuffer!//
                   //Index is the index starting point for the conversion (i.e.0) //!0!//
                   //count is the count of bytes in the broadcast the number of bytes to convert in the broadcast //!nCountReceived!//
                   dataRcvd = Encoding.ASCII.GetString(receiveBuffer, 0, nCRcv);
                   //ouput and validate if the data has been received
                   Console.WriteLine("# Bytes Rcv: " + nCRcv + Environment.NewLine
                                     +"Info: " + dataRcvd+ Environment.NewLine+"from: " 
                                     + eSender.ToString());

                   //clears the array receiveBuffer once data has been received
                   Array.Clear(receiveBuffer, 0, receiveBuffer.Length);

                 /*  // TODO echo function to send message along another path
                   if (!dataRcvd.Equals("<ECHO>")) continue;
                   //this rebroadcasts the info back to the sender
                   sockBroadCastReceiver.SendTo(receiveBuffer, 0, nCRcv, SocketFlags.None, eSender);
                   Console.WriteLine(" Echoed Back ");
                 */

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
