using Microsoft.Xna.Framework;
using StryfeRPG.Models.Items;
using StryfeRPG.Models.Maps;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Characters
{
    public class Player : NPC
    {
        public List<Item> Inventory;

        public Player(TmxObject obj, Tileset tileset) : base(obj, tileset, "")
        {
            Name = "Stryfe";
            Attributes = new AttributeSheet();
        }
    }
}
