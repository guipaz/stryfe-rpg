using Microsoft.Xna.Framework;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Characters
{
    public class Player : Character
    {
        public bool Move(Vector2 movement)
        {
            isMoving = true;

            destinationPosition = (mapPosition + movement) * Global.tileSize;

            return true;
        }

        // Singleton stuff
        private static Player instance;
        protected Player() { }
        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }
    }
}
