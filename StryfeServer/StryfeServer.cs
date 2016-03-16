using Lidgren.Network;
using StryfeCore.Network;
using StryfeCore.Network.Actions;
using StryfeCore.Network.Messages;
using StryfeCore.Network.Orders;
using StryfeServer.Handlers;
using StryfeServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StryfeServer
{
    class StryfeServer
    {
        static void Main(string[] args)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("stryferpg");
            config.Port = 1234;

            NetServer server = new NetServer(config);
            server.Start();

            int nextId = 1;

            NetIncomingMessage message;
            while (true)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            SActionMessage msgData = NetworkSerializer.DeserializeObject<SActionMessage>(message.ReadBytes(message.LengthBytes));

                            switch (msgData.type)
                            {
                                case ActionType.MapLoaded:
                                    // Sets default position
                                    SOrderMessage orderMsg = new SOrderMessage();
                                    orderMsg.type = OrderType.MovePlayer;

                                    SOrderMovePlayer order = new SOrderMovePlayer();
                                    order.id = nextId;
                                    order.x = 15;
                                    order.y = 12;
                                    orderMsg.order = order;

                                    nextId++;

                                    NetOutgoingMessage msg = server.CreateMessage();
                                    msg.Write(NetworkSerializer.SerializeObject(orderMsg));

                                    server.SendMessage(msg, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);

                                    // Warns other players
                                    SOrderPlayerInformation info = new SOrderPlayerInformation();
                                    info.id = nextId;
                                    info.x = order.x;
                                    info.y = order.y;

                                    orderMsg = new SOrderMessage();
                                    orderMsg.type = OrderType.PlayerInformation;
                                    orderMsg.order = info;

                                    msg = server.CreateMessage();
                                    msg.Write(NetworkSerializer.SerializeObject(orderMsg));

                                    server.SendMessage(msg, DataHolder.GetAllConnections(), NetDeliveryMethod.ReliableOrdered, 0);

                                    break;

                                case ActionType.Movement:
                                    // Warns other players
                                    SActionMovement mov = msgData.action as SActionMovement;

                                    SOrderPlayerInformation info2 = new SOrderPlayerInformation();
                                    info2.id = nextId;
                                    info2.x = mov.x;
                                    info2.y = mov.y;

                                    orderMsg = new SOrderMessage();
                                    orderMsg.type = OrderType.PlayerInformation;
                                    orderMsg.order = info2;
                                    
                                    msg = server.CreateMessage();
                                    msg.Write(NetworkSerializer.SerializeObject(orderMsg));

                                    server.SendMessage(msg, DataHolder.GetAllConnections(), NetDeliveryMethod.ReliableOrdered, 0);

                                    break;
                            }

                            break;

                        case NetIncomingMessageType.StatusChanged:
                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    DataHolder.AddConnection(message.SenderConnection);
                                    
                                    break;
                            }

                            Console.WriteLine(message.ReadString());

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
    }
}
