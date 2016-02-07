using StryfeRPG.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Characters
{
    public class CharacterSheet
    {
        public Tileset tileset { get; set; }

        public int gidDown { get; set; }
        public int gidUp { get; set; }
        public int gidLeft { get; set; }
        public int gidRight { get; set; }

        public CharacterSheet(int textureId, Tileset tileset)
        {
            this.tileset = tileset;

            int sheetsWide = tileset.tilesWide / 3;

            int y = textureId / (tileset.tilesWide * 4);
            int x = ((textureId - (y * tileset.tilesWide * 4)) / 3) % sheetsWide;

            gidDown = 1 + (x * 3) + (y * 4 * tileset.tilesWide);
            gidLeft = gidDown + tileset.tilesWide;
            gidRight = gidDown + tileset.tilesWide * 2;
            gidUp = gidDown + tileset.tilesWide * 3;
        }
    }
}
