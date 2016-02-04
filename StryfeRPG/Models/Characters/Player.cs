using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Characters
{
    public class Player : Character
    {
        public bool Move(int x, int y)
        {
            isMoving = true;

            destinationX = positionX + tileSize * x;
            destinationY = positionY + tileSize * y;

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
