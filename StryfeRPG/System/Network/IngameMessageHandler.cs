using Microsoft.Xna.Framework;
using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using StryfeRPG.Managers;
using StryfeRPG.Scenes;
using StryfeRPG.System.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Network
{
    public class IngameMessageHandler : IMessageHandler
    {
        public void Handle(SROrderMessage msg)
        {
            switch (msg.type)
            {
                case OrderType.UpdateMapInfo:
                    UpdateMapInfo(msg.args);
                    break;
                case OrderType.UpdateNPlayerInfo:
                    UpdateNPlayerInfo(msg.args[ArgumentName.PlayerInfo] as PlayerInfo);
                    break;
            }
        }

        void UpdateMapInfo(Dictionary<ArgumentName, object> info)
        {
            // Sets players around
            Stryfe.Instance.ChangeScene(Scene.SceneType.Game);
            MapManager.Instance.UpdatePlayers(info[ArgumentName.VisiblePlayers] as List<PlayerInfo>);

            // Updates itself for other players
            SRActionMessage msg = new SRActionMessage(ActionType.UpdatePosition, ServiceType.Map);
            msg.args[ArgumentName.PlayerInfo] = GameState.Instance.playerInfo;
            ClientHandler.Instance.SendMessage(msg);

            // Sets NPCs around

            // Sets enemies around
        }

        void UpdateNPlayerInfo(PlayerInfo info)
        {
            MapManager.Instance.UpdatePlayer(info);
        }
    }
}
