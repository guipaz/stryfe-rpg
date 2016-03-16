using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeServer
{
    public static class DataHolder
    {
        static List<NetConnection> connections = new List<NetConnection>();

        public static void AddConnection(NetConnection conn)
        {
            connections.Add(conn);
        }

        public static List<NetConnection> GetAllConnections()
        {
            return connections;
        }
    }
}
