using Lidgren.Network;
using Microsoft.Xna.Framework;
using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using StryfeServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeServer.Services
{
    public class MapService : IService
    {
        public void Handle(SRActionMessage msg, NetConnection conn)
        {
            switch (msg.type)
            {
                case ActionType.GetMapData:
                    SendMapData((int)msg.args[ArgumentName.PlayerId], conn);
                    break;
                case ActionType.UpdatePosition:
                    UpdatePlayerPosition(msg.args, conn);
                    break;
                default:
                    break;
            }
        }

        void SendMapData(int playerId, NetConnection conn)
        {
            List<PlayerInfo> players = DataManager.Instance.Players.Values.ToList();
            //TODO: check surroundings

            SROrderMessage msg = new SROrderMessage(OrderType.UpdateMapInfo, new Dictionary<ArgumentName, object>()
            {
                { ArgumentName.VisiblePlayers, players }
            });
            ServerHandler.Instance.SendMessageToPlayer(msg, conn);
        }

        void UpdatePlayerPosition(Dictionary<ArgumentName, object> args, NetConnection conn)
        {
            PlayerInfo info = args[ArgumentName.PlayerInfo] as PlayerInfo;

            SROrderMessage msg = new SROrderMessage(OrderType.UpdateNPlayerInfo);
            msg.args[ArgumentName.PlayerInfo] = info;

            //TODO: check surroundings
            List<NetConnection> conns = DataManager.Instance.Connections.Values.ToList();
            conns.Remove(conn);

            if (conns.Count > 0)
                ServerHandler.Instance.SendMessageToMany(msg, conns);
        }

        // Singleton stuff
        private static MapService instance;
        protected MapService() { }
        public static MapService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapService();
                }
                return instance;
            }
        }
    }
}
