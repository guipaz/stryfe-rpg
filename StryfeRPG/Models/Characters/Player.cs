using Microsoft.Xna.Framework;
using StryfeRPG.Models.Maps;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Characters
{
    public class Player : Character
    {
        public Player(TmxObject obj, Tileset tileset) : base(obj, tileset)
        {
            name = "Stryfe";
            nameColor = Color.White;
        }
    }
}
