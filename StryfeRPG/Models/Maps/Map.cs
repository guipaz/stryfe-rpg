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
        public TmxMap TmxMap { get; set; }
        public List<Tileset> Tilesets { get; set; }
        public List<Character> Npcs { get; set; } //TODO: maybe change it to generic MapObject
        public Player PlayerReference { get; set; }

        public int[] CollisionMap { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Map(TmxMap tmxMap)
        {
            this.TmxMap = tmxMap;

            Width = tmxMap.Width;
            Height = tmxMap.Height;

            Tilesets = new List<Tileset>();
            foreach (TmxTileset tmxTileset in tmxMap.Tilesets)
            {
                Tilesets.Add(new Tileset(tmxTileset));
            }

            Npcs = new List<Character>();
            foreach (TmxObjectGroup group in tmxMap.ObjectGroups)
            {
                foreach (TmxObject obj in group.Objects)
                {
                    if (obj.Type == "npc")
                    {
                        Npcs.Add(new Character(obj, GetTileset(obj.Tile.Gid)));
                    } else if (obj.Type == "player")
                    {
                        PlayerReference = new Player(obj, GetTileset(obj.Tile.Gid));
                    }
                }
            }

            PopulateCollisions();
        }

        public void PopulateCollisions()
        {
            CollisionMap = new int[Width * Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    foreach (TmxLayer layer in TmxMap.Layers)
                    {
                        if (layer.Tiles[y * Width + x].Gid != 0 &&
                            layer.Properties.ContainsKey("collision") &&
                            layer.Properties["collision"] == "true")
                        {
                            CollisionMap[y * Width + x] = 1;
                        }
                    }
                }
            }
        }

        public int GetCollision(Vector2 movement)
        {
            if (movement.X >= 0 && movement.Y >= 0 && movement.X < Width && movement.Y < Height)
                return CollisionMap[(int)movement.Y * Width + (int)movement.X];
            return 1;
        }

        public Tileset GetTileset(int gid)
        {
            foreach (Tileset tileset in Tilesets)
            {
                if (gid >= tileset.FirstGid && gid <= tileset.FinalGid)
                {
                    return tileset;
                }
            }

            return Tilesets[0];
        }

        public MapObject GetObject(Vector2 position)
        {
            // Checks NPCs
            foreach (MapObject npc in Npcs)
            {
                if (npc.MapPosition == position)
                    return npc;
            }

            return null;
        }
    }
}
