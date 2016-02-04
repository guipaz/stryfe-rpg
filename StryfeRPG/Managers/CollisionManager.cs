using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class CollisionManager
    {
        public bool GetCollision(int x, int y)
        {
            Map map = MapManager.Instance.currentMap;

            // Checks map collision
            int collision = map.GetCollision(x, y);

            // Checks player collision
            collision = Player.Instance.positionX == x && Player.Instance.positionY == y ? 1 : collision;

            //TODO: Checks NPC collision

            return collision == 1;
        }

        // Singleton stuff
        private static CollisionManager instance;
        protected CollisionManager() { }
        public static CollisionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionManager();
                }
                return instance;
            }
        }
    }
}
