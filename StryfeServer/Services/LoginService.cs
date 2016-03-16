using Lidgren.Network;
using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeServer.Services
{
    public class LoginService : IService
    {
        public void Handle(SRActionMessage msg, NetConnection conn)
        {
            switch (msg.type)
            {
                case ActionType.Login:
                    if (msg.args.ContainsKey(ArgumentName.LoginUsername) &&
                        msg.args.ContainsKey(ArgumentName.LoginPassword))
                    {
                        DoLogin(msg.args[ArgumentName.LoginUsername] as string,
                                msg.args[ArgumentName.LoginPassword] as string,
                                conn);

                    }
                    break;
                default:
                    break;
            }
        }

        public void DoLogin(string username, string password, NetConnection conn)
        {
            //TODO: validate login
            Console.WriteLine("{0} - {1}", username, password);

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
