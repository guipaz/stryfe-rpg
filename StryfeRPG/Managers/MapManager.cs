using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class MapManager
    {
        private static MapManager instance;

        protected MapManager() { }

        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }

        public void LoadMap(string mapName)
        {
            
        }
    }
}
