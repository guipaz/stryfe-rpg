using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TiledSharp;

namespace StryfeRPG.Models.Maps
{
    public class Map
    {
        public TmxMap tmxMap { get; set; }
        public Texture2D tileset { get; set; }

        public int tileWidth { get; set; }
        public int tileHeight { get; set; }
        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }
    }
}
