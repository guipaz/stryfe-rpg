using Microsoft.Xna.Framework;
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
        public bool GetCollision(Vector2 movement)
        {
            Map map = MapManager.Instance.currentMap;

            // Checks map collision
            int collision = map.GetCollision(movement);

            // Checks player collision
            collision = Player.Instance.mapPosition == movement ? 1 : collision;

            // Checks NPC collision
            foreach (MapObject obj in map.npcs)
            {
                collision = obj.mapPosition == movement ? 1 : collision;
            }

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
