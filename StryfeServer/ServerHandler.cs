using Lidgren.Network;
using StryfeCore.Network;
using StryfeServer.Models;
using StryfeServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StryfeServer
{
    public class ServerHandler : IDisposable
    {
        NetPeerConfiguration config;
        NetServer server;

        public void Run()
        {
            // Server initialization
            config = new NetPeerConfiguration("stryferpg");
            config.Port = 1234;

            server = new NetServer(config);
            server.Start();

            //TODO: services initialization

            // Messages receipt
            bool stop = false;
            NetIncomingMessage message;
            while (!stop)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            SRActionMessage action = NetworkSerializer.DeserializeObject<SRActionMessage>(message.ReadBytes(message.LengthBytes));

                            switch (action.serviceType)
                            {
                                case ServiceType.Login:
                                    LoginService.Instance.Handle(action, message.SenderConnection);
                                    break;
                                case ServiceType.Char:
                                    break;
                                case ServiceType.Map:
                                    MapService.Instance.Handle(action, message.SenderConnection);
                                    break;
                            }

                            break;

                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine("StatusChanged");
                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    Console.WriteLine("Connected");
                                    break;
                            }
                            
                            break;

                        case NetIncomingMessageType.DebugMessage:
                            Console.WriteLine(message.ReadString());
                            break;

                        default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            break;
                    }
                }

                //stop = Console.ReadKey().Key == ConsoleKey.S;
                Thread.Sleep(1);
            }
        }

        public void SendMessageToPlayer(SROrderMessage msg, NetConnection conn)
        {
            server.SendMessage(CreateMessage(msg), conn, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendMessageToMany(SROrderMessage msg, List<NetConnection> conns)
        {
            server.SendMessage(CreateMessage(msg), conns, NetDeliveryMethod.ReliableOrdered, 0);
        }

        NetOutgoingMessage CreateMessage(object contents)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(NetworkSerializer.SerializeObject(contents));
            return msg;
        }

        public void Dispose()
        {

        }

        // Singleton stuff
        private static ServerHandler instance;
        protected ServerHandler() { }
        public static ServerHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerHandler();
                }
                return instance;
            }
        }
    }
}
