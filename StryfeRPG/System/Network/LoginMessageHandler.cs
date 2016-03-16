using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using StryfeRPG.Scenes;
using StryfeRPG.System.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Network
{
    public class LoginMessageHandler : IMessageHandler
    {
        public void Handle(SROrderMessage msg)
        {
            switch (msg.type)
            {
                case OrderType.UpdateServerList:
                    if (msg.args.ContainsKey(ArgumentName.ServerList))
                        UpdateServerList(msg.args[ArgumentName.ServerList] as List<ServerInfo>);
                    break;
                case OrderType.SetPlayerInfo:
                    if (msg.args.ContainsKey(ArgumentName.PlayerInfo))
                        SetPlayerInfo(msg.args[ArgumentName.PlayerInfo] as PlayerInfo);
                    break;
            }
        }

        public void UpdateServerList(List<ServerInfo> info)
        {
            foreach (ServerInfo s in info)
                Console.WriteLine("{0} - {1}:{2}", s.name, s.host, s.port);
        }

        public void SetPlayerInfo(PlayerInfo info)
        {
            GameState.Instance.playerInfo = info;
            ClientHandler.Instance.SetHandler(new IngameMessageHandler());

            SRActionMessage msg = new SRActionMessage(ActionType.GetMapData, ServiceType.Map);
            msg.args[ArgumentName.PlayerId] = info.id;
            ClientHandler.Instance.SendMessage(msg);
        }
    }
}
