using Lidgren.Network;
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
    public class LoginService : IService
    {
        int debugId = 1;

        public void Handle(SRActionMessage msg, NetConnection conn)
        {
            switch (msg.type)
            {
                case ActionType.DoLogin:
                    if (msg.args.ContainsKey(ArgumentName.LoginUsername) &&
                        msg.args.ContainsKey(ArgumentName.LoginPassword))
                    {
                        DoLogin(msg.args[ArgumentName.LoginUsername] as string,
                                msg.args[ArgumentName.LoginPassword] as string,
                                conn);
                    }
                    break;
                case ActionType.GetServerList:
                    SendServerList(conn);
                    break;
                default:
                    break;
            }
        }

        public void SendServerList(NetConnection conn)
        {
            SROrderMessage msg = new SROrderMessage(OrderType.UpdateServerList);
            msg.args = new Dictionary<ArgumentName, object>()
                        {
                            {
                                ArgumentName.ServerList, new List<ServerInfo>() {
                                                         new ServerInfo("Normal", "localhost", 1234) }
                            }
                        };

            ServerHandler.Instance.SendMessageToPlayer(msg, conn);
        }

        public void DoLogin(string username, string password, NetConnection conn)
        {
            //TODO: validate login
            Console.WriteLine("{0} - {1}", username, password);

            //TODO: get player information
            PlayerInfo info = new PlayerInfo();
            info.name = "Stryfe";
            info.x = 20;
            info.y = 15;
            info.id = debugId;

            debugId++;

            DataManager.Instance.UpdateConnection(info.id, conn);
            DataManager.Instance.UpdatePlayer(info.id, info);

            SROrderMessage msg = new SROrderMessage(OrderType.SetPlayerInfo, new Dictionary<ArgumentName, object>() { { ArgumentName.PlayerInfo, info } });
            ServerHandler.Instance.SendMessageToPlayer(msg, conn);
        }
        
        // Singleton stuff
        private static LoginService instance;
        protected LoginService() { }
        public static LoginService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginService();
                }
                return instance;
            }
        }
    }
}
