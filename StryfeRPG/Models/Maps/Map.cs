using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Models.Characters;
using StryfeRPG.System;
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

        public int[] collisionMap { get; set; }

        public int width { get; set; }
        public int height { get; set; }

        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }

        public List<Character> npcs { get; set; }
        public MapObject playerReference { get; set; }

        public Map(TmxMap tmxMap, Texture2D tileset)
        {
            this.tmxMap = tmxMap;
            this.tileset = tileset;

            width = tmxMap.Width;
            height = tmxMap.Height;
            
            tilesetTilesWide = tileset.Width / Global.tileSize;
            tilesetTilesHigh = tileset.Height / Global.tileSize;

            npcs = new List<Character>();
            foreach (TmxObjectGroup group in tmxMap.ObjectGroups)
            {
                foreach (TmxObject obj in group.Objects)
                {
                    if (obj.Type == "npc")
                    {
                        npcs.Add(new Character(obj));
                    } else if (obj.Type == "player")
                    {
                        playerReference = new MapObject(obj);
                    }
                }
            }

            PopulateCollisions();
        }

        public void PopulateCollisions()
        {
            collisionMap = new int[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    foreach (TmxLayer layer in tmxMap.Layers)
                    {
                        if (layer.Tiles[y * width + x].Gid != 0 &&
                            layer.Properties.ContainsKey("collision") &&
                            layer.Properties["collision"] == "true")
                        {
                            collisionMap[y * width + x] = 1;
                        }
                    }
                }
            }
        }

        public int GetCollision(Vector2 movement)
        {
            if (movement.X >= 0 && movement.Y >= 0 && movement.X < width && movement.Y < height)
                return collisionMap[(int)movement.Y * width + (int)movement.X];
            return 1;
        }
    }
}
