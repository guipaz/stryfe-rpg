using Microsoft.Xna.Framework;
using StryfeCore.Network.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Runtime
{
    public class GameState
    {
        public PlayerInfo playerInfo { get; private set; }

        public void SetPlayerInfo(PlayerInfo info)
        {
            playerInfo = info;
            if (Global.Player != null)
                Global.Player.SetInfo(info);
        }

        // Singleton stuff
        private static GameState instance;
        protected GameState() { }
        public static GameState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameState();
                }
                return instance;
            }
        }
    }
}
