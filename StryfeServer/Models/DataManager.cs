using Lidgren.Network;
using StryfeCore.Network.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeServer.Models
{
    public class DataManager
    {
        public Dictionary<int, NetConnection> Connections { get; private set; }
        public Dictionary<int, PlayerInfo> Players { get; private set; }

        public void UpdatePlayer(int id, PlayerInfo info)
        {
            Players[id] = info;
        }

        public void UpdateConnection(int id, NetConnection conn)
        {
            Connections[id] = conn;
        }

        // Singleton stuff
        private static DataManager instance;
        protected DataManager()
        {
            Connections = new Dictionary<int, NetConnection>();
            Players = new Dictionary<int, PlayerInfo>();
        }
        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }
    }
}
