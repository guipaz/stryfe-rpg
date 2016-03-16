using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network.SharedModels
{
    [Serializable]
    public class ServerInfo
    {
        public string name;
        public string host;
        public int port;
        
        public ServerInfo(string name, string host, int port)
        {
            this.name = name;
            this.host = host;
            this.port = port;
        }
    }
}
