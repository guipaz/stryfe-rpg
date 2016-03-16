using Lidgren.Network;
using StryfeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StryfeRPG.System.Network
{
    public class ClientHandler
    {
        NetPeerConfiguration config;
        NetClient client;

        bool stop = false;

        public void Run()
        {
            // Client initialization
            config = new NetPeerConfiguration("stryferpg");
            client = new NetClient(config);
            client.Start();
            client.Connect("localhost", 1234);
            
            // Messages receipt
            NetIncomingMessage message;
            while (!stop)
            {
                while ((message = client.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            SROrderMessage msg = NetworkSerializer.DeserializeObject<SROrderMessage>(message.ReadBytes(message.LengthBytes));
                            MessageHandler.Instance.Handle(msg);
                            break;

                        case NetIncomingMessageType.StatusChanged:
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
                
                Thread.Sleep(1);
            }
        }

        public void SendMessage(SRActionMessage action)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(NetworkSerializer.SerializeObject(action));
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void Stop()
        {
            stop = true;
        }

        // Singleton stuff
        private static ClientHandler instance;
        protected ClientHandler() { }
        public static ClientHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientHandler();
                }
                return instance;
            }
        }
    }
}
