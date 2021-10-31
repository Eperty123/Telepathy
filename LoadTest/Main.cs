﻿using System;
using System.Threading;

namespace Telepathy.LoadTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Both();
            }
            else if (args[0] == "server")
            {
                Server(args);
            }
            else if (args[0] == "client")
            {
                Client(args);
            }
            else if (args[0] == "timed")
            {
                Both(args);
            }
            else
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("   LoadTest");
                Console.WriteLine("   LoadTest server <port> (<seconds>)");
                Console.WriteLine("   LoadTest client <host> <port> <clients> (<seconds>)");
                Console.WriteLine("   LoadTest timed <port> <seconds>");
            }
        }

        public static void Both(string[] args = null)
        {
            int port = 1337;
            int seconds = 0;
            
            if(args != null)
            {
                port = int.Parse(args[1]);
                seconds = int.Parse(args[2]);
            }

            Thread serverThread = new Thread(() =>
            {

                RunServer.StartServer(port, seconds);
            });
            serverThread.IsBackground = false;
            serverThread.Start();

            // test 500 clients, which means 500+500 = 1000 connections total.
            // this should be enough for any server or MMO.
            RunClients.StartClients("127.0.0.1", port, 500, seconds);
        }

        public static void Server(string [] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: LoadTest server <port>");
                return;
            }
            int port = int.Parse(args[1]);

            RunServer.StartServer(port);

        }


        public static void Client(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: LoadTest client <host> <port> <clients>");
                return;
            }
            string ip = args[1];
            int port = int.Parse(args[2]);
            int clients = int.Parse(args[3]);

            RunClients.StartClients(ip, port, clients);
        }
    }
}
