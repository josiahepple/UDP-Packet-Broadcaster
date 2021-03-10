//UDP Packet Broadcaster by Josiah Epple. Simple program to demonstrate Sleep-On-LAN when used in conjunction with https://github.com/josiahepple/Sleep-On-LAN.
//This program can broadcast both the conventional Wake-On-LAN and a custom-defined Sleep-On-LAN packet to the specified MAC Address.

using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SleepOnLAN
{
     class Program
     {
          static void Main(string[] args)
          {
               int portNumber = 9;
               int option = 3;
               byte[] packet;

               IPEndPoint broadcastPoint = new IPEndPoint(IPAddress.Broadcast, portNumber);
               UdpClient udpClient = new UdpClient();

               udpClient.EnableBroadcast = true;
               udpClient.Connect(broadcastPoint);

               while (option == 3)
               {
                    option = DecisionSelector();
               }

               packet = CraftPacket(option);

               udpClient.Send(packet, packet.Length);
          }

          static int DecisionSelector()
          {
               Console.WriteLine("Select which packet to send: ");
               Console.WriteLine("1. Wake-On LAN");
               Console.WriteLine("2. Sleep-On LAN");

               string decision = Console.ReadLine();
               if (decision == "1")
               {
                    return 1;
               }
               else if (decision == "2")
               {
                    return 2;
               }
               else
               {
                    Console.WriteLine("Invalid Input.");
                    return 3;
               }
          }

          static byte[] CraftPacket(int option)
          {
               byte[] packet = new byte[102];
               byte[] macAddress = new byte[6];
               UInt64 macAddressAsInteger;
               
               Console.WriteLine("Enter target Mac Address:");
               Console.WriteLine("E.g., 3A104C729A45");
               macAddressAsInteger = UInt64.Parse(Console.ReadLine(), System.Globalization.NumberStyles.HexNumber);

               for(int i = 5; i >= 0; i--)
               {
                    macAddress[i] = (byte)(macAddressAsInteger % 256);
                    macAddressAsInteger /= 256;
               }

               if (option == 1)
               {
                    for (int i = 0; i < 6; i++)
                    {
                         packet[i] = 0xff;
                    }
               }
               else
               {
                    for (int i = 0; i < 6; i++)
                    {
                         packet[i] = (byte)i;
                    }
               }
               for (int i = 6; i < 102;)
               {
                    for (int j = 0; j < 6; j++)
                    {
                         packet[i++] = macAddress[j];
                    }
               }
               return packet;
          }
     }
}