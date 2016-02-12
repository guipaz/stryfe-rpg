using StryfeRPG.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Characters
{
    public class SpriteSheet
    {
        public Tileset Tileset { get; set; }

        public int GidDown { get; set; }
        public int GidUp { get; set; }
        public int GidLeft { get; set; }
        public int GidRight { get; set; }

        public SpriteSheet(int textureId, Tileset tileset)
        {
            this.Tileset = tileset;

            int sheetsWide = tileset.TilesWide / 3;

            int y = textureId / (tileset.TilesWide * 4);
            int x = ((textureId - (y * tileset.TilesWide * 4)) / 3) % sheetsWide;

            GidDown = 1 + (x * 3) + (y * 4 * tileset.TilesWide);
            GidLeft = GidDown + tileset.TilesWide;
            GidRight = GidDown + tileset.TilesWide * 2;
            GidUp = GidDown + tileset.TilesWide * 3;
        }
    }
}
