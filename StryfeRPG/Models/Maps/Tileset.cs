using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Maps
{
    public class Tileset
    {
        public Texture2D Texture { get; set; }

        public int FirstGid { get; set; }
        public int FinalGid { get; set; }

        public int TilesWide { get; set; }
        public int TilesHigh { get; set; }

        public Tileset(TmxTileset tmxTileset)
        {
            Texture = Global.GetTexture(tmxTileset.Name);

            FirstGid = tmxTileset.FirstGid;
            FinalGid = FirstGid + (int)tmxTileset.TileCount-1;

            TilesWide = Texture.Width / Global.TileSize;
            TilesHigh = Texture.Height / Global.TileSize;
        }
    }
}
