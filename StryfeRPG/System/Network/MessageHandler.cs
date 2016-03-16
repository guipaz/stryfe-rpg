using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Network
{
    public class MessageHandler
    {
        public void Handle(SROrderMessage msg)
        {
            switch (msg.type)
            {
                case OrderType.UpdateServerList:
                    if (msg.args.ContainsKey(ArgumentName.ServerList))
                        UpdateServerList(msg.args[ArgumentName.ServerList] as List<ServerInfo>);
                    break;
            }
        }

        public void UpdateServerList(List<ServerInfo> info)
        {
            foreach (ServerInfo s in info)
                Console.WriteLine("{0} - {1}:{2}", s.name, s.host, s.port);
        }

        // Singleton stuff
        private static MessageHandler instance;
        protected MessageHandler() { }
        public static MessageHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageHandler();
                }
                return instance;
            }
        }
    }
}
