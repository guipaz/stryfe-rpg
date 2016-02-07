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
        public List<Tileset> tilesets { get; set; }
        public List<Character> npcs { get; set; } //TODO: maybe change it to generic MapObject
        public Character playerReference { get; set; }

        public int[] collisionMap { get; set; }

        public int width { get; set; }
        public int height { get; set; }

        public Map(TmxMap tmxMap)
        {
            this.tmxMap = tmxMap;

            width = tmxMap.Width;
            height = tmxMap.Height;

            tilesets = new List<Tileset>();
            foreach (TmxTileset tmxTileset in tmxMap.Tilesets)
            {
                tilesets.Add(new Tileset(tmxTileset));
            }

            npcs = new List<Character>();
            foreach (TmxObjectGroup group in tmxMap.ObjectGroups)
            {
                foreach (TmxObject obj in group.Objects)
                {
                    if (obj.Type == "npc")
                    {
                        npcs.Add(new Character(obj, GetTileset(obj.Tile.Gid)));
                    } else if (obj.Type == "player")
                    {
                        playerReference = new Character(obj, GetTileset(obj.Tile.Gid));
                        Player.DefineInstance(playerReference);
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

        public Tileset GetTileset(int gid)
        {
            foreach (Tileset tileset in tilesets)
            {
                if (gid >= tileset.firstGid && gid <= tileset.finalGid)
                {
                    return tileset;
                }
            }

            return tilesets[0];
        }
    }
}
