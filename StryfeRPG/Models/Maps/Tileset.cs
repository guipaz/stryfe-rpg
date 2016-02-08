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
        public Texture2D texture { get; set; }

        public int firstGid { get; set; }
        public int finalGid { get; set; }

        public int tilesWide { get; set; }
        public int tilesHigh { get; set; }

        public Tileset(TmxTileset tmxTileset)
        {
            texture = Global.GetTexture(tmxTileset.Name);

            firstGid = tmxTileset.FirstGid;
            finalGid = firstGid + (int)tmxTileset.TileCount;

            tilesWide = texture.Width / Global.TileSize;
            tilesHigh = texture.Height / Global.TileSize;
        }
    }
}
