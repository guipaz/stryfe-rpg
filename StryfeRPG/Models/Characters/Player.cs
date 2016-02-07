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
        // Singleton stuff
        private static Player instance;
        protected Player()
        {
            texture = Global.GetTexture("charsets");
            textureId = 1;
            name = "Stryfe";
            nameColor = Color.White;
        }
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
