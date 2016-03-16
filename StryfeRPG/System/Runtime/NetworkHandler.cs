using Lidgren.Network;
using Microsoft.Xna.Framework;
using StryfeCore.Network;
using StryfeCore.Network.Actions;
using StryfeCore.Network.Messages;
using StryfeCore.Network.Orders;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StryfeRPG
{
    public class NetworkHandler
    {
        NetClient client;
        bool killProcess = false;

        public void Start()
        {
            SetupClient();
            
            NetIncomingMessage message;
            while (!killProcess)
            {
                while ((message = client.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:

                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    //TODO: login
                                    break;
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            SOrderMessage orderMsg = NetworkSerializer.DeserializeObject<SOrderMessage>(message.ReadBytes(message.LengthBytes));

                            switch (orderMsg.type)
                            {
                                case OrderType.MovePlayer:
                                    SOrderMovePlayer order = orderMsg.order as SOrderMovePlayer;
                                    Global.Player.MapPosition = new Vector2(order.x, order.y);
                                    Global.Player.id = order.id;
                                    break;

                                case OrderType.PlayerInformation:
                                    SOrderPlayerInformation info = orderMsg.order as SOrderPlayerInformation;
                                    Global.SetPlayerInformation(info);
                                    break;
                            }

                            break;
                    }
                }

                Thread.Sleep(1);
            }
        }

        public void Stop()
        {
            killProcess = true;
        }

        void SetupClient()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("stryferpg");
            client = new NetClient(config);
            client.Start();
            client.Connect("localhost", 1234);
        }

        public void SendMessage(ActionType type, SAction action)
        {
            SActionMessage actionMsg = new SActionMessage(type, action);

            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(NetworkSerializer.SerializeObject(actionMsg));

            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        // Singleton stuff
        private static NetworkHandler instance;
        protected NetworkHandler() { }
        public static NetworkHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkHandler();
                }
                return instance;
            }
        }
    }
}
