using StryfeRPG.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Characters
{
    public class Character : Maps.MapObject
    {
        public Character() { }
        public Character(TmxObject obj, Tileset tileset) : base(obj, tileset) { }
    }
}
