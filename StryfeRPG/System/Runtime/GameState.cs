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
        public PlayerInfo playerInfo;

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
